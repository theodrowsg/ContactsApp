using MediatR;

namespace ContactsApp.API.Features.Users
{
    public class UserProfilePhotoQuery : IRequest<string>
    {
        public int UserId { get; set; }
    }
}