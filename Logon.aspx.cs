using Sentry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace Dash
{
    public partial class Logon : System.Web.UI.Page
    {
        private SqlCommand cmd;
        private string role;
        private List<string> strings = new List<string>();
        private string connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Pumpkin");
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("Expires", "0");

                if (!IsPostBack)
                {
                    FetchDataFillList();
                }

                // Check if the authentication cookie exists
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    // Decrypt the authentication ticket
                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket tkt = FormsAuthentication.Decrypt(authCookie.Value);
                    string userName = tkt.Name;

                    // Get the role of the user
                    string role = GetRole(userName); // Adjusted to not require password

                    // Redirect based on the user's role
                    if (role == "SuperAdmin")
                    {
                        Response.Redirect("Index.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("IndexTenant.aspx", false);
                    }

                    // Complete the request to prevent further processing
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private string GetRole(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        cmd = new SqlCommand("SELECT user_role FROM users WHERE uname=@userName", conn);
                        cmd.Parameters.AddWithValue("@userName", username);
                        return cmd.ExecuteScalar()?.ToString(); // Returns the role or null
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return string.Empty;
            }
        }

        private void FetchDataFillList()
        {
            try
            {
                ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
                if (connections.Count != 0)
                {
                    foreach (ConnectionStringSettings connection in connections)
                    {
                        strings.Add(connection.Name);
                    }
                    strings.RemoveAt(0);
                    databaseList.DataSource = strings;
                    databaseList.DataBind();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private bool ValidateUser(string userName, string passWord)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        string lookupPassword = null;

                        // Input validation
                        if ((string.IsNullOrEmpty(userName)) || (userName.Length > 15))
                        {
                            System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of userName failed.");
                            return false;
                        }

                        if ((string.IsNullOrEmpty(passWord)) || (passWord.Length > 25))
                        {
                            System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of passWord failed.");
                            return false;
                        }

                        // Get password from DB
                        cmd = new SqlCommand("SELECT password FROM users WHERE uname=@userName", conn);
                        cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25).Value = userName;
                        lookupPassword = (string)cmd.ExecuteScalar();

                        // If no password found, return false
                        if (lookupPassword == null)
                        {
                            return false;
                        }

                        string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(passWord, "SHA1");
                        return (lookupPassword.Equals(HashedPassword, StringComparison.Ordinal));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception: " + ex.Message);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            try
            {
                PerformUserValidation();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void PerformUserValidation()
        {
            try
            {
                if (ValidateUser(txtUserName.Value, txtUserPass.Value))
                {
                    FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, txtUserName.Value, DateTime.Now,
                    DateTime.Now.AddYears(42), true, "Analytics");

                    string cookiestr = FormsAuthentication.Encrypt(tkt);
                    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr)
                    {
                        Path = FormsAuthentication.FormsCookiePath
                    };

                    if (chkPersistCookie.Checked)
                        ck.Expires = tkt.Expiration;

                    Response.Cookies.Add(ck);
                    string strRedirect = GetRole(txtUserName.Value);
                    Response.Redirect(strRedirect == "SuperAdmin" ? "Index.aspx" : "IndexTenant.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Response.Redirect("Logon.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}