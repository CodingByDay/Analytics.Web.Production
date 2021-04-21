<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Custom.aspx.cs" Inherits="peptak.Custom" %>

<%@ Register assembly="DevExpress.Dashboard.v20.2.Web.WebForms, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
          function onBeforeRender(sender) {
        var dashboardControl = sender.GetDashboardControl();
        // ...
                dashboardControl.registerExtension(new DevExpress.Dashboard.DashboardPanelExtension(dashboardControl));
    }
            $(document).keypress(function (e) { if (e.keyCode === 13) { e.preventDefault(); return false; } });

            $(function () {

                $(':text').bind('keydown', function (e) { //on keydown for all textboxes  

                    if (e.keyCode == 13) // if this is enter key  

                        e.preventDefault();

                });

            });  

    </script>
    <div class="row">
        <div class="col-sm-12">
        
        </div>
    </div>
    <div class="jumbotron">

        <dx:ASPxDashboard ID="ASPxDashboard2" runat="server" AllowCreateNewJsonConnection="True" AllowExecutingCustomSql="True" AllowInspectAggregatedData="True" AllowInspectRawData="True" EnableCustomSql="True" EnableTextBoxItemEditor="True" LimitVisibleDataMode="DesignerAndViewer">
                   <ClientSideEvents BeforeRender="onBeforeRender" />

        </dx:ASPxDashboard>
    </div>

   


</asp:Content>
