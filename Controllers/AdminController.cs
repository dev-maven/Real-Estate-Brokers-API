using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Urban.ng.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Urban.ng.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Urban.ng.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await (from user in _context.Users orderby user.UserName
                                    select new
                                    {
                                        Id = user.Id,
                                        Email = user.Email,
                                        FirstName = user.FirstName,
                                        LastNAme = user.LastName,
                                        DateJoined = user.DateRegistered,
                                        PhoneNumber = user.PhoneNumber,
                                        Roles = (from userRole in user.UserRoles
                                                join role in _context.Roles
                                                on userRole.RoleId
                                                equals role.Id
                                                select role.Name).ToList()
                                    }).ToListAsync();

            return Ok(userList);
        }

        [HttpPost("editRoles/{id}")]
        public async Task<IActionResult> EditRoles(int id, RoleEditViewModel roleEditViewModel)
        {
            var userId = id.ToString();
            var user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);
             
            var selectedRoles = roleEditViewModel.RoleNames;

            selectedRoles = selectedRoles ?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove the roles");
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = id.ToString();
            var user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (result.Succeeded)
            { 
              _context.Remove(user);

            await _context.SaveChangesAsync();

            return NoContent();
        }
            return BadRequest("Failed to remove the roles");

            throw new Exception("This user could not be deleted");
        }


    }
}