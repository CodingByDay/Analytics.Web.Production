using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class Administration : System.Web.UI.Page
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private List<String> DataUser = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            Button btn = ((Button)Master.FindControl("admin"));
            btn.Text = "Nazaj";
            
            btn.Click += Btn_Click;
            string UserNameForCheckingAdmin = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=petpakDash;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand("Select uname from Users", conn); /// Intepolation or the F string. C# > 5.0       
            // Execute command and fetch pwd field into lookupPassword string.
            SqlDataReader sdr = cmd.ExecuteReader();
            
           
                while (sdr.Read())
                {
                DataUser.Add(sdr["uname"].ToString());

                }



            users.DataSource = DataUser;
            users.DataBind();

        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }
    }
}
