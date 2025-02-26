using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Helpers;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _folderName;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
            _folderName = cloudinarySettings.FolderName;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file was provided");

            using (var stream = file.OpenReadStream())
            {
                string extension = Path.GetExtension(file.FileName).ToLower();
                string resourceType = (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                ? "image"
                : "raw";
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = _folderName,
                    Overwrite = true,
                    PublicId = file.FileName,
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return;

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }
        }

        private string GetPublicIdFromUrl(string url)
        {
            // Extract public ID from Cloudinary URL
            var uri = new Uri(url);
            var segments = uri.Segments;
            var fileName = segments[segments.Length - 1];
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}
