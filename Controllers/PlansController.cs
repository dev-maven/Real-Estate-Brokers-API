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
    [Route("api/property/{propertyId}/plans")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly IPropertyRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private Cloudinary _cloudinary;
        public PlansController(IPropertyRepository repo, IMapper mapper,
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

        [HttpGet("{id}", Name = "GetPlan")]
        public async Task<IActionResult> GetPlan(int id)
        {
            var planFromRepo = await _repo.GetPlan(id);

            var plan = _mapper.Map<PlanReturnDto>(planFromRepo);

            return Ok(plan);
        }


        [HttpPost]
        public async Task<IActionResult> AddPlan(int propertyId, [FromForm] PlanCreateDto planCreateDto)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != _context.Properties.FindAsync(propertyId).Result.UserId)
                return Unauthorized();

            var propertyFromRepo = await _repo.GetProperty(propertyId);

            var file = planCreateDto.PlanFile;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {

                    var uploadParams = new ImageUploadParams()
                    {

                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500)
                          .Height(500)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            planCreateDto.PlanUrl = uploadResult.Uri.ToString();
            planCreateDto.PlanPublicId = uploadResult.PublicId;

            var plan = _mapper.Map<Plan>(planCreateDto);

            propertyFromRepo.Plans.Add(plan);

            if (await _repo.SaveAll())
            {
                var planToReturn = _mapper.Map<PlanReturnDto>(plan);

                return CreatedAtRoute("GetPlan", new { id = propertyId }, planToReturn);
            }

            return BadRequest("Failed to add plan(s)");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int propertyId, int id)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var property = await _repo.GetProperty(propertyId);

            if (!property.Plans.Any(p => p.PlanId == id))
                return Unauthorized();

            var planFromRepo = await _repo.GetPlan(id);


            if (planFromRepo.PlanPublicId != null)
            {
                var deleteParams = new DeletionParams(planFromRepo.PlanPublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(planFromRepo);
                }
            }

            if (planFromRepo.PlanPublicId == null)
            {
                _repo.Delete(planFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to Delete");
        }




    }
}