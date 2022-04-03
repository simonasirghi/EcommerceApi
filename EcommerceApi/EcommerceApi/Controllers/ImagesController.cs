using EcommerceApi.DTO;
using EcommerceApi.Queue;
using EcommerceApi.Storage;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Models;
using EcommerceApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ImagesController : Controller
    {
        private readonly AppDbContext _context;

        public ImagesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("upload")]

        public async Task<IActionResult> Upload(IFormFile file, int productId)
        {
            LocalStorage localStorage = new LocalStorage();
            
            var rabbitmq = new RabbitMQHandler("image-processor");
            string fileName = await localStorage.Upload(file);
            string filePath = "/home/TempStorage/" + fileName;
            rabbitmq.Publish(filePath);

            // Save image to db
            Image imageDb = new Image();
            imageDb.Name = fileName;
            imageDb.ProductId = productId;

            await _context.Images.AddAsync(imageDb);
            await _context.SaveChangesAsync();

            return Ok("https://ecommerceapi.blob.core.windows.net/imagecontainer/" + fileName);
        }
    }
}
