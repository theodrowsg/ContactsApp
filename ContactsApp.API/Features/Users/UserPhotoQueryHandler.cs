using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Models;
using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class UserPhotoQueryHandler : IRequestHandler<UserPhotoQuery, PhotoViewModel>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserPhotoQueryHandler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }
        public async Task<PhotoViewModel> Handle(UserPhotoQuery request, CancellationToken cancellationToken)
        {
                  var photo = await _context.Photos.FirstOrDefaultAsync( p => p.Id == request.PhotoId);

                  return _mapper.Map<PhotoViewModel>(photo);
        }
    }
}