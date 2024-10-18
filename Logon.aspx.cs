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
        private SqlConnection conn;
        private SqlCommand cmd;
        private string role;
        private List<String> strings = new List<string>();
        private bool passport = false;
        private string connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Pumpkin");
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("Expires", "0");
            if (!IsPostBack)
            {
                FetchDataFillList();
            }
        }

        private string GetRole(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    var hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    cmd = new SqlCommand($"SELECT user_role FROM users WHERE uname='{username}' AND password='{hashed}';", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        role = (reader["user_role"].ToString());
                    }
                    return role;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        private void FetchDataFillList()
        {
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            if (connections.Count != 0)
            {
                // go trough all available ConnectionStrings in app.config
                foreach (ConnectionStringSettings connection in connections)
                {
                    // reading the ConnectionString
                    strings.Add(connection.Name);
                }
                strings.RemoveAt(0);
                databaseList.DataSource = strings;
                databaseList.DataBind();
            }
        }

     
        private bool ValidateUser(string userName, string passWord)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd;
                    string lookupPassword = null;
                    // Check for invalid userName.
                    // userName must not be null and must be between 1 and 15 characters.
                    if ((null == userName) || (0 == userName.Length) || (userName.Length > 15))
                    {
                        System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of userName failed.");
                        return false;
                    }

                    // Check for invalid passWord.
                    // passWord must not be null and must be between 1 and 25 characters.
                    if ((null == passWord) || (0 == passWord.Length) || (passWord.Length > 25))
                    {
                        System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of passWord failed.");
                        return false;
                    }
                    try
                    {
                        // Consult with your SQL Server administrator for an appropriate connection
                        // string to use to connect to your local SQL Server.
                        // Create SqlCommand to select pwd field from users table given supplied userName.
                        cmd = new SqlCommand("SELECT password FROM users WHERE uname=@userName", conn);
                        cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25);
                        cmd.Parameters["@userName"].Value = userName;
                        // Execute command and fetch pwd field into lookupPassword string.
                        lookupPassword = (string)cmd.ExecuteScalar();
                        // Cleanup command and connection objects.
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // Add error handling here for debugging.
                        // This error message should not be sent back to the caller.
                        System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
                        return false;
                    }

                    // If no password found, return false.
                    if (null == lookupPassword)
                    {
                        // You could write failed login attempts here to event log for additional security.
                        return false;
                    }
                    string HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(passWord, "SHA1");
                    // Compare lookupPassword and input passWord, using a case-sensitive comparison.
                    return (0 == string.Compare(lookupPassword, HashedPassword, false));
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            ValidateUser();
        }

        private void ValidateUser()
        {
            if (ValidateUser(txtUserName.Value, txtUserPass.Value))
            {
                FormsAuthenticationTicket tkt;
                string cookiestr;
                HttpCookie ck;
                tkt = new FormsAuthenticationTicket(1, txtUserName.Value, DateTime.Now,
                DateTime.Now.AddYears(42), true, "Analytics");
                cookiestr = FormsAuthentication.Encrypt(tkt);
                ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                if (chkPersistCookie.Checked)
                    ck.Expires = tkt.Expiration;
                ck.Path = FormsAuthentication.FormsCookiePath;
                Response.Cookies.Add(ck);
                string strRedirect;
                role = GetRole(txtUserName.Value, txtUserPass.Value);
                if (role == "SuperAdmin")
                {
                    strRedirect = "Index.aspx";
                    Response.Redirect(strRedirect, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    strRedirect = "IndexTenant.aspx";
                    Response.Redirect(strRedirect, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                conn.Close();
                conn.Dispose();
            }
            else
            {
                Response.Redirect("Logon.aspx", false);
                Context.ApplicationInstance.CompleteRequest();

            }
        }

    }
}