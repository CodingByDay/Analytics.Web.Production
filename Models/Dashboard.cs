using Dash.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Dash.Models
{
    public class Dashboard
    {
        public string Connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        public class DashboardInternal
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string CustomName { get; set; }
        }

        public List<DashboardInternal> AllGraphs { get; set; } = new List<DashboardInternal>();

        public Dashboard(int Id)
        {
            this.AllGraphs = GetGraphs(Id);
        }

        public void UpdateCompanyNames(List<DashboardInternal> data, int id)
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
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
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
                    var insert = new SqlCommand($"UPDATE custom_names SET names='{json}' WHERE company_id={companyID};", conn);
                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
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
                    var cmd = new SqlCommand($"SELECT d.ID, d.caption FROM dashboards d;", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Names> data = new List<Names>();
                    while (reader.Read())
                    {
                        var Caption = (reader["caption"].ToString());
                        data.Add(new Names { original = Caption, custom = Caption });
                    }
                    reader.Close();
                    var json = JsonConvert.SerializeObject(data);
                    var insert = new SqlCommand($"INSERT INTO custom_names(names, company_id) values('{json}', {id});", conn);
                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
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
                    var cmd = new SqlCommand($"SELECT d.id, d.caption, cn.company_id, cn.names FROM dashboards d, custom_names cn WHERE cn.company_id={id};", conn);
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
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    return lc;
                }
            }
        }

        public string GetSingularNameOriginal(int id, string customInner)
        {
            string original = string.Empty;
            var names = GetNamesCurrent(id);
            try
            {
                original = names.FirstOrDefault(x => x.custom == customInner).original;
                using (SqlConnection conn = new SqlConnection(Connection))
                {
                    conn.Open();
                    var cmd = new SqlCommand($"SELECT d.id FROM dashboards d WHERE d.caption = '{original}'", conn);
                    int result = (int)cmd.ExecuteScalar();
                    original = result.ToString();
                }
            }
            catch (Exception)
            {
                return original;
            }

            return original;
        }

        public List<Names> GetNamesCurrent(int id)
        {
            var graphs = GetGraphs(id);
            List<Dashboard.Names> data = new List<Dashboard.Names>();
            foreach (var obj in graphs)
            {
                data.Add(new Dashboard.Names { original = obj.Name, custom = obj.CustomName });
            }
            return data;
        }

        public List<DashboardInternal> GetGraphs(int id)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();

                    var username = HttpContext.Current.User.Identity.Name;
                    // Create SqlCommand to select pwd field from users table given supplied userName.
                    var cmd = new SqlCommand($"SELECT d.id, d.caption, cn.company_id, cn.names FROM dashboards d, custom_names cn WHERE cn.company_id={id};", conn);
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

                        DashboardInternal graph = new DashboardInternal();
                        graph.ID = ID;
                        graph.Name = Caption;
                        try
                        {
                            graph.CustomName = lc.FirstOrDefault(x => x.original == Caption).custom;
                        }
                        catch
                        {
                            graph.CustomName = Caption;
                        }

                        AllGraphs.Add(graph);
                    }

                    return AllGraphs;
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                    return AllGraphs;
                }
            }
        }

        internal void Delete(int companyID)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    var insert = new SqlCommand($"DELETE FROM CustomNames WHERE company_id={companyID};", conn);
                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.InnerException.Message);
                }
            }
        }
    }
}