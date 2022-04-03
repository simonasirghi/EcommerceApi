using Azure.Storage.Blobs;

namespace EcommerceApi.Storage
{
    public class CloudStorage: GenericStorage
    {
        private readonly IConfiguration _configuration;
        public CloudStorage(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public override async Task<string> Upload(IFormFile file)
        {
            string blobstorageconnection = _configuration.GetValue<string>("BlobConnectionString");
            string blobcontainer = _configuration.GetValue<string>("BlobContainerName");
            string systemFileName = GenerateRandomFileName(64) + "." + GetFileExtension(file.FileName);

            var container = new BlobContainerClient(blobstorageconnection, blobcontainer);
            var blob = container.GetBlobClient(systemFileName);
            await blob.UploadAsync(file.OpenReadStream());

            return systemFileName;
        }

    }
}
