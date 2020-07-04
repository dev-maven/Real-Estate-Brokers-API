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
using Urban.ng.Models;
using Urban.ng.ViewModels;

namespace Urban.ng.Controllers
{
    [Route("api/property/{propertyId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPropertyRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private Cloudinary _cloudinary;
        public PhotosController(IPropertyRepository repo, IMapper mapper,
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

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoReturnDto>(photoFromRepo);

            return Ok(photo);
        }


        [HttpPost]
        public async Task<IActionResult> AddPhoto(int propertyId, [FromForm] PhotoCreateDto photoCreateDto)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != _context.Properties.FindAsync(propertyId).Result.UserId)
                return Unauthorized();

            var propertyFromRepo = await _repo.GetProperty(propertyId);

            var file = photoCreateDto.PhotoFile;

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

            photoCreateDto.PhotoUrl = uploadResult.Uri.ToString();
            photoCreateDto.PhotoPublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoCreateDto);

            if (!propertyFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            propertyFromRepo.Photos.Add(photo);

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoReturnDto>(photo);

                return CreatedAtRoute("GetPhoto", new { id = propertyId }, photoToReturn);
            }

            return BadRequest("Failed to add photo(s)");
        }


        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int propertyId, int id)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var propertyFromRepo = await _repo.GetProperty(propertyId);

            if (!propertyFromRepo.Photos.Any(p => p.PhotoId == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repo.GetMainPhotoForProperty(propertyId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Main photo not set");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int propertyId, int id)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var property = await _repo.GetProperty(propertyId);

            if (!property.Photos.Any(p => p.PhotoId == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You can't delete your main photo");

            if (photoFromRepo.PhotoPublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PhotoPublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PhotoPublicId == null)
            {
                _repo.Delete(photoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to Delete");
        }




    }
}