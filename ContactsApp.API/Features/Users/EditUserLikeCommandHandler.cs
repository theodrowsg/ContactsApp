using System.Threading;
using System.Threading.Tasks;
using ContactsApp.API.Data;
using ContactsApp.API.Models;
using ContactsApp.API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class EditUserLikeCommandHandler : IRequestHandler<EditUserLikeCommand, ServiceResult>
    {
        private readonly DataContext _context;
        public EditUserLikeCommandHandler(DataContext context)
        {
            _context = context;

        }
        public async Task<ServiceResult> Handle(EditUserLikeCommand request, CancellationToken cancellationToken)
        {
               var like = await _context.Likes.FirstOrDefaultAsync(u => u.LikeeId == request.RecepientId && u.LikerId == request.UserId);
               ServiceResult _result = new ServiceResult();
               if(like != null){
                  _result.MessageCode = 400;
                  _result.Message = "You already liked this member";
                  return _result;
               }

               if(!await _context.Users.AnyAsync(u => u.Id == request.RecepientId)){
                    _result.MessageCode = 404; 
                    return _result;
               }

               like = new Like {
                   LikerId = request.UserId,
                   LikeeId = request.RecepientId
               };

               _context.Likes.Add(like);
               await _context.SaveChangesAsync();
               _result.Success = true;

            return _result;
        }
    }
}