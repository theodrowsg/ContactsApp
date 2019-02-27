using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactsApp.API.Dtos;
using ContactsApp.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ContactsApp.API.Providers
{
    public class ImageStore : IImageStore
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly Cloudinary _cloudinary;
        public ImageStore(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;

             Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(new Account("dtvenrlzm","284291775722925","xPDqBd-ZMxi2AxWcOlRx61yW-Oc"));

        }

        public string  RemoveImage(string publicId)
        {
           var deletionParams = new DeletionParams(publicId);

           var result = _cloudinary.Destroy(deletionParams);

           return result.Result;
        }

        public ImageUploadOutput UploadFromStream(IFormFile file)
        {
             ImageUploadResult uploadResult = null;
             var imgUploadOutput = new ImageUploadOutput();
             using (var stream = file.OpenReadStream())
             {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };

                uploadResult = _cloudinary.Upload(uploadParams);

                if(uploadResult != null){
                    imgUploadOutput.Url = uploadResult.Uri.ToString();
                    imgUploadOutput.PublicId = uploadResult.PublicId;

                }
             }

             return imgUploadOutput;
        }
    }
}