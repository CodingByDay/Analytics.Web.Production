<%@ Page Title="Emulator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Emulator.aspx.cs" Inherits="Dash.Emulator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function emulatorLoaded(emulator) {
            emulator.contentWindow.ASPxDashboard.DashboardInitialized.AddHandler(function(s, e) { dxDemo.Navigation.saveToUrl("dashboardId", e.DashboardId) })
            emulator.contentWindow.ASPxDashboard.DashboardStateChanged.AddHandler(function(s, e) { dxDemo.Navigation.saveToUrl("dashboardState", e.DashboardState) })
            window.getDashboardControl = function() {
                return emulator.contentWindow.getDashboardControl();
            }
        }
    </script>
    <div class="phone-wrapper">
        <iframe id="emulator" src="<%= Page.ResolveUrl("/MobileDashboard.aspx") + "?" + Request.QueryString.ToString() %>" onload="emulatorLoaded(this)"></iframe>
    </div>
</asp:Content>
 