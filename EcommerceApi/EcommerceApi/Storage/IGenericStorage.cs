namespace EcommerceApi.Storage
{
    public interface IGenericStorage
    {
        string GetFileExtension(string fileName);
        string GenerateRandomFileName(int length);
        Task<string> Upload(IFormFile file);
    }
}
