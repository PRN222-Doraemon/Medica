namespace Core.Ultilities.Seeders
{
    public interface IFileReader
    {
        Task<string> ReadFileAsync(string filePath);
    }
}