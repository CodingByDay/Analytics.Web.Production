using peptak.Session;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class Success : System.Web.UI.Page
    {
        private object MyObject;

        protected void Page_Load(object sender, EventArgs e)
        {


            // Stripe configuration key. Safe to use like this...
            StripeConfiguration.ApiKey = "sk_test_51IdsgFI2g0bX7R9aOa1atoifUOkT6csUhopE53KQYUA55UZmKVPdWP3BLcZ9UTqF6zVIn0OIpeAXlL5CiiLHFWv900tcoe7Bzt";
            // Getting the query string.
            var id = Request.QueryString["id"];


            // testing the query string.
            Response.Write(id.ToString());



            /// Page that will show all the data for the session and also write info to the database.
            /// 
            var service = new SessionService();



            var session = service.Get(id);



            Response.Write(session.ToJson());
            //









        }
    }
}