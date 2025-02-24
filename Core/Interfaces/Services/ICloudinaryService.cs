using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(IFormFile file);
        Task DeleteImageAsync(string publicId);
    }
}
