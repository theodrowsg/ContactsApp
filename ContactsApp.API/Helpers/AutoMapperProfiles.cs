using AutoMapper;
using ContactsApp.API.Dtos;
using ContactsApp.API.Models;
using ContactsApp.API.ViewModels;
using System.Linq;

namespace ContactsApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
       public AutoMapperProfiles(){
           CreateMap<User, UserListViewModel>()
           .ForMember(dest => dest.Age, opt => opt.MapFrom( d => d.DateOfBirth.CalculateAge()))
           .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom( p => p.Photos.FirstOrDefault(m => m.IsMain).Url));
           CreateMap<User, UserDetailsViewModel>()
           .ForMember(dest => dest.Age, opt => opt.MapFrom( d => d.DateOfBirth.CalculateAge()))
           .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom( p => p.Photos.FirstOrDefault(m => m.IsMain).Url));
           CreateMap<Photo, PhotoViewModel>();
           CreateMap<ImageUploadOutput, Photo>();
           CreateMap<UserEditViewModel, User>();
           CreateMap<UserCreateInput, User>();
       }
        
    }
}