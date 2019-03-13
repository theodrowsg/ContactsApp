using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Dtos;
using ContactsApp.API.Helpers;
using ContactsApp.API.Models;
using ContactsApp.API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class EditUserRegisterCommandHandler : IRequestHandler<EditUserRegisterCommand, ServiceResult>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EditUserRegisterCommandHandler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }
        public async Task<ServiceResult> Handle(EditUserRegisterCommand request, CancellationToken cancellationToken)
        {
             ServiceResult result = new ServiceResult();
             byte [] passwordHash, passwordSalt;
            
            var createUserInput = request.UserCreateInput;
            SecurityService.CreatePasswordHash(createUserInput.Password, out passwordHash, out passwordSalt);
    
            createUserInput.UserName = createUserInput.UserName.ToLower();

            if (await _context.Users.AnyAsync( x => x.UserName == createUserInput.UserName))
            {
                result.Success = false;
                result.Message = "username already exists";
            }

            var user =  _mapper.Map<User>(createUserInput);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

             await _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();
             result.ObjectId = user.Id;
             result.Success = true;

            return result;
        }
    }
}