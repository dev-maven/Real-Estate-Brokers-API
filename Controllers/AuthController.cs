using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Urban.ng.Models;
using Urban.ng.ViewModels;

namespace Urban.ng.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IConfiguration config, IMapper mapper, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)

        {
            var userToCreate = _mapper.Map<User>(model);
            userToCreate.UserName = model.Email;


            var result = await _userManager.CreateAsync(userToCreate, model.Password);

            var userToReturn = _mapper.Map<UserLoginViewModel>(userToCreate);

            if (model.UserType == "Cooperate Agent")
            {
                await _userManager.AddToRoleAsync(userToCreate, "Cooperate Agent");
            }

            else if (model.UserType == "Customer (Buy/Rent)")
            {
                await _userManager.AddToRoleAsync(userToCreate, "Customer");
            }
            else if (model.UserType == "Independent Agent")
            {
                await _userManager.AddToRoleAsync(userToCreate, "Individual Agent");
            }
            else if (model.UserType == "Admin")
            {
                await _userManager.AddToRoleAsync(userToCreate, "Admin");
            } 
            else
            {
                await _userManager.AddToRoleAsync(userToCreate, "Owner");
            }


            if (result.Succeeded)
            {
                return CreatedAtRoute("GetUser",
                    new { controller = "Users", id = userToCreate.Id }, userToReturn);
            }

            return BadRequest(result.Errors);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            

            var user = await _userManager.FindByNameAsync(model.Email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
           

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                var userToReturn = _mapper.Map<UserLoginViewModel>(appUser);

                var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email)
            };

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds

                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var returnToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(returnToken);

                return Ok(new
                {
                    token,
                    user = userToReturn
                });

            }

            return Unauthorized();
        }

        
    }
}
