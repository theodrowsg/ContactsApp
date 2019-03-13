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
            string photoUrl = string.Empty;
            var photo = await _context.Photos.Where(u => u.UserId == request.UserId).FirstOrDefaultAsync( p => p.IsMain);
            if(photo != null)
             photoUrl = photo.Url;
            return photoUrl;
        }
    }
}