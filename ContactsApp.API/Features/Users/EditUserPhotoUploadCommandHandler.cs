using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Dtos;
using ContactsApp.API.Models;
using ContactsApp.API.Services;
using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class EditUserPhotoUploadCommandHandler : IRequestHandler<EditUserPhotoUploadCommand, PhotoViewModel>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public EditUserPhotoUploadCommandHandler(DataContext context, IMapper mapper, IImageService imageService)
        {
            _imageService = imageService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<PhotoViewModel> Handle(EditUserPhotoUploadCommand request, CancellationToken cancellationToken)
        {
           ImageUploadOutput uploadOutput = _imageService.UploadUserPhotoImageAsync(request.UserId, request.File);
           
            var photo = _mapper.Map<Photo>(uploadOutput);

            User user = await _context.Users
                            .Include(t => t.Photos)
                            .SingleOrDefaultAsync(u => u.Id == request.UserId);
                            
            if (!user.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            await _context.SaveChangesAsync();

            return _mapper.Map<PhotoViewModel>(photo);
        }
    }
}