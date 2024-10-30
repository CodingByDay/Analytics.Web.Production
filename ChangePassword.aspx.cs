using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace Dash
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPasswordResetLinkValid())
                {
                    ShowAlert("Čas za resetiranje gesla je potekal ali link ni več vredo.");
                    Response.Redirect("Logon.aspx", false);
                }
            }
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Logon.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void change_Click(object sender, EventArgs e)
        {
            if (ArePasswordsEqual())
            {
                if (ChangeUserPassword())
                {
                    ShowAlert("Geslo uspešno spremenjeno.");
                    Response.Redirect("Logon.aspx", false); // Redirect after successful password change
                }
                else
                {
                    ShowAlert("Link za spremembo gesla je potekal ali ni vejaven.");
                }
            }
            else
            {
                ShowAlert("Gesla nista enaka.");
            }
        }

        private bool IsPasswordResetLinkValid()
        {
            var uid = Request.QueryString["uid"];
            if (string.IsNullOrEmpty(uid))
            {
                return false; // Handle missing UID
            }

            var paramList = new List<SqlParameter>
            {
                new SqlParameter("@GUID", uid)
            };

            return ExecuteStoredProcedure("sp_is_password_reset_link_valid", paramList);
        }

        private bool ChangeUserPassword()
        {
            var uid = Request.QueryString["uid"];
            if (string.IsNullOrEmpty(uid))
            {
                return false; // Handle missing UID
            }

            var paramList = new List<SqlParameter>
            {
                new SqlParameter("@GUID", uid),
                new SqlParameter("@Password", HashPassword(pwd.Text))
            };

            return ExecuteStoredProcedure("sp_change_password", paramList);
        }

        private bool ArePasswordsEqual()
        {
            return string.Equals(pwd.Text, REpwd.Text);
        }

        private bool ExecuteStoredProcedure(string spName, List<SqlParameter> spParameters)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddRange(spParameters.ToArray());

                    conn.Open();
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
        }

        private string HashPassword(string password)
        {
            // Consider using a more secure hashing algorithm like PBKDF2, bcrypt, or Argon2
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
        }
        private void ShowAlert(string message)
        {
            string script = $@"
            <script type='text/javascript'>
                document.addEventListener('DOMContentLoaded', function() {{
                    Swal.fire('{message}');
                }});
            </script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", script, false);
        }
    }
}
