﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace Dash
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private SqlConnection conn;

        // Comment
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPasswordResetLinkValid())
                {
                    Response.Write($"<script type=\"text/javascript\">alert('Čas za resetiranje gesla je potekal ali link ni več vredo.'  );</script>");
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
            if (checkEquality())
            {
                if (ChangeUserPassword())
                {
                    Response.Write($"<script type=\"text/javascript\">alert('Geslo uspešno spremenjeno.'  );</script>");
                }
                else
                {
                    Response.Write($"<script type=\"text/javascript\">alert('Link za spremembo gesla je potekal ali ni vejaven.'  );</script>");
                }
            }
            else
            {
                Response.Write($"<script type=\"text/javascript\">alert('Gesla niso ista.'  );</script>");
            }
        }

        private bool IsPasswordResetLinkValid()
        {
            List<SqlParameter> paramList = new List<SqlParameter>()
         {
         new SqlParameter()
          {
            ParameterName = "@GUID",
            Value = Request.QueryString["uid"]
          }
        };

            return ExecuteSP("sp_is_password_reset_link_valid", paramList);
        }

        private bool ChangeUserPassword()
        {
            List<SqlParameter> paramList = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = "@GUID",
                    Value = Request.QueryString["uid"]
                },
                new SqlParameter()
                {
                    ParameterName = "@Password",
                    Value = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd.Text, "SHA1")
                }
            };

            return ExecuteSP("sp_change_password", paramList);
        }

        private bool checkEquality()
        {
            if (pwd.Text == REpwd.Text)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ExecuteSP(string SPName, List<SqlParameter> SPParameters)
        {
            var ConnectionString = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

            conn = new SqlConnection(ConnectionString);

            using (conn)
            {
                SqlCommand cmd = new SqlCommand(SPName, conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (SqlParameter parameter in SPParameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                conn.Open();
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
    }
}