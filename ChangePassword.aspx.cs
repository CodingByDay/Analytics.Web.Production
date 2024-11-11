using Sentry;
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
            try
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

        protected void change_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private bool IsPasswordResetLinkValid()
        {
            try
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        private bool ChangeUserPassword()
        {
            try
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        private bool ArePasswordsEqual()
        {
            try
            {
                return string.Equals(pwd.Text, REpwd.Text);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        private bool ExecuteStoredProcedure(string spName, List<SqlParameter> spParameters)
        {
            try
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
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            try
            {
                // Consider using a more secure hashing algorithm like PBKDF2, bcrypt, or Argon2
                return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }
        private void ShowAlert(string message)
        {
            try
            {
                string script = $@"
            <script type='text/javascript'>
                document.addEventListener('DOMContentLoaded', function() {{
                    Swal.fire('{message}');
                }});
            </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", script, false);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}
