<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="indextenant.aspx.cs" Inherits="Dash.indextenant" %>

<%@ Register assembly="DevExpress.Dashboard.v20.2.Web.WebForms, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css" />
	<link rel="stylesheet" href="css/all.css" />

	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
    <webopt:bundlereference runat="server" path="~/css/graphs.css" />
    <link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css" />
	<link rel="stylesheet" href="css/all.css" />
	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <div class="row">
    <webopt:bundlereference runat="server" path="~/css/graphs.css" />
    <link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js" integrity="sha512-3j3VU6WC5rPQB4Ld1jnLV7Kd5xr+cq9avvhwqzbH/taCRNURoeEpoPBK9pDyeukwSxwRPJ8fDgvYXd6SkaZ2TA==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>

    <script src="js/application/admin.js"></script>
        <style>
        .dx-widget{  
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
   
 
        <div style="position: absolute; left: 0; right: 0; top:35px; bottom:0;">

        <dx:ASPxDashboard ID="ASPxDashboard3" runat="server" AllowCreateNewJsonConnection="True" ClientInstanceName="dashboard"  AllowExecutingCustomSql="True" AllowInspectAggregatedData="True" MobileLayoutEnabled="Auto" AllowInspectRawData="True" EnableCustomSql="True" EnableTextBoxItemEditor="True">
        <ClientSideEvents BeforeRender="onBeforeRender"
                          ItemCaptionToolbarUpdated="onItemCaptionToolbarUpdated" 
                          ItemWidgetCreated="customizeWidgets"
                          ItemWidgetUpdated="updatecustomizeWidgets"                   
                          DashboardInitialized="correctTheLoadingState"                      
                          />
      </dx:ASPxDashboard>
        </div>

  
</asp:Content>