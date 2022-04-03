using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ImageProcessorWorkerService.Storage
{
    public class CloudStorage: GenericStorage
    {
        private readonly IOptions<AzureBlobStorageKeys> _options;
        public CloudStorage(IOptions<AzureBlobStorageKeys> options)
        {
            _options = options;
        }
        public override async Task<string> Upload(FileStream file, string fileName)
        {
            string blobstorageconnection = _options.Value.BlobConnectionString;
            string blobcontainer = _options.Value.BlobContainerName;

            var container = new BlobContainerClient(blobstorageconnection, blobcontainer);
            var blob = container.GetBlobClient(fileName);
            await blob.UploadAsync(file);

            return fileName;
        }

    }
}
