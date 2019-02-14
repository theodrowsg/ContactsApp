using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsApp.API.Features.Users
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, int>
    {
        private readonly DataContext _context;
        public IMapper _mapper;

        public EditUserCommandHandler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(EditUserCommand message, CancellationToken cancellationToken){

            var user = await _context.Users.SingleOrDefaultAsync( t => t.Id == message.User.Id) ?? _context.Add(new User()).Entity;

            _mapper.Map(message.User, user);

            await _context.SaveChangesAsync();

            return user.Id;

        }
    }
}