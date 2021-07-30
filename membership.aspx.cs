using peptak.Session;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace peptak
{
    public partial class membership : System.Web.UI.Page
    {


        public string sessionId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Create a checkout session and set sessionId 

            StripeConfiguration.ApiKey = "sk_test_51IdsgFI2g0bX7R9aOa1atoifUOkT6csUhopE53KQYUA55UZmKVPdWP3BLcZ9UTqF6zVIn0OIpeAXlL5CiiLHFWv900tcoe7Bzt";

            var options = new SessionCreateOptions
            {
                SuccessUrl = "https://localhost:44339/Success?id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://example.com/cancel",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                  {
             new SessionLineItemOptions
             {
             Quantity=1,
             Description = "Graphs application",
             Price = "price_1Ieh7bI2g0bX7R9aBCNuuHEi",

            },
             },
                Mode = "subscription",
            };
            var service = new SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            sessionId = session.Id;

            // Service GET(id);




           
        }
    }
}



