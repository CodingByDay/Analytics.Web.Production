﻿using DevExpress.DashboardCommon;
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace peptak
{
    public partial class indextenant : System.Web.UI.Page
    {
        public static string ConnectionString;
        private SqlConnection conn;
        private int companyID;
        private int stringID;
        private string stringConnection;
        private int value;
        private bool flag = false;
        private string state;
        private string returnString;

        protected void Page_Load(object sender, EventArgs e)
        {

                HtmlAnchor admin = this.Master.FindControl("backButtonA") as HtmlAnchor;

                admin.Visible = false;
                ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

          
                ASPxDashboard3.LimitVisibleDataMode = LimitVisibleDataMode.DesignerAndViewer;

                ASPxDashboard3.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());

                var dataBaseDashboardStorage = new DataBaseEditableDashboardStorageCustom(ConnectionString);

                ASPxDashboard3.SetDashboardStorage(dataBaseDashboardStorage);

                ASPxDashboard3.ConfigureDataConnection += ASPxDashboard1_ConfigureDataConnection;

                ASPxDashboard3.AllowCreateNewDashboard = true;

                ASPxDashboard3.DashboardLoading += ASPxDashboard1_DashboardLoading;

                ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeGreenMist;

                ASPxDashboard3.DataRequestOptions.ItemDataRequestMode = ItemDataRequestMode.BatchRequests;
                if(Session["DesignerPayed"] is null)
               {
                ASPxDashboard3.WorkingMode = WorkingMode.ViewerOnly;

               }
              if (Session["DesignerPayed"].ToString() == "true")
                {

                    if (Session["FirstLoad"].ToString() != "true")
                    {
                        ASPxDashboard3.InitialDashboardId = Session["id"].ToString();
                    }
                ASPxDashboard3.WorkingMode = WorkingMode.Viewer;


            }
            else

                {

                ASPxDashboard3.WorkingMode = WorkingMode.ViewerOnly;

                }

               if (Request.Cookies.Get("state") is null)
               {

                Response.Cookies["state"].Value = "light";

               }

              else

              {

                state = Request.Cookies.Get("state").Value;

                switch (state)
                {
                    case "light":
                        ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeLight;
                        break;
                    case "dark":
                        ASPxDashboard3.ColorScheme = ASPxDashboard.ColorSchemeDarkMoon;
                        break;

                }
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

        private void ASPxDashboard1_DashboardLoading(object sender, DevExpress.DashboardWeb.DashboardLoadingWebEventArgs e)
        {
            if (Session["DesignerPayed"].ToString() == "true")
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
                        if (_initial != "false")
                        {
                            DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl);
                            Session["InitialPassed"] = "false";

                        }
                    }
                    else
                    {
                        Session["mode"] = "Designer";
                        Session["flag"] = "true";

                    }
                }
                else
                {
                    Session["flag"] = "false";

                    Session["InitialPassed"] = "true";
                }
            } else
            {

            }
        }

        private bool checkDB(string ID)
        {
                bool flag = false; /* For added security default=false */

                string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
                var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

                conn = new SqlConnection(ConnectionString);
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

        private void ASPxDashboard1_ConfigureDataConnection(object sender, DevExpress.DashboardWeb.ConfigureDataConnectionWebEventArgs e)
        {
            ConnectionStringSettings conn = GetConnectionString();

            CustomStringConnectionParameters parameters =
                  (CustomStringConnectionParameters)e.ConnectionParameters;
            MsSqlConnectionParameters msSqlConnection = new MsSqlConnectionParameters();
            // Here is the error.

            parameters.ConnectionString = conn.ConnectionString;

        
        }

        private ConnectionStringSettings GetConnectionString()
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
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

            var a = get_connectionStringName(companyID);
          


            ConnectionStringSettings stringFinal = ConfigurationManager.ConnectionStrings[a];

            return stringFinal;

        }

        private string get_connectionStringName(int companyID)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);

            conn.Open();
            SqlCommand cmd = new SqlCommand($"select databaseName from companies where id_company={companyID}", conn);

            try
            {
                string result = cmd.ExecuteScalar().ToString();
                returnString = result; 

            }


            catch (Exception error)
            {
                // Implement logging here.
                Response.Write($"<script type=\"text/javascript\">alert('Prišlo je do napake... {error}'  );</script>");
            }


            cmd.Dispose();
            conn.Close();
            return returnString;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int get_id_string(int id)
        {
            string UserNameForChecking = HttpContext.Current.User.Identity.Name; /* For checking admin permission. */
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);

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
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);
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