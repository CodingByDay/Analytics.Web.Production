<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navigation.ascx.cs" Inherits="AdaptiveSlideNavigation.UserControls.Navigation" %>
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
            <dx:ASPxTreeView runat="server" ID="NavigationTreeView" ClientInstanceName="NavigationTreeView" DataSourceID="XmlDataSource1" Width="100%"
                ShowTreeLines="false" ShowExpandButtons="true" OnNodeDataBound="NavigationTreeView_NodeDataBound" OnPreRender="NavigationTreeView_PreRender" EnableHotTrack="false"
                TextField="Title" NavigateUrlField="Url">
                <Styles>
                    <Node CssClass="node" />
                    <Elbow CssClass="elbow" />
                </Styles>
            </dx:ASPxTreeView>
            <script>
                var NavControl = new Site.Nav.NavigationControl(NavigationTreeView, NavigationBreadCrumbsButton, 'NavControl', 'breadCrumbsText');
                NavControl.Init();
            </script>
        </div>
    </div>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Navigation.xml" XPath="/namespace/*" />
</nav>
