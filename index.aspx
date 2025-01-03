﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Dash.index" %>
<%@ Register assembly="DevExpress.Dashboard.v24.1.Web.WebForms, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>






<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
      
<style>


.popup-container {
    display: flex;
    justify-content: center; /* Center horizontally */
    align-items: center; /* Center vertically */
    position: fixed; /* Ensure it covers the viewport */
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5); /* Optional: semi-transparent background */
    z-index: 999; /* Ensure it's on top */
}


.assign-metadata {
    background: white; /* Background color */
    border-radius: 8px; /* Rounded corners */
    padding: 20px; /* Inner padding */
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Shadow for better visibility */
    width: 80%; /* Adjust width as needed */
    max-width: 600px; /* Maximum width */
    max-height: 80%; /* Maximum height */
    overflow: auto; /* Add scroll if content overflows */
}

#MainContent_ASPxDashboard3 {

height: 100% !important;

}





</style>

        <script async>

            window.onload = function () {
                setInterval(checkViewerMode, 1000);
            };
            function AssignMetadata(dashboardid) {
                showMetadataPopup();
            }              
   
            function PerformDelete(dashboardid) {
                setCookie("temp", dashboardid, 365);
                $.ajax({
                    type: "POST",
                    url: 'index.aspx/DeleteItem',
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


            function onItemCaptionToolbarUpdated(s, e) {
                var control = dashboard.GetDashboardControl();
                design = control.isDesignMode();

                if (!design) {
                    var list = dashboard.GetParameters().GetParameterList();
                    if (list.length > 0) {

                        window.item_caption = e.Options.staticItems[0].text;
                        var parameterized_values = regex_return(item_caption);
                        if (parameterized_values.length != 0) {
                            parameterized_values.forEach((singular) => {
                                const found = list.find(element => element.Name == singular)
                                indexOfElement = list.indexOf(found)
                                if (found != null && indexOfElement != -1) {
                                    text_to_replace = "#" + found.Name
                                    try {
                                        text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value.toLocaleDateString("uk-Uk")
                                    } catch (err) {
                                        text_replace = dashboard.GetParameters().GetParameterList()[indexOfElement].Value
                                    }
                                    window.item_caption = window.item_caption.replace(text_to_replace, text_replace);
                                    e.Options.staticItems[0].text = window.item_caption;
                                }
                            })
                        }

                    }
                }
            }


            var extension;

            /**
             *  
             * @param sender
             */

            function onBeforeRender(sender) {

                var dashboardControl = sender.GetDashboardControl();
                extension = new DevExpress.Dashboard.DashboardPanelExtension(dashboardControl);
                dashboardControl.surfaceLeft(extension.panelWidth);
                dashboardControl.registerExtension(extension);
                dashboardControl.registerExtension(new DeleteDashboardExtension(sender));
                // dashboardControl.registerExtension(new AssignMetadataExtension(sender));
                dashboardControl.unregisterExtension("designerToolbar");


            }

            function setCookie(cname, cvalue, exdays) {
                const d = new Date();
                d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
                let expires = "expires=" + d.toUTCString();
                document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
            }

            /**
             * Getting the cookie value.
             * @param cname
             */

            function getCookie(cname) {
                let name = cname + "=";
                let decodedCookie = decodeURIComponent(document.cookie);
                let ca = decodedCookie.split(';');
                for (let i = 0; i < ca.length; i++) {
                    let c = ca[i];
                    while (c.charAt(0) == ' ') {
                        c = c.substring(1);
                    }
                    if (c.indexOf(name) == 0) {
                        return c.substring(name.length, c.length);
                    }
                }
                return "";
            }



            $(document).keypress(function (e) { if (e.keyCode === 13) { e.preventDefault(); return false; } });

            $(function () {

                $(':text').bind('keydown', function (e) { // on keydown for all textboxes  

                    if (e.keyCode == 13) // if this is enter key  

                        e.preventDefault();

                });

            });

            /**
             * Change the visibility of the collapsable hamburger menu. */
            function toggleVisibilityHide(toHide) {
                try {
                    var picture = document.getElementById("pic")
                    if (toHide == true) {

                        picture.style.visibility = "hidden"
                    } else {
                        picture.style.visibility = "visible"
                    }
                } catch (ex) {
                    return;
                }
            }

            /* Jquery function to handle hamburger clicked */
            $(document).ready(function () {
                $("#pic").mouseover(function () {

                    var expand = getCookie("expand");
                    if (expand == "true") {
                        onExpand();
                    } else {
                        var control = dashboard.GetDashboardControl();
                        design = control.isDesignMode();
                      
                        if (design == false) {
                            onCollapse();
                        }
                    }


                });

            });

            function show() {
                $('.dx-overlay-content').show();
                console.log("Show");
                $(".dx-dashboard-surface").attr('style', 'left: 250px !important');
                changePicStateHideIt(true);

            }


            function hide() {

                $('.dx-overlay-content').hide();
                console.log("hide");
                $(".dx-dashboard-surface").attr('style', 'left: 10px !important');
                changePicStateHideIt(false);

            }

            function onExpand() {

                var control = dashboard.GetDashboardControl();
                extension.showPanelAsync({}).done(function (e) {

                    control.surfaceLeft(e.surfaceLeft);
                    setCookie("expand", "false", 365);

                });
            }

            function correctTheLoadingState() {

                var control = dashboard.GetDashboardControl();
                design = control.isDesignMode();
                if (design == false) {
                    onCollapse();
                }


            }


            function onCollapse() {

                var control = dashboard.GetDashboardControl();
                extension.hidePanelAsync({}).done(function (e) {
                    control.surfaceLeft(e.surfaceLeft);
                    toggleVisibilityHide(false);
                    setCookie("expand", "true", 365);

                });
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
            function showMetadataPopup() {
                $('#assignMetadataModal').modal('show');
            }

            function hideMetadataPopup() {
                $('#assignMetadataModal').modal('hide');
            }


            function showNotificationDevexpress(message) {
                DevExpress.ui.notify(message);
                
            }

        </script>
    
        <div class="col-sm-12">
        <div style="position: absolute; left: 80px; right: 0; top:0; bottom:30px;">
 
</div>
        </div>
    </div>
    <div class="jumbotron">
        

</div>
   
 
<div style="position: absolute; left: 0; right: 0; top:35px; bottom:0;">

    <dx:ASPxDashboard ID="ASPxDashboard3" runat="server" AllowCreateNewJsonConnection="True" ClientInstanceName="dashboard" DataRequestOptions-ItemDataRequestMode="BatchRequests"  AllowExecutingCustomSql="True" AllowInspectAggregatedData="True" MobileLayoutEnabled="Auto" AllowInspectRawData="True" EnableCustomSql="True" EnableTextBoxItemEditor="True">
        <ClientSideEvents BeforeRender="onBeforeRender"
                          ItemWidgetCreated="customizeWidgets"
                          DashboardTitleToolbarUpdated ="onDashboardTitleToolbarUpdated"
                          DashboardEndUpdate="onDashboardEndUpdate"
                          ItemWidgetUpdated="updatecustomizeWidgets"        
                          ItemCaptionToolbarUpdated="onItemCaptionToolbarUpdated" 
                          DashboardInitialized="correctTheLoadingState" 
                         
                          />
    </dx:ASPxDashboard>
</div>







</asp:Content>