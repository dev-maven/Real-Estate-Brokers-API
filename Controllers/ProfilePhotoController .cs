using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MavenDate.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Urban.ng.Data;
using Urban.ng.Dtos;
using Urban.ng.Models;
using Urban.ng.ViewModels;

namespace Urban.ng.Controllers
{
    [Route("api/users/{userId}/profilephoto")]
    [ApiController]
    public class ProfilePhotoController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private Cloudinary _cloudinary;
        public ProfilePhotoController(IUserRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig,
            UserManager<User> userManager, DataContext context)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
              _cloudinaryConfig.Value.CloudName,
              _cloudinaryConfig.Value.ApiKey,
              _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetProfilePhoto")]
        public async Task<IActionResult> GetProfilePhoto(int id)
        {
            var profilephotoFromRepo = await _repo.GetProfilePhoto(id);

            var profilephoto = _mapper.Map<ProfilePhotoReturnDto>(profilephotoFromRepo);

            return Ok(profilephoto);
        }


        [HttpPost]
        public async Task<IActionResult> AddProfilePhoto(int userId, [FromForm] ProfilePhotoCreateDto profilePhotoCreateDto)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = profilePhotoCreateDto.ProfilePhotoFile;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {

                    var uploadParams = new ImageUploadParams()
                    {

                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500)
                          .Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            profilePhotoCreateDto.ProfilePhotoUrl = uploadResult.Uri.ToString();
            profilePhotoCreateDto.ProfilePhotoPublicId = uploadResult.PublicId;

            var profilePhoto = _mapper.Map<ProfilePhoto>(profilePhotoCreateDto);

            userFromRepo.ProfilePhoto.Add(profilePhoto);

            if (await _repo.SaveAll())
            {
                var profilePhotoToReturn = _mapper.Map<ProfilePhotoReturnDto>(profilePhoto);

                return CreatedAtRoute("GetProfilePhoto", new { id = userId }, profilePhotoToReturn);
            }

            return BadRequest("Failed to add profile photo");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfilePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.ProfilePhoto.Any(p => p.ProfilePhotoId == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetProfilePhoto(id);

            if (photoFromRepo.ProfilePhotoPublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.ProfilePhotoPublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.ProfilePhotoPublicId == null)
            {
                _repo.Delete(photoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to Delete");
        }




    }
}