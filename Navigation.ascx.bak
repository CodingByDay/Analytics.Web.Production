<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navigation.ascx.cs" Inherits="Dash.Navigation" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<nav>
    <div id="NavControl" class="nav-control" style="visibility: hidden">
        <dx:ASPxButton runat="server" ID="NavigationBreadCrumbsButton" ClientInstanceName="NavigationBreadCrumbsButton" CssClass="navigation-breadcrumbs-button"
            Text="All Sections" Width="100%" AutoPostBack="false" HorizontalAlign="Left" UseSubmitBehavior="false" Height="34">
            <FocusRectBorder BorderWidth="0" />
            <FocusRectPaddings Padding="0" />
            <Image SpriteProperties-CssClass="icon"></Image>
            <ClientSideEvents Click="function(){ NavControl.onNavigationBreadCrumbsButtonClick(); }" />
        </dx:ASPxButton>
        <div class="nav-tree-view">
            <span id="breadCrumbsText" class="breadCrumbs"></span>
            <dx:ASPxTreeView runat="server" ID="NavigationTreeView"  ClientInstanceName="NavigationTreeView" DataSourceID="XmlDataSource1" Width="100%"
                ShowTreeLines="false" ShowExpandButtons="true" OnNodeDataBound="NavigationTreeView_NodeDataBound" OnPreRender="NavigationTreeView_PreRender" EnableHotTrack="false"
                TextField="Title" NavigateUrlField="Url" Theme="Moderno">
                <Styles>
                    <Node CssClass="node" />
                    <Elbow CssClass="elbow" />
                </Styles>
            </dx:ASPxTreeView>
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
            <script>
                $(document).ready(function () {
                 
                  
                    var NavControl = new Site.Nav.NavigationControl(this.NavigationTreeView, this.NavigationBreadCrumbsButton, 'NavControl', 'breadCrumbsText');
                    NavControl.Init();
                });
            
            </script>
        </div>
    </div>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Navigation.xml" XPath="/namespace/*" />
</nav>
