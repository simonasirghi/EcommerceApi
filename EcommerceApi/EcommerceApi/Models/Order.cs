namespace EcommerceApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string PaymentIntent { get; set; }
        public string Status { get; set; }
    }
}
