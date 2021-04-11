using peptak.Session;
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
          

          var id = Request.QueryString["id"];



            Response.Write(id.ToString());






        }
    }
}