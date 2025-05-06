using MediatR;
using DAL.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Application.CQRS.Payment.Commands
{
    public class PaymentSuccess
    {
        public class PaymentSuccessCommand : IRequest<Unit>
        {
            public string SessionId { get; set; }

            public PaymentSuccessCommand(string sessionId)
            {
                SessionId = sessionId;
            }
        }

        public sealed class PaymentSuccessCommandHandler : IRequestHandler<PaymentSuccessCommand, Unit>
        {
            private readonly AppDbContext _context;
            private readonly IConfiguration _configuration;

            public PaymentSuccessCommandHandler(AppDbContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public async Task<Unit> Handle(PaymentSuccessCommand request, CancellationToken cancellationToken)
            {
                var stripeSecretKey = _configuration["Stripe:SecretKey"];
                StripeConfiguration.ApiKey = stripeSecretKey;

                var sessionService = new SessionService();

                using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    var session = await sessionService.GetAsync(request.SessionId);

                    Console.WriteLine($"✅ Stripe Session ID: {session.Id}, Status: {session.PaymentStatus}");

                    if (session.PaymentStatus == "paid")
                    {
                        if (!int.TryParse(session.ClientReferenceId, out var userId))
                        {
                            throw new ApplicationException("Invalid ClientReferenceId: Cannot parse userId.");
                        }

                        // Siparişi güncelle
                        var order = await _context.Orders
                            .Where(o => o.UserId == userId && o.Status == "Pending")
                            .OrderByDescending(o => o.OrderDate)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (order == null)
                        {
                            throw new ApplicationException($"Order not found for user {userId}.");
                        }

                        order.Status = "Paid";
                        // Eğer PaidDate varsa buraya ekleyebilirsiniz.
                        // order.PaidDate = DateTime.UtcNow;

                        // Sadece CartLines'ı temizle
                        var cart = await _context.Carts
                            .Include(c => c.CartLines)
                            .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted, cancellationToken);

                        if (cart != null)
                        {
                            // CartLines'ı temizle
                            _context.CartLines.RemoveRange(cart.CartLines);
                        }

                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);

                        Console.WriteLine($"🎉 Payment Success: Order #{order.Id} marked as Paid, CartLines cleared.");
                    }
                    else
                    {
                        throw new ApplicationException("Payment status is not 'paid'.");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    Console.WriteLine($"🔥 Error during payment success processing: {ex.Message}");
                    throw new ApplicationException("Payment processing failed.", ex);
                }

                return Unit.Value;
            }
        }



    }
}