using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class UserDetailsQueryHandler : IRequestHandler<UserDetailsQuery, UserDetailsViewModel>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserDetailsQueryHandler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        public async Task<UserDetailsViewModel> Handle(UserDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Users.AsNoTracking()
                                    .Include(t => t.Photos)
                                    .SingleOrDefaultAsync(t => t.Id == request.Id);
            if(result == null)
            return null;
                    
            return _mapper.Map<UserDetailsViewModel>(result);                      
        }
    }
}