using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Dash.Models
{
    public class DashboardFilters
    {
        [JsonIgnore]
        public string Connection = ConfigurationManager.ConnectionStrings["graphsConnectionString"].ConnectionString;

        public List<DashboardFilter> Filters { get; set; } = new List<DashboardFilter>();
    }




    public class DashboardFilter
    {
        public int id { get; set; }


    }
}