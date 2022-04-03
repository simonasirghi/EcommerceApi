using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessorWorkerService.Storage
{
    public class AzureBlobStorageKeys
    {
        public string BlobConnectionString { get;set; }
        public string BlobContainerName { get;set; }

    }
}
