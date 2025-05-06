using Application.CQRS.Payment.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper; // AutoMapper namespace ekleniyor

namespace CarHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper; // AutoMapper'ı ekledik

        public PaymentController(ISender sender, IConfiguration configuration, IMapper mapper)
        {
            _sender = sender;
            _configuration = configuration;
            _mapper = mapper; // AutoMapper'ı inject ettik
        }

        [HttpPost("payment-success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return BadRequest(new { Errors = new[] { "SessionId is required." } });
            }

            try
            {
                Console.WriteLine($"📦 Gelen SessionId: {sessionId}");

                // PaymentSuccessCommand oluşturuluyor
                var paymentSuccessCommand = new PaymentSuccess.PaymentSuccessCommand(sessionId);

                // Komut gönderiliyor
                await _sender.Send(paymentSuccessCommand);

                return Ok(new { Message = "Payment processed successfully." });
            }
            catch (Exception ex)
            {
                // Hata loglaması yapılır
                Console.WriteLine($"🔥 PaymentSuccess error: {ex.Message}");
                return StatusCode(500, new { Errors = new[] { $"Payment success processing failed: {ex.Message}" } });
            }
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            // Request Body'nin tekrar okunabilmesi için buffering aktif et
            HttpContext.Request.EnableBuffering();

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // Stream'i başa sarıyoruz
            HttpContext.Request.Body.Position = 0;

            Console.WriteLine("🚀 Webhook endpoint triggered.");

            try
            {
                var stripeSecret = _configuration["Stripe:WebhookSecret"];
                var stripeSignatureHeader = Request.Headers["Stripe-Signature"];

                if (string.IsNullOrEmpty(stripeSignatureHeader))
                {
                    Console.WriteLine("⚠️ Stripe-Signature header eksik.");
                    return BadRequest();
                }

                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignatureHeader, stripeSecret);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    if (session != null)
                    {
                        Console.WriteLine($"✅ Webhook received, Session ID: {session.Id}");

                        var userId = session.ClientReferenceId;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            Console.WriteLine($"👤 UserId found: {userId}");

                            var paymentSuccessCommand = _mapper.Map<PaymentSuccess.PaymentSuccessCommand>(session);

                            await _sender.Send(paymentSuccessCommand);

                            Console.WriteLine($"🎉 Cart cleared and order status updated for UserId {userId}");
                        }
                        else
                        {
                            Console.WriteLine("⚠️ ClientReferenceId (UserId) is missing.");
                        }
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"🔥 Stripe webhook error: {e.Message}");
                return BadRequest();
            }
        }
    }
}