using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Features.Users;
using ContactsApp.API.Models;
using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Controllers
{
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
        public async Task<IActionResult> GetUsers()
        {
         
          var usersToReturn = await _mediator.Send(new UsersQuery());
          return Ok(usersToReturn);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {

          var userToReturn = await _mediator.Send(new UserDetailsQuery { Id = id});
            return Ok(userToReturn);
        }

    }
}