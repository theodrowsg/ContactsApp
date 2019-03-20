
using System.Collections.Generic;
using ContactsApp.API.Helpers;
using ContactsApp.API.ViewModels;
using MediatR;

namespace ContactsApp.API.Features.Users
{
    public class UsersQuery : IRequest<PagedList<UserListViewModel>>
    {
        public UserParams UsersParam { get; set; }
    }
}