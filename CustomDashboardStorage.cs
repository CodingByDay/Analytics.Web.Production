using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using DevExpress.DashboardWeb;

namespace WebDesigner_CustomDashboardStorage
{
    public class CustomDashboardStorage : IEditableDashboardStorage
    {
        DashboardStorageDataSet dashboards = new DashboardStorageDataSet();

        // Adds a dashboard with the specified ID and name to a DataSet. 
        // Note that the 'DashboardID' column is an auto-increment column that is used to store unique dashboard IDs.
        public string AddDashboard(XDocument dashboard, string dashboardName)
        {
            DataRow newRow = dashboards.Tables[0].NewRow();
            newRow["DashboardName"] = dashboardName;
            newRow["DashboardXml"] = dashboard;
            dashboards.Tables[0].Rows.Add(newRow);
            return newRow["DashboardID"].ToString();
        }

        // Gets information about dashboards available in a DataSet.
        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<DashboardInfo> dashboardInfos = new List<DashboardInfo>();
            foreach (DataRow row in dashboards.Tables[0].Rows)
            {
                DashboardInfo dashboardInfo = new DashboardInfo();
                dashboardInfo.ID = row["DashboardID"].ToString();
                dashboardInfo.Name = row["DashboardName"].ToString();
                dashboardInfos.Add(dashboardInfo);
            }
            return dashboardInfos;
        }

        // Loads a dashboard corresponding to the specified ID.
        public XDocument LoadDashboard(string dashboardID)
        {
            DataRow currentRow = dashboards.Tables[0].Rows.Find(dashboardID);
            XDocument dashboardXml = XDocument.Parse(currentRow["DashboardXml"].ToString());
            return dashboardXml;
        }

        // Saves the dashboard with the specified ID to a DataSet.
        public void SaveDashboard(string dashboardID, XDocument dashboard)
        {
            DataRow currentRow = dashboards.Tables[0].Rows.Find(dashboardID);
            currentRow["DashboardXml"] = dashboard;
        }
    }
}