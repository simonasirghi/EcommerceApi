using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApi.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
