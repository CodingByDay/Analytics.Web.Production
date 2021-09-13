<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Emulator.aspx.cs" Inherits="peptak.Emulator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      ﻿<script src="http://code.jquery.com/jquery-3.3.1.min.js"  integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.2/knockout-min.js"></script>
    <script type="text/javascript" src="https://cdn3.devexpress.com/jslib/18.1.5/js/dx.all.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/18.1.5/css/dx.common.css" />
        <link href="Content/styles.css?v=2013" rel="stylesheet" />
    <link href="Content/sidebar/sidebar.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/18.1.5/css/dx.light.css" />
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
 