using System.Text;

namespace EcommerceApi.Storage
{
    public abstract class GenericStorage : IGenericStorage
    {
        public string GenerateRandomFileName(int length)
        {
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        public string GetFileExtension(string fileName)
        {
            return fileName.Split('.').Last();
        }

        public abstract Task<string> Upload(IFormFile file);
    }
}
