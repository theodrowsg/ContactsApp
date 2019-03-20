using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Features.Users;
using ContactsApp.API.Helpers;
using ContactsApp.API.Models;
using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
         private readonly IMediator _mediator;
         public UsersController(IMediator mediator)
        {
               _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {         
          userParams.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
          var usersToReturn = await _mediator.Send(new UsersQuery{ UsersParam = userParams});
          Response.AddPagination(usersToReturn.CurrentPage, usersToReturn.PageSize, usersToReturn.TotalCount, usersToReturn.TotalPages);
          return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name ="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {

          var userToReturn = await _mediator.Send(new UserDetailsQuery { Id = id});
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id , UserDetailsViewModel userForUpdate)
        {
             if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
               return Unauthorized();

             if(await _mediator.Send(new EditUserCommand { User = userForUpdate}) > 0)
               return NoContent();

             throw new Exception($"Updating user{id} failed to save");
        }

    }
}