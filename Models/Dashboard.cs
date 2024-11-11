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
                    // Serialize the data to JSON format
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var username = HttpContext.Current.User.Identity.Name;
                    // Use parameterized query to avoid SQL injection
                    using (SqlCommand cmd = new SqlCommand("UPDATE CustomNames SET names = @Names WHERE company_id = @CompanyID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Names", json);
                        cmd.Parameters.AddWithValue("@CompanyID", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), $"Error updating company names: {ex.Message}, StackTrace: {ex.StackTrace}");
                }
            }
        }

        public void UpdateGraphs(List<Names> payload, int companyId)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    var json = JsonConvert.SerializeObject(payload);
                    var insert = new SqlCommand($"UPDATE custom_names SET names='{json}' WHERE company_id={companyId};", conn);
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
                        var ID = (reader["id"].ToString());
                        var Caption = (reader["caption"].ToString());
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

        public void DeleteCustomNames(int companyID)
        {
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM custom_names WHERE company_id = @CompanyID", conn))
                    {
                        deleteCommand.Parameters.AddWithValue("@CompanyID", companyID);
                        deleteCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(typeof(Admin), ex.Message);
                }
            }
        }
    }
}