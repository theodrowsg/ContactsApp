using System.Threading.Tasks;
using ContactsApp.API.Dtos;
using Microsoft.AspNetCore.Http;

namespace ContactsApp.API.Providers
{
    public interface IImageStore
    {
         ImageUploadOutput UploadFromStream(IFormFile file);
         string RemoveImage(string imageId);
    }
}