<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndexTenant.aspx.cs" Inherits="Dash.IndexTenant" %>

<%@ Register assembly="DevExpress.Dashboard.v23.2.Web.WebForms, Version=23.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v23.2, Version=23.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
 
      <div class="overlay" id="overlay" hidden>
    

        <script>

            var contextMenu = false;
            window.oncontextmenu = function (e) {
                    var x = e.pageX;
                    var y = e.pageY;
                    var frompoint = document.elementsFromPoint(x, y);
                    var dash = frompoint[1].textContent.trim();
                    if (dash.length < 50 && x <= 250) {

                    if (confirm(`Odpri ${dash} v novi kartici?`)) {
                        setCookie("tab", dash, 365);
                        NewTab(dash);
                    }
                    e.preventDefault();

                }
            }


            function NewTab(dash) {
                window.open(
                    `https://dash.in-sist.si/IndexTenant.aspx?p=${dash}`, "_blank");
            }

            function askPopup() {
            }

            function PerformDelete(dashboardid) {
                setCookie("temp", dashboardid, 365);
                $.ajax({
                    type: "POST",
                    url: 'IndexTenant.aspx/DeleteItem',
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




            function onDashboardTitleToolbarUpdated(sender, e) {
                e.Options.actionItems.unshift({
                    type: "button",
                    icon: "dx-dashboard-clear-master-filter",
                    hint: "Clear all filters",
                    click: function (element) {
                        ClearMasterFilterState();
                    }
                });
            }
            function ClearMasterFilterState() {

                var state = JSON.parse(dashboard.GetDashboardState());
                $.each(state.Items, function (index, element) {
                    var startState = JSON.parse(initialState);
                    debugger;
                    if (startState.Items[index]) {
                        element.MasterFilterValues = startState.Items[index].MasterFilterValues;
                    }
                    else
                        element.MasterFilterValues = [];
                });
                var newState = JSON.stringify(state);
                dashboard.SetDashboardState(newState);
            }
            var initialState = '';

            function onDashboardEndUpdate(s, e) {

                if (initialState == '') {
                    initialState = s.GetDashboardState();
                }
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
        <ClientSideEvents 
            BeforeRender="onBeforeRender"
            DashboardInitializing ="ask"
            ItemCaptionToolbarUpdated="onItemCaptionToolbarUpdated"
            ItemWidgetCreated="customizeWidgets"
            DashboardEndUpdate="onDashboardEndUpdate"
            DashboardTitleToolbarUpdated ="onDashboardTitleToolbarUpdated"
            ItemWidgetUpdated="updatecustomizeWidgets"
            DashboardInitialized="correctTheLoadingState" />
      </dx:ASPxDashboard>
        </div>

  
</asp:Content>