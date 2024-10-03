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
                Session["passport"] = "false";
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

        private bool IsCompanyDesigner(string name)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    // Query to get the id_company from users
                    using (SqlCommand cmd = new SqlCommand("SELECT id_company FROM users WHERE uname=@name;", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        var result = cmd.ExecuteScalar();
                        // Check if the result is null
                        if (result == null || result == DBNull.Value)
                        {
                            return false; // No matching user or company found
                        }
                        // Convert result to int safely
                        int companyId = (int) result;
                        // Query to check if the company has a designer
                        using (SqlCommand checkingDesigner = new SqlCommand("SELECT designer FROM companies WHERE id_company=@companyId;", conn))
                        {
                            checkingDesigner.Parameters.AddWithValue("@companyId", companyId);
                            var flag = checkingDesigner.ExecuteScalar();

                            if (flag != null && flag != DBNull.Value)
                            {
                                return true;
                            }
                            else
                            {
                                return false; 
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
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
            Session["conn"] = "";
            ValidateUser();
        }

        private void ValidateUser()
        {
            Response.Cookies["Edit"].Value = "no";
            if (ValidateUser(txtUserName.Value, txtUserPass.Value))
            {

                var IsDesignerCompany = IsCompanyDesigner(txtUserName.Value);
                var IsUserAllowed = IsUserAllowedToViewAndDesign(txtUserName.Value);

                if (IsDesignerCompany || txtUserName.Value == "Admin")
                {
                    Session["DesignerPayed"] = "true";
                    if (IsUserAllowed)
                    {
                        Session["UserAllowed"] = "true";
                    }
                    else
                    {
                        Session["UserAllowed"] = "false";
                    }
                }
                else
                {
                    Session["UserAllowed"] = "false";
                    Session["DesignerPayed"] = "false";
                }

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

                Session["current"] = "";
                string strRedirect;
                role = GetRole(txtUserName.Value, txtUserPass.Value);

                if (role == "SuperAdmin")
                {
                    strRedirect = "Index.aspx";
                    Session["mode"] = "ViewerOnly";
                    Response.Redirect(strRedirect, true);
                }
                else
                {
                    strRedirect = "IndexTenant.aspx";
                    Response.Redirect(strRedirect, true);
                }

                conn.Close();
                conn.Dispose();
            }
            else
            {
                Response.Redirect("Logon.aspx", true);
            }
        }

        private bool IsUserAllowedToViewAndDesign(string username)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    // Use a parameterized query to avoid SQL injection
                    using (SqlCommand command = new SqlCommand("SELECT view_allowed FROM users WHERE uname=@username", conn))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        // Execute the query and get the permission value
                        var permissionResult = command.ExecuteScalar();

                        // Check if the result is null
                        if (permissionResult == null || permissionResult == DBNull.Value)
                        {
                            return false; // No matching user or no permission found
                        }

                        // Convert result to string and check permission
                        string permission = permissionResult.ToString();

                        // Return true only if the permission is "Viewer&Designer"
                        return permission == "Viewer&Designer";
                    }
                }
                catch (Exception)
                {
                    // Log the exception if necessary
                    return false;
                }
            }
        }
    }
}