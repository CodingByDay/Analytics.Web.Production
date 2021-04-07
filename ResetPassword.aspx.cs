using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void reset_Click(object sender, EventArgs e)
        {
            CheckIfUserExists();
            SendEmail();
            UpdateVisualDesign();
        }

        private void UpdateVisualDesign()
        {
            throw new NotImplementedException();
        }

        private void SendEmail()
        {
            throw new NotImplementedException();
        }

        private void CheckIfUserExists()
        {
            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            string sendPassword = String.Format($"select uname, Pwd FROM Users where email='{email.Text}'");
            SqlCommand createUser = new SqlCommand(finalQueryRegistration, conn);
            var username = TxtUserName.Text;
            try
            {
                createUser.ExecuteNonQuery();
            }

        }



        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }



        private void SendPasswordResetEmail(string ToEmail, string UserName, string UniqueId)
        {
            // MailMessage class is present is System.Net.Mail namespace
            MailMessage mailMessage = new MailMessage("YourEmail@gmail.com", ToEmail);


            // StringBuilder class is present in System.Text namespace
            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("Dear " + UserName + ",<br/><br/>");
            sbEmailBody.Append("Prosim sledite link da resetirate geslo.");
            sbEmailBody.Append("<br/>"); sbEmailBody.Append("http://localhost/ChangePassword.aspx?uid=" + UniqueId);
            sbEmailBody.Append("<br/><br/>");
            sbEmailBody.Append("<b>IN SIST doo</b>");

            mailMessage.IsBodyHtml = true;

            mailMessage.Body = sbEmailBody.ToString();
            mailMessage.Subject = "Resetirajte geslo.";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "YourEmail@gmail.com",
                Password = "YourPassword"
            };

            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }



    }
}