
using System.Collections.Generic;
using ContactsApp.API.ViewModels;
using MediatR;

namespace ContactsApp.API.Features.Users
{
    public class UsersQuery : IRequest<List<UserListViewModel>>
    {
        
    }
}