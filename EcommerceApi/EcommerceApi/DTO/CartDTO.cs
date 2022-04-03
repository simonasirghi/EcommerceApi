using EcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApi.DTO
{
    public class CartDTO
    {
        public string BuyerToker { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
