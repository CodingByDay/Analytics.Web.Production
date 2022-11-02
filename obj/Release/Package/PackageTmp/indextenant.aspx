<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="indextenant.aspx.cs" Inherits="Dash.indextenant" %>

<%@ Register assembly="DevExpress.Dashboard.v20.2.Web.WebForms, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="css/all.css" />

    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
    <webopt:bundlereference runat="server" path="~/css/graphs.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript" defer></script>
    <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="css/all.css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
    <webopt:bundlereference runat="server" path="~/css/graphs.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js" integrity="sha512-3j3VU6WC5rPQB4Ld1jnLV7Kd5xr+cq9avvhwqzbH/taCRNURoeEpoPBK9pDyeukwSxwRPJ8fDgvYXd6SkaZ2TA==" crossorigin="anonymous" referrerpolicy="no-referrer" async></script>
    <script src="js/SaveAsExtension.js" type="text/javascript"></script>
    <script src="js/DeleteExtension.js" type="text/javascript"></script>

    <script src="js/application/admin.js"></script>
      <div class="overlay" id="overlay" hidden>
    

        <script>

            //var contextMenu = false;
            //window.oncontextmenu = function (e) {
            //        var x = e.pageX;
            //        var y = e.pageY;
            //        var frompoint = document.elementsFromPoint(x, y);
            //        var dash = frompoint[1].textContent.trim();
            //        if (confirm(`Odpri ${dash} v novi kartici?`)) {
            //            setCookie("tab", dash, 365);
            //            NewTab();
            //        }
            //        e.preventDefault();
            //}


            //function NewTab() {
            //    window.open(
            //        "https://dash.in-sist.si/indextenant.aspx", "_blank");
            //}

            //function askPopup() {
            //    console.log("ask");
            //}

            function PerformDelete(dashboardid) {
                setCookie("temp", dashboardid, 365);
                $.ajax({
                    type: "POST",
                    url: 'indextenant.aspx/DeleteItem',
                    data: `{id: ${dashboardid}}`,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                    },
                    error: function (e) {
                    }
                });
                window.location.reload();

            }
        </script>
        <style>
        .dx-widget  {  
        color: #333!important;  
        font-weight: normal!important;  
        font-size: 11px!important;  
        } 

        #MainContent_ASPxDashboard3 {
        height: 100% !important;
        }

        </style>
        <div class="col-sm-12">
        <div style="position: absolute; left: 80px; right: 0; top:0; bottom:30px;">
 
        </div>
        </div>
        </div>
        <div class="jumbotron">
        

        </div>
         <!-- Defines the "Save As" extension template. -->
         <script type="text/html" id="dx-save-as-form">
        <div>Dashboard Name:</div>
        <div style="margin: 10px 0" data-bind="dxTextBox: { value: newName }"></div>
        <div data-bind="dxButton: { text: 'Save', onClick: saveAs }"></div>
         </script>
 
        <div style="position: absolute; left: 0; right: 0; top:35px; bottom:0;">

        <dx:ASPxDashboard ID="ASPxDashboard3" runat="server" AllowCreateNewJsonConnection="True" ClientInstanceName="dashboard" DataRequestOptions-ItemDataRequestMode="BatchRequests"  AllowExecutingCustomSql="True" AllowInspectAggregatedData="True" MobileLayoutEnabled="Auto" AllowInspectRawData="True" EnableCustomSql="True" EnableTextBoxItemEditor="True">
        <ClientSideEvents BeforeRender="onBeforeRender"
            DashboardInitializing ="ask"
            ItemCaptionToolbarUpdated="onItemCaptionToolbarUpdated"
            ItemWidgetCreated="customizeWidgets"
            ItemWidgetUpdated="updatecustomizeWidgets"
            DashboardInitialized="correctTheLoadingState" />
      </dx:ASPxDashboard>
        </div>

  
</asp:Content>