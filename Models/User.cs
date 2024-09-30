using Dash.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Dash.Models
{
    public static class UserHelper
    {
        public static List<User> getAll()
        {
            List<User> payload = new List<User>();

            return payload;
        }
    }

    public class GraphName
    {
        public string name { get; set; }
    }

    public class User
    {
        private string connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        public string uname { get; set; }

        public string password { get; set; }

        public string user_role { get; set; }

        public string view_allowed { get; set; }

        public string email { get; set; }

        public string dashboards { get; set; }

        public User(string uname, string Pwd, string userRole, string ViewState, string email)
        {
            this.uname = uname;
            this.password = Pwd;
            this.user_role = userRole;
            this.view_allowed = ViewState;
            this.email = email;
        }

        public User(string uname, string Pwd, string userRole, string ViewState, string email, string graphs)
        {
            this.uname = uname;
            this.password = Pwd;
            this.user_role = userRole;
            this.view_allowed = ViewState;
            this.email = email;
            this.dashboards = graphs;
        }

        public List<GraphName> GetGraphNames()
        {
            List<GraphName> data = new List<GraphName>();
            var splitted = this.dashboards.Split(',');
            foreach (string split in splitted)
            {
                data.Add(new GraphName { name = split });
            }
            return data;
        }

        public void RemoveGraph(string name)
        {
            try
            {
                var graphs = GetGraphNames();
                var toRemove = graphs.Find(x => x.name == name);
                graphs.RemoveAt(graphs.IndexOf(toRemove));

                string returnValue = string.Empty;
                foreach (var x in graphs)
                {
                    returnValue += $"{x.name},";
                }
                this.dashboards = returnValue;
            }
            catch
            {
            }
        }

        public bool IsPermited(string name)
        {
            bool isOk = false;

            try
            {
                var graphs = GetGraphNames();
                foreach (var graph in graphs)
                {
                    if (graph.name == name)
                    {
                        isOk = true;
                    }
                }

                return isOk;
            }
            catch
            {
                isOk = false;
                return isOk;
            }
        }

        public void AddGraph(string name)
        {
            try
            {
                var graphs = GetGraphNames();
                graphs.Add(new GraphName { name = name });

                string returnValue = string.Empty;
                foreach (var x in graphs)
                {
                    returnValue += $"{x.name},";
                }
                this.dashboards = returnValue;
            }
            catch
            {
            }
        }

        public User(string uname, bool isSQL)
        {
            if (isSQL)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE uname='{uname}'", conn);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            this.uname = sdr["uname"].ToString();
                            this.user_role = sdr["userRole"].ToString();
                            this.view_allowed = sdr["ViewState"].ToString();
                            this.dashboards = sdr["graphs"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(typeof(tenantadmin), ex.Message);
                    }
                }
            }
        }
    }
}