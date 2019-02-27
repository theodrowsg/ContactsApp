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
    public class DeleteUserPhotoCommandHandler : IRequestHandler<DeleteUserPhotoCommand, ServiceResult>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public DeleteUserPhotoCommandHandler(DataContext context, IMapper mapper, IImageService imageService)
        {
            _mapper = mapper;
            _context = context;
           _imageService = imageService;

    }
    public async Task<ServiceResult> Handle(DeleteUserPhotoCommand request, CancellationToken cancellationToken)
    {
       
        var user = await _context.Users.AsNoTracking()
                                   .Include(p => p.Photos)
                                   .SingleOrDefaultAsync(u => u.Id == request.UserId);

        ServiceResult serviceResult = new ServiceResult();

        if (!user.Photos.Any(p => p.Id == request.PhotoId))
        {
            serviceResult.Success = false;
            serviceResult.MessageCode = 401;
            return serviceResult;
        }

        var photo = await _context.Photos.Where(p => p.Id == request.PhotoId).FirstOrDefaultAsync();

        if (photo.IsMain)
        {
            serviceResult.Success = false;
            serviceResult.MessageCode = 400;
            serviceResult.Message = "Can not delete main photo";
            return serviceResult;
        }

        var result = _imageService.DeletePhotoImage(photo.PublicId);

        if(result == "ok"){
             _context.Photos.Remove(photo);
             await _context.SaveChangesAsync();
             serviceResult.Success = true;

        }

        return serviceResult;

    }
  } 
}