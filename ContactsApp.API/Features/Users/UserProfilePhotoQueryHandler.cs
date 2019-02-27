using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContactsApp.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class UserProfilePhotoQueryHandler : IRequestHandler<UserProfilePhotoQuery, string>
    {
        private readonly DataContext _context;
        public UserProfilePhotoQueryHandler(DataContext context)
        {
            _context = context;

        }
        public async Task<string> Handle(UserProfilePhotoQuery request, CancellationToken cancellationToken)
        {
            var photo = await _context.Photos.Where(u => u.UserId == request.UserId).FirstOrDefaultAsync( p => p.IsMain);

            return photo.Url;
        }
    }
}