using System.Threading.Tasks;
using ContactsApp.API.Dtos;
using Microsoft.AspNetCore.Http;

namespace ContactsApp.API.Services
{
    public interface IImageService
    {
         ImageUploadOutput UploadUserPhotoImageAsync(int userId, IFormFile image);
         string DeletePhotoImage(string imageUrl);
    }
}