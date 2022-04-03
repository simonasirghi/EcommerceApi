// This example sets up an endpoint using the ASP.NET MVC framework.
// Watch this video to get started: https://youtu.be/2-mMOB8MhmE.

using System.Collections.Generic;
using EcommerceApi.Data;
using EcommerceApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentsController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe").GetSection("SecretKey").Value;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateCheckoutSession([FromBody] BuyerDTO buyer)
        {
            var cart = await _context.Carts.Where(b => b.BuyerToker == buyer.BuyerToker)
                .Include(b => b.Product).ToListAsync();

            List<SessionLineItemOptions> PurchasedItems = new List<SessionLineItemOptions>();
            // VERIFICA DACA AI STOC SUFICIENT
            foreach (var item in cart)
            {
                var aux = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = item.Product.Price,
                        Currency = "RON",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },

                    },
                    Quantity = item.Quantity,
                };

                PurchasedItems.Add(aux);
            }

            var options = new SessionCreateOptions
            {
                LineItems = PurchasedItems,
                Mode = "payment",
                SuccessUrl = buyer.SuccessUrl,
                CancelUrl = buyer.CancelUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);

            // Create order
            var order = new Models.Order();
            order.PaymentIntent = session.PaymentIntentId;
            order.Status = "requires_payment_method";

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok(session.Url);
        }

        [HttpPost]
        [Route("/stripe/webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string endpointSecret = _configuration.GetSection("Stripe").GetSection("WebHookSecret").Value;
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("A successful payment for {0} was made.", paymentIntent.Amount);
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);

                    var order = await _context.Orders.Where(p => p.PaymentIntent == paymentIntent.Id).FirstAsync();
                    order.Status = paymentIntent.Status;


                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    
    }
}