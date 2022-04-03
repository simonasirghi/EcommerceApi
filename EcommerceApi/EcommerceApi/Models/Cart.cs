using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApi.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string BuyerToker { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

    }
}
