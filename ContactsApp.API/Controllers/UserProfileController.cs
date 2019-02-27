using System.Security.Claims;
using System.Threading.Tasks;
using CloudinaryDotNet;
using ContactsApp.API.Dtos;
using ContactsApp.API.Features.Users;
using ContactsApp.API.Helpers;
using ContactsApp.API.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ContactsApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IImageService _imageService;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;

        public UserProfileController(IMediator mediator, IImageService imageService)
        {
            _mediator = mediator;
            _imageService = imageService;
        }


        [HttpGet("{Id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoVM = await _mediator.Send(new UserPhotoQuery { PhotoId = id });
            return Ok(photoVM);
        }


        [HttpPost]
        public async Task<IActionResult> UploadPhoto(int userId, [FromForm]IFormFile file)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file to upload");
            }

             var photoVM = await _mediator.Send(new EditUserPhotoUploadCommand { UserId = userId, File = file });

            if (photoVM.Id > 0)
            {
                return CreatedAtAction("GetPhoto", new { id = photoVM.Id }, photoVM);
            }

            return BadRequest("Could not add photo");

        }
        [HttpPost("{photoId}/SetMain")]
        public async Task<IActionResult> Edit(int userId, int photoId){

             if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                 return Unauthorized();
             }

             var commandResult = await _mediator.Send(new EditUserPhotoSetMainCommand { UserId = userId, PhotoId = photoId});

             if(!commandResult.Success){
                 switch(commandResult.MessageCode){
                     case 401:
                       return Unauthorized();
                      case 400:
                         return BadRequest(commandResult.Message);
                       default:
                           return BadRequest("Coudn't set photo to main");
                 }
             }

             return NoContent();


        }
       [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int photoId){
                if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                    return Unauthorized();
                }

                var commandResult = await _mediator.Send(new DeleteUserPhotoCommand { UserId = userId, PhotoId = photoId});

                if(!commandResult.Success){
                 switch(commandResult.MessageCode){
                     case 401:
                       return Unauthorized();
                      case 400:
                         return BadRequest(commandResult.Message);
                       default:
                           return BadRequest("Can't remove  photo at this time , try again");
                 }
             }

             return Ok();
          
        }



    }
}