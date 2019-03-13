using ContactsApp.API.Dtos;
using ContactsApp.API.Services;
using MediatR;

namespace ContactsApp.API.Features.Users
{
    public class EditUserRegisterCommand : IRequest<ServiceResult>
    {
        public UserCreateInput UserCreateInput { get; set; }
    }
}