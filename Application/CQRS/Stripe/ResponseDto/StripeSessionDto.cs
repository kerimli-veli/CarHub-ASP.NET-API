using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Stripe.ResponseDto
{
    public class StripeSessionDto
    {
        public string SessionId { get; set; }
        public string CheckoutUrl { get; set; }
    }
}
