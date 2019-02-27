using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ContactsApp.API.Features.Users
{
    public class EditUserPhotoUploadCommand : IRequest<PhotoViewModel>
    {
        public int UserId { get; set; }
        public IFormFile File {get; set;}

    }
}