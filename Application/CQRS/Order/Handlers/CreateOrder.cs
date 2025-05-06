using Application.CQRS.Order.ResponseDtos;
using Application.CQRS.Stripe.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;
using DAL.SqlServer.Context;
using Application.CQRS.Payment.Commands;

public class CreateOrder
{
    public class CreateOrderCommand : IRequest<Result<CreateOrderDto>>
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
    }

    public sealed class Handler : IRequestHandler<CreateOrderCommand, Result<CreateOrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context; // DbContext doğrudan inject edildi!

        public Handler(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartLines)
                    .ThenInclude(cl => cl.Product)
                    .FirstOrDefaultAsync(c => c.UserId == request.UserId && !c.IsDeleted);

                if (cart == null || !cart.CartLines.Any(cl => !cl.IsDeleted))
                {
                    return new Result<CreateOrderDto>
                    {
                        Data = null,
                        Errors = new List<string> { "The cart is empty or all products have been deleted." },
                        IsSuccess = false
                    };
                }

                var order = _mapper.Map<Order>(request);
                order.OrderDate = DateTime.Now;
                order.Status = "Pending";
                order.OrderLines = cart.CartLines
                    .Where(cl => !cl.IsDeleted)
                    .Select(cl => new OrderLine
                    {
                        ProductId = cl.ProductId,
                        Quantity = cl.Quantity,
                        UnitPrice = cl.UnitPrice
                    }).ToList();

                await _unitOfWork.OrderRepository.CreateOrderAsync(order);
                await _unitOfWork.SaveChangeAsync();

                var lineItems = cart.CartLines
                    .Where(cl => !cl.IsDeleted)
                    .Select(cl => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(cl.UnitPrice * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cl.Product?.Name ?? $"Product ID: {cl.ProductId}"
                            }
                        },
                        Quantity = cl.Quantity
                    }).ToList();

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = lineItems,
                    Mode = "payment",
                    /*SuccessUrl = "https://seninsiten.com/payment-success,"*/
                    SuccessUrl = "https://seninsiten.com/payment-success?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = "https://seninsiten.com/payment-cancel",
                    ClientReferenceId = request.UserId.ToString()
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                if (session == null || string.IsNullOrEmpty(session.Id))
                {
                    return new Result<CreateOrderDto>
                    {
                        Data = null,
                        Errors = new List<string> { "Stripe payment session creation error." },
                        IsSuccess = false
                    };
                }

                // Sepeti ödeme başarılı olduktan sonra temizle
                var paymentSuccessCommand = new PaymentSuccess.PaymentSuccessCommand(session.Id);
                await _unitOfWork.SaveChangeAsync();

                var dto = _mapper.Map<CreateOrderDto>(order);
                dto.SessionId = session.Id;
                dto.CheckoutUrl = session.Url;

                return new Result<CreateOrderDto>
                {
                    Data = dto,
                    Errors = new List<string>(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                return new Result<CreateOrderDto>
                {
                    Data = null,
                    Errors = new List<string> { "An error occurred: " + ex.Message },
                    IsSuccess = false
                };
            }
        }
    }


}