using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Data;
using ContactsApp.API.Helpers;
using ContactsApp.API.Models;
using ContactsApp.API.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Features.Users
{
    public class UsersQueryHandler : IRequestHandler<UsersQuery, PagedList<UserListViewModel>>
    {
       private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UsersQueryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PagedList<UserListViewModel>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
             var users =  _context.Users.Where(u => u.Id != request.UsersParam.UserId).OrderByDescending(u => u.LastActive).Include(t => t.Photos).AsQueryable();   
             var gender = request.UsersParam.Gender;
             if(string.IsNullOrEmpty(gender)){
                 var user = await _context.Users.Where(u => u.Id == request.UsersParam.UserId).SingleOrDefaultAsync();
                 gender = user.Gender == "female" ? "male":"female";
             }
             if(request.UsersParam.MinAge !=18 || request.UsersParam.MaxAge != 99){
               var minDob = DateTime.Today.AddYears(-request.UsersParam.MaxAge);
               var maxDob = DateTime.Today.AddYears(-request.UsersParam.MinAge);
               users = _context.Users.Where( u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
             }

             users = users.Where( u => u.Gender == gender);

             if(!string.IsNullOrEmpty(request.UsersParam.OrderBy)){
                 switch(request.UsersParam.OrderBy){
                     case "created":
                       users = users.OrderByDescending(u =>u.Created);
                       break;
                      default:
                       users = users.OrderByDescending(u => u.LastActive);
                       break;
                 }
             }
             var pagedList = await PagedList<User>.CreateAsync(users, request.UsersParam.PageNumber, request.UsersParam.PageSize);
             var pagedVM =   _mapper.Map<PagedList<UserListViewModel>>(pagedList);
             pagedVM.PageSize = pagedList.PageSize;
             pagedVM.CurrentPage = pagedList.CurrentPage;
             pagedVM.TotalCount = pagedList.TotalCount;
             pagedVM.TotalPages = pagedList.TotalPages;

             return pagedVM;
        }
    }
}