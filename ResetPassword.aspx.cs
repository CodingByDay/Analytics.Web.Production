using Sentry;
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
            try
            {
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void reset_Click(object sender, EventArgs e)
        {
            try
            {
                SendActivationRequest();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        /// <summary>
        ///  Stored procedure checking if the user exists and fetching the uuid and an emal.
        ///  Stored procedure: spResetPassword
        ///  Parameter/s: @Username
        /// </summary>
        private void SendActivationRequest()
        {
            try
            {
                var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                conn = new SqlConnection(ConnectionString);
                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("sp_reset_password", conn);
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
                            Response.Write("<script type=\"text/javascript\">window.onload = function() { Swal.fire('Email Sent!', 'We have sent you an email with instructions.', 'success'); };</script>");
                        }
                        else
                        {
                            Response.Write("<script type=\"text/javascript\">window.onload = function() { Swal.fire('Error', 'There was an error in reseting the password', 'error'); };</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Logon.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void SendPasswordResetEmail(string ToEmail, string UserName, string UniqueId)
        {
            try
            {
                string email = ConfigurationManager.AppSettings["email"];
                string username = ConfigurationManager.AppSettings["username"];
                string password = ConfigurationManager.AppSettings["password"];

                // MailMessage class is present in System.Net.Mail namespace
                MailMessage mailMessage = new MailMessage(email, ToEmail);

                // StringBuilder class is present in System.Text namespace
                StringBuilder sbEmailBody = new StringBuilder();
                sbEmailBody.Append("Dear " + UserName + ",<br/><br/>");
                sbEmailBody.Append("Please reset you password at the following link.");
                sbEmailBody.Append("<br/>");

                // Get the base URL dynamically
                string baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/";
                sbEmailBody.Append($"<a href='{baseUrl}ChangePassword.aspx?uid={UniqueId}'>Reset password</a><br/><br/>");

                sbEmailBody.Append("<br/><br/>");
                sbEmailBody.Append("<b>In.Sist d.o.o.</b>");

                mailMessage.IsBodyHtml = true;
                mailMessage.Body = sbEmailBody.ToString();
                mailMessage.Subject = "Dash system password reset";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = username,
                    Password = password
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}