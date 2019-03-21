using ContactsApp.API.Services;
using MediatR;

namespace ContactsApp.API.Features.Users
{
    public class EditUserLikeCommand: IRequest<ServiceResult>
    {
        public int UserId { get; set; }
        public int RecepientId { get; set; }
        
    }
}