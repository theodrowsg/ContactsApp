using ContactsApp.API.ViewModels;
using MediatR;

namespace ContactsApp.API.Features.Users
{
    public class UserPhotoQuery : IRequest<PhotoViewModel>
    {
        public int PhotoId { get; set; }
    }
}