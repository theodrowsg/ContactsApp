using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Dtos;
using ContactsApp.API.Features.Users;
using ContactsApp.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ContactsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;

        public AuthController(IAuthRepository repo, IConfiguration config, IMediator mediator)
        {
            _mediator = mediator;
            _repo = repo;
            _config = config;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateInput createUserInput)
        {

            createUserInput.UserName = createUserInput.UserName.ToLower();

            if (await _repo.UserExists(createUserInput.UserName))
            {
                return BadRequest("username already exists");
            };

            var userToCreate = new User
            {
                UserName = createUserInput.UserName
            };

            var createdUser = await _repo.Register(userToCreate, createUserInput.Password);

            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginInput userLoginInput)
        {

            var userFromRepo = await _repo.Login(userLoginInput.UserName.ToLower(), userLoginInput.Password);
            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]{
                   new Claim(ClaimTypes.NameIdentifier , userFromRepo.Id.ToString()),
                   new Claim(ClaimTypes.Name, userFromRepo.UserName)
               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptior);

            var ppUrl = await _mediator.Send(new UserProfilePhotoQuery { UserId = userFromRepo.Id  } );

            return Ok(new
            {
                 token = tokenHandler.WriteToken(token),
                 ppUrl
            });


        }
    }
}