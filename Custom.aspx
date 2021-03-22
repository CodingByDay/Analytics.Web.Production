<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="peptak._Default" %>

<%@ Register assembly="DevExpress.Dashboard.v20.2.Web.WebForms, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-sm-12">
        
        </div>
    </div>
    <div class="jumbotron">

        <dx:ASPxDashboard ID="ASPxDashboard2" runat="server" AllowCreateNewJsonConnection="True" AllowExecutingCustomSql="True" AllowInspectAggregatedData="True" AllowInspectRawData="True" EnableCustomSql="True" EnableTextBoxItemEditor="True" LimitVisibleDataMode="DesignerAndViewer">
        </dx:ASPxDashboard>
    </div>

   


</asp:Content>
