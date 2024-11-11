<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navigation.ascx.cs" Inherits="Dash.Navigation" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v23.2, Version=23.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v23.2, Version=23.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<nav>
    <div id="NavControl" class="nav-control" style="visibility: hidden">
         <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
        <dx:ASPxButton runat="server" ID="NavigationBreadCrumbsButton" ClientEnabled="true" ClientSideEvents-Init="initializeButton" ClientInstanceName="NavigationBreadCrumbsButtonClient" CssClass="navigation-breadcrumbs-button"
            Text="All Sections" Width="100%" AutoPostBack="false" HorizontalAlign="Left" UseSubmitBehavior="false" Height="34" Visible="false">
            <FocusRectBorder BorderWidth="0" />
            <FocusRectPaddings Padding="0" />
            <Image SpriteProperties-CssClass="icon"></Image>
            <ClientSideEvents Click="function(s, e) { breadCrumbs();   }" />
        </dx:ASPxButton>
        <div class="nav-tree-view">
            <span id="breadCrumbsText" class="breadCrumbs"></span>
            <dx:ASPxTreeView runat="server" EnableCallBacks="true"  OnNodeClick="NavigationTreeView_NodeClick" ID="NavigationTreeView"  ClientSideEvents-Init="initializeTree" EnableClientSideAPI="true" ClientInstanceName="NavigationTreeViewClient" DataSourceID="XmlDataSource1" Width="100%"
                ShowTreeLines="false" ShowExpandButtons="true"  OnCheckedChanged="NavigationTreeView_CheckedChanged1"   OnNodeDataBound="NavigationTreeView_NodeDataBound" OnPreRender="NavigationTreeView_PreRender" EnableHotTrack="false"
                TextField="Title" NavigateUrlField="Url" Theme="Moderno">
                <Styles>
                    <Node CssClass="node" />
                    <Elbow CssClass="elbow" />
                </Styles>
            </dx:ASPxTreeView>
             <script>

                 function breadCrumbs() {
                     window.NavControl.onNavigationBreadCrumbsButtonClick();

                 }

                 function initializeTree(s, e) {
                     window.tree = s;
                    

                     InitializeAll();
                 }
       
                 function initializeButton(s, e) {
                     window.button = s;
               

                 }



                 function InitializeAll() {
                 
                     window.NavControl = new Site.Nav.NavigationControl(window.tree, window.button, 'NavControl', 'breadCrumbsText');
                     window.NavControl.Init();
                 }
               

             </script>
           
          
          
        </div>
    </div>
   
    

    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Navigation.xml" XPath="/namespace/*" />
</nav>
