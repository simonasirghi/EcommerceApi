namespace EcommerceApi.Storage
{
    public class LocalStorage: GenericStorage
    {
        public override async Task<string> Upload(IFormFile file)
        {
            long size = file.Length;
            string fileName = GenerateRandomFileName(64) + "." + GetFileExtension(file.FileName);
            var filePath = ("/home/TempStorage/" + fileName);

            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (size > 0)
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return fileName;

        }
    }
}
