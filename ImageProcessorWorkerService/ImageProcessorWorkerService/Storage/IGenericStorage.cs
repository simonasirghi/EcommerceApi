using Microsoft.AspNetCore.Http;

namespace ImageProcessorWorkerService.Storage
{
    public interface IGenericStorage
    {
        string GetFileExtension(string fileName);
        string GenerateRandomFileName(int length);
        Task<string> Upload(FileStream file, string fileName);
    }
}
