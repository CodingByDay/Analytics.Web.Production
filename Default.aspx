<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="peptak._Default" %>

<%@ Register assembly="DevExpress.Dashboard.v20.2.Web.WebForms, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
        <script>



          function onBeforeRender(sender) {
        var dashboardControl = sender.GetDashboardControl();
        // ...
                dashboardControl.registerExtension(new DevExpress.Dashboard.DashboardPanelExtension(dashboardControl));
    }
            $(document).keypress(function (e) { if (e.keyCode === 13) { e.preventDefault(); return false; } });

            $(function () {

                $(':text').bind('keydown', function (e) { //on keydown for all textboxes  

                    if (e.keyCode == 13) //if this is enter key  

                        e.preventDefault();

                });

            });  

        </script>
    
        <div class="col-sm-12">
        <div style="position: absolute; left: 80px; right: 0; top:0; bottom:30px;">
 
</div>
        </div>
    </div>
    <div class="jumbotron">
        

</div>
   
 
<div style="position: absolute; left: 0; right: 0; top:60px; bottom:0;">
    <dx:ASPxDashboard ID="ASPxDashboard1" runat="server" AllowCreateNewJsonConnection="True" AllowExecutingCustomSql="True" AllowInspectAggregatedData="True"    MobileLayoutEnabled="Auto" AllowInspectRawData="True" DashboardStorageFolder="~/App_Data/Dashboards" EnableCustomSql="True" EnableTextBoxItemEditor="True">
        <ClientSideEvents BeforeRender="onBeforeRender" />
    </dx:ASPxDashboard>
</div>

   


</asp:Content>
