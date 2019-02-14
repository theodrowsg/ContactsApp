using ContactsApp.API.ViewModels;
using MediatR;
namespace ContactsApp.API.Features.Users
{
    public class EditUserCommand : IRequest<int>
    {
        public UserEditViewModel User {get; set;}
    }
}