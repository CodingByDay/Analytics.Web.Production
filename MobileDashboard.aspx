<%@ Page Title="" Language="C#" MasterPageFile="~/Extension.Master" AutoEventWireup="true" CodeBehind="MobileDashboard.aspx.cs" Inherits="peptak.MobileDashboard"  %>
<%@ Register Assembly="DevExpress.Dashboard.v20.2.Web.WebForms, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>
<asp:Content ContentPlaceHolderID="LayoutContentPlaceHolder" runat="server">
     <script>
        function getDashboardControl() {
            return ASPxDashboard.getDashboardControl();
        }
    </script>
    <div class="demo-content" style="top: 0;">
        
            <dx:ASPxDashboard ID="ASPxWebDashboard1" runat="server"
                ClientInstanceName="ASPxDashboard"
                Height="100%"
                WorkingMode="ViewerOnly"
                IncludeDashboardIdToUrl="True"
                IncludeDashboardStateToUrl="True"
                AllowExecutingCustomSql="true"
                OnDashboardLoading="OnDashboardLoading"
                OnDataLoading="OnDataLoading">
                <ClientSideEvents
                    BeforeRender="function (s) { onBeforeRender(s.getDashboardControl()); }"
                    DashboardInitialized="function (s, args) { onDashboardChanged({ component: s.getDashboardControl(), dashboardId: args.DashboardId }); } "
                    DashboardTitleToolbarUpdated="function(s,e) { onDashboardTitleToolbarUpdated({ component: s.getDashboardControl(), options: e.Options }); }" />
            </dx:ASPxDashboard>
      
</div>
    <script>
        if(window.parent !== window && !DevExpress.devices.current().phone) {
            DevExpress.devices.current("genericPhone");
        }
    </script>
    <!--         Custom Properties    -->    
    <script src="Content/chart-line-options-extension.js?v=2013"></script>
    <!--         Custom Item Extensions          -->
    <script src="Content/d3-funnel/d3.v4.min.js?v=1"></script>
    <script src="Content/d3-funnel/d3-funnel.min.js?v=1"></script>
    <script src="Content/d3-funnel/funnel-d3-item.js?v=1"></script>
    <script src="Content/online-map-item.js?v=1"></script>
    <script src="Content/webpage-item.js?v=1"></script>
    <!--         Custom API                      -->
    <script src="Content/save-as-extension.js?v=1"></script>
    <!--                                          -->
</asp:Content>
