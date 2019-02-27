using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class EditUserPhotoSetMainCommandHandler : IRequestHandler<EditUserPhotoSetMainCommand, ServiceResult>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EditUserPhotoSetMainCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(EditUserPhotoSetMainCommand request, CancellationToken cancellationToken)
        {
            var users = await _context.Users.Include(p => p.Photos)
                                .SingleOrDefaultAsync(u => u.Id == request.UserId);
             
             ServiceResult serviceResult = new ServiceResult();
            if(!users.Photos.Any(p => p.Id == request.PhotoId))
            {
                serviceResult.Success = false;
                serviceResult.MessageCode = 401;
                return serviceResult;
            }

            var photo = await _context.Photos.Where( p => p.Id == request.PhotoId).FirstOrDefaultAsync();

            if(photo.IsMain){
                serviceResult.Success = false;
                serviceResult.MessageCode = 400;  
                serviceResult.Message = "This photo is already the main photo";
                return serviceResult;
            }

            var mainPhoto = await _context.Photos.Where(u => u.UserId == request.UserId).FirstOrDefaultAsync(p => p.IsMain);
              mainPhoto.IsMain = false;
              photo.IsMain = true;

              await _context.SaveChangesAsync();
              serviceResult.Success = true;

            return serviceResult;

        }
    }
}