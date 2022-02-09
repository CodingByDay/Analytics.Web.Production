<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="AdaptiveSlideNavigation.UserControls.Search" %>
<div class="search-wrapper">
    <dx:ASPxButtonEdit runat="server" ClientInstanceName="SearchEditor" CssClass="search-input" NullText="Search"
        NullTextDisplayMode="UnfocusedAndFocused" DataSourceID="XmlDataSource1">
        <NullTextStyle CssClass="null-text" />
        <ClearButton DisplayMode="Always" Image-SpriteProperties-CssClass="clear-button" />
        <ClientSideEvents GotFocus="Site.Search.OnSearchBoxGotFocus" LostFocus="Site.Search.OnSearchBoxLostFocus" KeyDown="Site.Search.OnSearchEditorKeyDown" />
    </dx:ASPxButtonEdit>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Navigation.xml" XPath="//page" />
    <dx:ASPxCallbackPanel runat="server" ID="searchResults" ClientInstanceName="searchResults" OnCallback="SearchResults_Callback"
        CssClass="search-results">
        <ClientSideEvents EndCallback="Site.Search.OnEndCallback" />
        <SettingsLoadingPanel Enabled="false" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxNavBar runat="server" ID="SearchResultsNavBar" ClientInstanceName="SearchResultsNavBar" AllowSelectItem="true"
                    ShowGroupHeaders="false" CssClass="search-results-nav-bar" Width="100%" EnableViewStat="false" EncodeHtml="false">
                    <ItemStyle CssClass="search-result-item" SelectedStyle-CssClass="search-result-item-selected"
                        HoverStyle-CssClass="search-result-item-hovered" />
                    <GroupContentStyle CssClass="search-result-group-content" />
                </dx:ASPxNavBar>
                <div id="noResultsContainer" runat="server" class="no-results-container">
                    No results found for <b runat="server" id="requestText"></b>. Try input 'Page', to test search.
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</div>
