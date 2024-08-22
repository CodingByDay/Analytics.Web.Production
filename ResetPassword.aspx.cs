using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;

namespace Dash
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void reset_Click(object sender, EventArgs e)
        {
            SendActivationRequest();
        }


        /// <summary>
        /// Stored procedure checking if the user exists and fetching the uuid and an emal.
        ///  Stored procedure: spResetPassword
        ///  Parameter/s: @Username
        /// </summary>
        private void SendActivationRequest()
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
            using (conn)
            {
                SqlCommand cmd = new SqlCommand("spResetPassword", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramUsername = new SqlParameter("@UserName", username.Text);

                cmd.Parameters.Add(paramUsername);

                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (Convert.ToBoolean(rdr["ReturnCode"]))
                    {
                        SendPasswordResetEmail(rdr["Email"].ToString(), username.Text, rdr["UniqueId"].ToString());
                        Response.Write($"<script type=\"text/javascript\">alert('Email sa instrukcijama za resetiranje vašega gesla smo poslali na vaš email.'  );</script>");

                    }
                    else
                    {
                        Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake. Uporabniško ime ne obstaja.'  );</script>");

                    }
                }
            }

        }



        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("logon.aspx", true);
        }



        private void SendPasswordResetEmail(string ToEmail, string UserName, string UniqueId)
        {
            try
            {
                string email = ConfigurationManager.AppSettings["email"];
                string username = ConfigurationManager.AppSettings["username"];
                string password = ConfigurationManager.AppSettings["password"];
                // MailMessage class is present is System.Net.Mail namespace
                MailMessage mailMessage = new MailMessage(email, ToEmail);
                // StringBuilder class is present in System.Text namespace
                StringBuilder sbEmailBody = new StringBuilder();
                sbEmailBody.Append("Spoštovani " + UserName + ",<br/><br/>");
                sbEmailBody.Append("Prosimo da na naslednji povezavi resetirate geslo.");
                sbEmailBody.Append("<br/>"); sbEmailBody.Append("https://dash.in-sist.si/ChangePassword.aspx?uid=" + UniqueId);
                sbEmailBody.Append("<br/><br/>");
                sbEmailBody.Append("<b>IN SIST d.o.o.</b>");
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = sbEmailBody.ToString();
                mailMessage.Subject = "Resetiranje gesla IN SIST";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = username,
                    Password = "edtstqntgzlaszcr"
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            } catch (Exception ex)
            {
                var s = ex;
                var stop = true;
            }
        }



    }
}