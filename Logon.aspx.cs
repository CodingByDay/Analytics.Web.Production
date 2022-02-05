using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;
using System.Configuration;

namespace Dash
{
    public partial class Logon : System.Web.UI.Page
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private string role;
        private List<String> strings = new List<string>();
        private bool passport = false;

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

        private string getRole(string username, string password)

        {

            var connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(connection);
            conn.Open();
            var hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"select userRole from Users where uname='{username}' and Pwd='{hashed}';", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                role = (reader["userRole"].ToString());
            }
            return role;
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

        private bool getCurrentID(string name)
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            // Create SqlCommand to select pwd field from users table given supplied userName.
            cmd = new SqlCommand($"select id_company from users where uname='{name}';", conn);
            var result = cmd.ExecuteScalar();
            int FinalCurrentID = (int)result;



            var checkingDesigner = new SqlCommand($"select Designer from companies where id_company={FinalCurrentID};", conn);

            var flag = checkingDesigner.ExecuteScalar();

            var flagINT = (int)flag;
            cmd.Dispose();
            checkingDesigner.Dispose();
            conn.Close();
            if(flagINT==1)
            {
                return true;
            } else
            {
             return false;
            }

        }





        private bool ValidateUser(string userName, string passWord)
        {
            SqlConnection conn;
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


                var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                conn = new SqlConnection(ConnectionString);
                conn.Open();

                // Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("Select pwd from users where uname=@userName", conn);
                cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25);
                cmd.Parameters["@userName"].Value = userName;

                // Execute command and fetch pwd field into lookupPassword string.
                lookupPassword = (string)cmd.ExecuteScalar();

                // Cleanup command and connection objects.
                cmd.Dispose();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                // Add error handling here for debugging.
                // This error message should not be sent back to the caller.
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
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

        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            var role = getRole(txtUserName.Value, txtUserPass.Value);
            Session["conn"] = "";
            //if (Session["passport"].ToString() == "true")
            //{
            //    Session["conn"] = databaseList.SelectedValue;
                validate();
            //}
            //else
            //{

            //    if (role == "SuperAdmin")
            //    {
            //        database.Visible = true;
            //        passport = true;
            //        Session["passport"] = "true";
            //    }
            //    else
            //    {
            //        validate();
            //    }
            //}
            

        }

   private void validate()
        {
            Response.Cookies["EDIT"].Value = "no";
            Session["change"] = "no";
            if (ValidateUser(txtUserName.Value, txtUserPass.Value))
            {

                var isDesigner = getCurrentID(txtUserName.Value);
                var isUserAllowed = isIndividualAllowed(txtUserName.Value);
                if (isDesigner)
                {
                    Session["DesignerPayed"] = "true";
                    if(isUserAllowed)
                    {
                        Session["UserAllowed"] = "true";                    
                    }
                    else {
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
                DateTime.Now.AddMinutes(30), chkPersistCookie.Checked, "your custom data");
                cookiestr = FormsAuthentication.Encrypt(tkt);
                ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                if (chkPersistCookie.Checked)
                    ck.Expires = tkt.Expiration;
                ck.Path = FormsAuthentication.FormsCookiePath;
                Response.Cookies.Add(ck);


                string strRedirect;
                role = getRole(txtUserName.Value, txtUserPass.Value);

                if (role == "SuperAdmin")
                {
                    strRedirect = "index.aspx";
                    Session["mode"] = "ViewerOnly";
                    Session["flag"] = "false";
                    Session["id"] = "2";
                    Session["InitialPassed"] = "false";
                    Session["FirstLoad"] = "true";
                    // For some reason this doesn't fire.
                    Session["value"] = "Skaza";
                    Response.Redirect(strRedirect, true);
                }
                else
                {
                    Session["value"] = "Skaza";
                    Session["flag"] = "false";
                    Session["id"] = "2";
                    Session["InitialPassed"] = "false";
                    Session["FirstLoad"] = "true";

                    strRedirect = "indextenant.aspx";
                    Response.Redirect(strRedirect, true);
                }



                conn.Close();
                conn.Dispose();
            }
            else
            {
                Response.Redirect("logon.aspx", true);
            }
        }

        private bool isIndividualAllowed(string value)
        {
            conn.Close();
            conn.Open(); 
            var checkingDesigner = new SqlCommand($"select ViewState from users where uname='{value}';", conn);

            var flag = checkingDesigner.ExecuteScalar();

            string result = flag.ToString();
            cmd.Dispose();
            checkingDesigner.Dispose();
            conn.Close();
            if (result == "Viewer&Designer")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected void reset_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResetPassword.aspx");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("membership.aspx", true);
        }

        protected void membership_Click(object sender, EventArgs e)
        {
            Response.Redirect("membership.aspx", true);
        }
    }
    }


