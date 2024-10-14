using DevExpress.DashboardCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dash.Models
{
    public class UserDashboardState
    {
        List<DashboardStateSingle> States {  get; set; }
    }

    public class DashboardStateSingle
    {
        int DashboardId { get; set; }
        DashboardState State { get; set; }
    }
}