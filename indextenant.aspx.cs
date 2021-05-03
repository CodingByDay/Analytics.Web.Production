using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using peptak.DatabaseStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class indextenant : System.Web.UI.Page
    {
        public static string ConnectionString = @"Data Source=10.100.100.25\SPLAHOST; Database=graphs;Application Name = Dashboard; Integrated Security = false; User ID = petpakn; Password=net123321!";
        private SqlConnection conn;
        private int companyID;
        private int stringID;
        private string stringConnection;
        private int value;
        private bool flag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());

            var dataBaseDashboardStorage = new DataBaseEditableDashboardStorageCustom(ConnectionString);
            ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);
            ASPxDashboard3.ConfigureDataConnection += ASPxDashboard3_ConfigureDataConnection;
            ASPxDashboard3.AllowCreateNewDashboard = true;
            ASPxDashboard3.DashboardLoading += ASPxDashboard3_DashboardLoading;
            ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;
            if (Session["FirstLoad"].ToString() != "true")
            {
                ASPxDashboard3.InitialDashboardId = Session["id"].ToString();
            }
            if(Session["mode"].ToString() == "ViewerOnly")
            {
                ASPxDashboard3.WorkingMode = WorkingMode.ViewerOnly;
            } else
            {
                ASPxDashboard3.WorkingMode = WorkingMode.Designer;
            }
        }

   

        /// <summary>
        /// If database checked disable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// flag starts with false
        /// mode with viewer only
        /// id with 2
        ///
        ///

        private void ASPxDashboard3_DashboardLoading(object sender, DevExpress.DashboardWeb.DashboardLoadingWebEventArgs e)
        {
            Session["FirstLoad"] = "false";

            string _initial = Session["InitialPassed"].ToString();
            string ID = e.DashboardId;
            var result = checkDB(ID);
            Session["id"] = e.DashboardId.ToString();
            if (Session["flag"].ToString() == "false")
            {
            
                if (result)
                {
                  
                        Session["mode"] = "ViewerOnly";
                        Session["flag"] = "false";

                        if(_initial != "false") { 

                        DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl);
                        Session["InitialPassed"] = "false";

                    }
                }
                    else
                {
                    Session["mode"] = "Designer";
                    Session["flag"] = "true";
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl); 
                }
            } else
            {
                Session["flag"] = "false";
                Session["InitialPassed"] = "true";


            }
        }

        private bool checkDB(string ID)
        {
                bool flag = false; /* For added security default=false */
                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */

                conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select isViewerOnly from Dashboards where ID={ID}", conn);

                try
                {
                    var result = cmd.ExecuteScalar();
                    value = System.Convert.ToInt32(result);

                }


                catch (Exception error)
                {
                    // Implement logging here.
                    Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
                }
                

                cmd.Dispose();
                conn.Close();
             if(value==1)
            {
                flag = true;
            } else
            {
                flag = false;
            }

            return flag;
        }

        private void ASPxDashboard3_ConfigureDataConnection(object sender, DevExpress.DashboardWeb.ConfigureDataConnectionWebEventArgs e)
        {
            ConnectionStringSettings conn = GetConnectionString();

            CustomStringConnectionParameters parameters =
                  (CustomStringConnectionParameters)e.ConnectionParameters;

            parameters.ConnectionString = conn.ConnectionString;
        }

        private ConnectionStringSettings GetConnectionString()
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select id_company from Users where uname='{UserNameForChecking}'", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                companyID = System.Convert.ToInt32(result);

            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();

            var a = get_id_string(companyID);
            string get_conn = get_conn_name(a);


            ConnectionStringSettings stringFinal = ConfigurationManager.ConnectionStrings[get_conn];

            return stringFinal;

        }



        public int get_id_string(int id)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select ID from companies where id_company={id}", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                stringID = System.Convert.ToInt32(result);

            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();
            return stringID;
        }



        public string get_conn_name(int id)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */

            conn = new SqlConnection("server=10.100.100.25\\SPLAHOST;Database=graphs;Integrated Security=false;User ID=petpakn;Password=net123321!;");
            conn.Open();
            SqlCommand cmd = new SqlCommand($"select string from company_string where ID={id}", conn);

            try
            {
                var result = cmd.ExecuteScalar();
                stringConnection = result.ToString().Replace(" ", string.Empty);

            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();
            return stringConnection;
        }
    }
}