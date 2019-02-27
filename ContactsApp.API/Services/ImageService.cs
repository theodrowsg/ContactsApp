using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using ContactsApp.API.Dtos;
using ContactsApp.API.Models;
using ContactsApp.API.Providers;
using Microsoft.AspNetCore.Http;

namespace ContactsApp.API.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageStore _imageStore;
        private readonly IMapper _mapper;

        public ImageService(IImageStore imageStore, IMapper mapper)
        {
            _imageStore = imageStore;
            _mapper = mapper;

        }

        public ImageUploadOutput UploadUserPhotoImageAsync(int userId, IFormFile image)
        {
            var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.ToString().Trim('"').ToLower();

            if (fileName.EndsWith(".jpeg") || fileName.EndsWith(".jpg") || fileName.EndsWith(".png") || fileName.EndsWith(".gif"))
            {          
               return _imageStore.UploadFromStream(image);
            }

           throw new Exception("Invalid file extension: " + fileName + "You can only upload images with the extension: jpg, jpeg, gif, or png");
        }

        public string DeletePhotoImage(string publicId){
              
              return _imageStore.RemoveImage(publicId);
        }
    }
}