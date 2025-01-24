namespace Infrastructure.Services.Ultilities.Seeders
{
    public interface IFileReader
    {
        Task<string> ReadFileAsync(string filePath);
    }
}