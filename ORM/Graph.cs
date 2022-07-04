using Dash.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Dash.ORM
{
    public class Graph
    {
        public string Connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;
    

        public class GraphInternal
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string CustomName { get; set; }

        }

        public List<GraphInternal> AllGraphs { get; set; } = new List<GraphInternal>();

        public Graph(int id_company)
        {
            this.AllGraphs = GetGraphs(id_company);
            // SetGraphs(id_company);
        }
        public void UpdateCompanyNames(List<GraphInternal> data, int id)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var username = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    var cmd = new SqlCommand($"update CustomNames set names='{json}' where company_id={id}", conn);
                    cmd.ExecuteNonQuery();                 
                }

                catch (Exception ex)
                {
                    Logger.LogError(typeof(admin), ex.InnerException.Message);
                }
            }
        }
        public void UpdateGraphs(List<Names> payload, int companyID)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    var json = JsonConvert.SerializeObject(payload);
                    var insert = new SqlCommand($"update CustomNames set names='{json}' where company_id={companyID};", conn);
                    insert.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    Logger.LogError(typeof(admin), ex.InnerException.Message);

                }
            }
        }

        public void SetGraphs(int id)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();

                    var username = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    var cmd = new SqlCommand($"select d.ID, d.Caption, cn.company_id, cn.names from Dashboards d, CustomNames cn where cn.company_id={id};", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Names> data = new List<Names>();
                    while (reader.Read())
                    {

                        var Caption = (reader["Caption"].ToString());
                        data.Add(new Names { original = Caption, custom = Caption });


                    }

                    var json = JsonConvert.SerializeObject(data);
                    var insert = new SqlCommand($"insert into CustomNames(names, company_id) values({json}, {id});");
                    insert.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    Logger.LogError(typeof(admin), ex.InnerException.Message);

                }
            }
        }
        public class Names
        {
            public string original { get; set; }
            public string custom { get; set; }
        }



        public List<Names> GetNames(int id)
        {
            List<Names> lc = new List<Names>();
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();

                    var username = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    var cmd = new SqlCommand($"select d.ID, d.Caption, cn.company_id, cn.names from Dashboards d, CustomNames cn where cn.company_id={id};", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var ID = (reader["ID"].ToString());
                        var Caption = (reader["Caption"].ToString());
                        var NamesObj = (reader["names"].ToString());
                        var uscaped = Regex.Unescape(NamesObj).Replace(@"\", string.Empty);

                        lc = JsonConvert.DeserializeObject<List<Names>>(uscaped, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });                    

                    }

                    return lc;

                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(admin), ex.InnerException.Message);
                    return lc;
                }
            }
        }

        public List<GraphInternal> GetGraphs(int id)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();

                    var username = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    var cmd = new SqlCommand($"select d.ID, d.Caption, cn.company_id, cn.names from Dashboards d, CustomNames cn where cn.company_id={id};", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var ID = (reader["ID"].ToString());
                        var Caption = (reader["Caption"].ToString());
                        var NamesObj = (reader["names"].ToString());
                        var uscaped = Regex.Unescape(NamesObj).Replace(@"\", string.Empty);

                        List<Names> lc = JsonConvert.DeserializeObject<List<Names>>(uscaped, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        GraphInternal graph = new GraphInternal();
                        graph.ID = ID;
                        graph.Name = Caption;
                        
                        graph.CustomName = lc.FirstOrDefault(x => x.original == Caption).custom;


                        AllGraphs.Add(graph);

                    }


                    return AllGraphs;
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(admin), ex.InnerException.Message);
                    return AllGraphs;
                }
            }

        }
    }
}