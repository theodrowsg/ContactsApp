using ContactsApp.API.ViewModels;
using MediatR;
namespace ContactsApp.API.Features.Users
{
    public class EditUserCommand : IRequest<int>
    {
        public UserDetailsViewModel User {get; set;}
    }
}