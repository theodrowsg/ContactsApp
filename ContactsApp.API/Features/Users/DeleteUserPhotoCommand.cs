using ContactsApp.API.Services;
using MediatR;

namespace ContactsApp.API.Features.Users
{
    public class DeleteUserPhotoCommand : IRequest<ServiceResult>
    {
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        
    }
}