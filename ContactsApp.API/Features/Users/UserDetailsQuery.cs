using ContactsApp.API.ViewModels;
using MediatR;
namespace ContactsApp.API.Features.Users
{
    public class UserDetailsQuery: IRequest<UserDetailsViewModel>
    {
        public int Id { get; set; }
    }
}