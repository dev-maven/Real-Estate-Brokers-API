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
using Video = Urban.ng.Models.Video;

namespace Urban.ng.Controllers
{
    [Route("api/property/{propertyId}/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IPropertyRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private Cloudinary _cloudinary;
        public VideosController(IPropertyRepository repo, IMapper mapper,
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

        [HttpGet("{id}", Name = "GetVideo")]
        public async Task<IActionResult> GetVideo(int id)
        {
            var videoFromRepo = await _repo.GetVideo(id);

            var video = _mapper.Map<VideoReturnDto>(videoFromRepo);

            return Ok(video);
        }


        [HttpPost]
        public async Task<IActionResult> AddVideo(int propertyId, [FromForm] VideoCreateDto videoCreateDto)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != _context.Properties.FindAsync(propertyId).Result.UserId)
                return Unauthorized();

            var propertyFromRepo = await _repo.GetProperty(propertyId);

            var file = videoCreateDto.VideoFile;

            var uploadResult = new VideoUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {

                    var uploadParams = new VideoUploadParams()
                    {

                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500)
                          .Height(500)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            videoCreateDto.VideoUrl = uploadResult.Uri.ToString();
            videoCreateDto.VideoPublicId = uploadResult.PublicId;

            var video = _mapper.Map<Video>(videoCreateDto);

            propertyFromRepo.Videos.Add(video);

            if (await _repo.SaveAll())
            {
                var videoToReturn = _mapper.Map<VideoReturnDto>(video);

                return CreatedAtRoute("GetVideo", new { id = propertyId }, videoToReturn);
            }

            return BadRequest("Failed to add video");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int propertyId, int id)
        {
                        int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var property = await _repo.GetProperty(propertyId);

            if (!property.Videos.Any(p => p.VideoId == id))
                return Unauthorized();

            var videoFromRepo = await _repo.GetVideo(id);


            if (videoFromRepo.VideoPublicId != null)
            {
                var deleteParams = new DeletionParams(videoFromRepo.VideoPublicId);


                    deleteParams.ResourceType = ResourceType.Video;

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(videoFromRepo);
                }
            }

            if (videoFromRepo.VideoPublicId == null)
            {
                _repo.Delete(videoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to Delete");
        }




    }
}
