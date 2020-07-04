using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MavenDate.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Urban.ng.Data;
using Urban.ng.Dtos;
using Urban.ng.Helpers;
using Urban.ng.Models;
using Urban.ng.ViewModels;
using Video = Urban.ng.Models.Video;

namespace Urban.ng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public PropertyController(IPropertyRepository repo, IMapper mapper, UserManager<User> userManager, DataContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProperties([FromQuery]UserParams userParams)
        {
           
            var properties = await _repo.GetProperties(userParams);

            var propertiesToReturn = _mapper.Map<IEnumerable<PropertyForListDto>>(properties);

            Response.AddPagination(properties.CurrentPage, properties.PageSize, properties.TotalCount, properties.TotalPages);


            return Ok(propertiesToReturn);
        }

        [HttpGet("{id}/GetMyProperty", Name = "GetMyProperty")]
        public async Task<IActionResult> GetMyProperties([FromQuery]UserParams userParams)
        {

            var properties = await _repo.GetProperties(userParams);

            var propertiesToReturn = _mapper.Map<IEnumerable<PropertyForListDto>>(properties);

            Response.AddPagination(properties.CurrentPage, properties.PageSize, properties.TotalCount, properties.TotalPages);


            return Ok(propertiesToReturn);
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetProperty")]
        public async Task<IActionResult> GetProperty(int id)
        {
            var propertyFromRepo = await _repo.GetProperty(id);

            var property = _mapper.Map<PropertyReturnDto>(propertyFromRepo);

            return Ok(property);
        }
        
        [HttpPost("CreateProperty")]
        public async Task<IActionResult> AddProperty([FromBody] PropertyCreateDto propertyCreateDto)
        {
           var id = _userManager.GetUserId(User);
            int myId  = Convert.ToInt32(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var property = _mapper.Map<Property>(propertyCreateDto);
            property.UserId = myId;
                _repo.Add(property);
                if (await _repo.SaveAll())
                {
                    var propertyToReturn = _mapper.Map<PropertyReturnDto>(property);
                    return CreatedAtRoute("GetProperty", new { id = property.PropertyId }, propertyToReturn);
                }

            return BadRequest("Failed to add property");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));

            if (userId != _context.Properties.FindAsync(id).Result.UserId)
                return Unauthorized();

            var propertyFromRepo = await _repo.GetProperty(id);
            _repo.Delete(propertyFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("This property could not be deleted");    
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditProperty(int id, [FromBody] PropertyEditDto propertyEditDto)
        {
            int userId = Convert.ToInt32(_userManager.GetUserId(User));
            var propertyFromRepo = await _repo.GetProperty(id);

            if (userId != propertyFromRepo.UserId)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != propertyFromRepo.PropertyId)
            {
                return BadRequest();
            }


            _mapper.Map(propertyEditDto, propertyFromRepo);

            _context.Entry(propertyFromRepo).State = EntityState.Modified;



            if (await _repo.SaveAll())
                return NoContent();
            return BadRequest("Property update failed");



    }
    }
    }
    
