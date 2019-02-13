using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class UsersQueryHandler : IRequestHandler<UsersQuery, List<UserListViewModel>>
    {
       private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UsersQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<UserListViewModel>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
             var result = await _context.Users.Include(t => t.Photos).ToListAsync();
             return  _mapper.Map<List<UserListViewModel>>(result);
        }
    }
}