﻿<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Dash.Admin" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">


         <script type="text/c#" runat="server">

             [System.Web.Services.WebMethod(EnableSession = true)]
             public static bool test(string InitialCatalog, string DataSource, string UserID, string Password)
             {
                 var result = Dash.HelperClasses.CheckConnection.TestConnection(InitialCatalog, DataSource, UserID, Password);
                 if(result)
                 {
                     return true;
                 }

                 else
                 {
                     return false;
                 }
             }

    </script>

    <script>
        $(document).ready(function () {
            $('#company').on('click', function () {
                // Make an AJAX call to set the session variable to false
                $.ajax({
                    type: "POST",
                    url: "Admin.aspx/CreatingCompanySessionEdit", // Update with your actual page name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        return;
                    },
                    error: function (xhr, status, error) {
                        location.reload();
                    }
                });
            });
        });

        function testConnection() {

            var InitialCatalog = document.getElementById("dbNameInstance").value;
            var DataSource = document.getElementById("dbDataSource").value;
            var UserID = document.getElementById("dbUser").value;
            var Password = document.getElementById("dbPassword").value;

            $.ajax({
                type: 'POST',
                url: '<%= ResolveUrl("~/Admin.aspx/test") %>',
                 data: JSON.stringify({ InitialCatalog: InitialCatalog, DataSource: DataSource, UserID: UserID, Password: Password   }),
                 contentType: 'application/json; charset=utf-8',
                 dataType: 'json',
                 success: function (msg) {
                     if (msg.d) {
                         notify(false, "Uspešna konekcija.");
                     } else {
                         notify(true, "Napaka v konekciji.");
                     }
                 }
             });
        }

        /**
         *  is Error is boolean showing whether or not swall is an error notification or not. The message is the string to be shown in the message body.
         * @param isError
         * @param message
         */
        function notify(isError, message) {
            if (isError) {
                Swal.fire(
                    'Napaka',
                    message,
                    'error'
                )
            } else {
                Swal.fire(
                    'Uspeh!',
                    message,
                    'success'
                )
            }
        }

        function checkFields(company_name, website, company_number) {

            company_name = document.getElementById("companyName").value
            website = document.getElementById("website").value
            company_number = document.getElementById("companyNumber").value

            if (company_name == "" || website == "" || company_number == "") {
               notify(true, "Podatki manjkajo.")
            } else {
                if (isNaN(company_number)) {
                    notify(true, "Številka ni v pravi obliki.")
                } else {
                    btnRegistration = document.getElementById('<%=companyButton.ClientID %>');
                    btnRegistration.click();
                }

            }
        }

        function showDialogSync() {

            $("#userForm").css('display', 'flex');

            var elem = document.getElementById("userForm");

            setTimeout(function () {
                elem.style.opacity = 1;
               // document.getElementById('overlay').style.backgroundColor = "gray";

            }, 100);

        }

     
        function showDialogSyncCompany() {
            $('#companyModal').modal('show');

        }



        function showDialogSyncUser() {
            $('#userFormModal').modal('show');

        }
    
     


        function OnInitSpecific(s, e, gridName) {

            AdjustSize(gridName);

            if (gridName == "company") {
                document.getElementById("gridContainerCompanies").style.visibility = "";
            } else if (gridName == "user") {
                document.getElementById("gridContainerUser").style.visibility = "";
            } else if (gridName == "dashboard") {
                document.getElementById("gridContainerDashboard").style.visibility = "";
            }

        }

        function OnEndCallback(s, e, gridName) {
            AdjustSize(gridName);
        }

        function AdjustSize(gridName) {

            var height = Math.max(0, document.documentElement.clientHeight - (0.25 * document.documentElement.clientHeight)); 

            if (gridName == "company") {
                companyGrid.SetHeight(height);
            } else if (gridName == "user") {
                userGrid.SetHeight(height);
            } else if (gridName == "dashboard") {
                dashboardGrid.SetHeight(height);
            }

        }


        function filterGrid(category) {

            if (category === 'all') {

                dashboardGrid.PerformCallback(""); 
            } else {

                dashboardGrid.PerformCallback("filter|" + category); 
            }

        }

        window.addEventListener('resize', function () {
            location.reload();
        });

        function Type_Lookup_ValueChanged(s, e) {
            dashboardGrid.PerformCallback("FilterByType");
        }

        function Company_Lookup_ValueChanged(s, e) {
            dashboardGrid.PerformCallback("FilterByCompany");
        }

        function Language_Lookup_ValueChanged(s, e) {
            dashboardGrid.PerformCallback("FilterByLanguage");
        }
        function onCustomButtonClick(s, e) {
            if (e.buttonID === 'EditBtn') {
                var key = s.GetRowKey(e.visibleIndex);
                s.PerformCallback(key);
            }
        }
        function groupChanged(s, e) {

            // Save batch changes
            userGrid.UpdateEdit(); // Calls SaveBatchChanges on the server side
        }

    </script>


	


    <div class="content-flex">
        <div class="inner-item companies">
		    <asp:SqlDataSource ID="companiesGrid" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT id_company, company_name, database_name FROM companies"></asp:SqlDataSource>
            <div class="control_obj">
             <div class="grid-container full-height">
                 <div id="gridContainerCompanies" style="visibility: hidden">

            <dx:BootstrapGridView ID="companiesGridView" SettingsPopup-EditForm-AllowResize="false"  SettingsBehavior-AllowDragDrop="false" SettingsResizing-ColumnResizeMode="NextColumn" ClientInstanceName="companyGrid" Settings-VerticalScrollableHeight="400"  runat="server" SettingsEditing-Mode="PopupEditForm" KeyFieldName="id_company" Settings-VerticalScrollBarMode="Visible" DataSourceID="companiesGrid" Width="70%" CssClasses-Control="control" AutoGenerateColumns="False">
                <CssClassesEditor NullText="Urejaj"></CssClassesEditor>
                    <ClientSideEvents Init="function(s, e) { OnInitSpecific(s, e, 'company'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'company'); }" />

              <Settings VerticalScrollBarMode="Visible" />
             <SettingsPager  Mode="ShowAllRecords" PageSize="15" Visible="False">
             </SettingsPager>
            <SettingsText HeaderFilterCancelButton="Prekliči" CommandUpdate="Posodobi" CommandCancel="Zapri" CommandEdit=" " SearchPanelEditorNullText="Poiščite podjetje"></SettingsText>

                <SettingsEditing Mode="PopupEditForm"></SettingsEditing>

                   <SettingsDataSecurity AllowEdit="True" />
                <Columns>

                    <dx:BootstrapGridViewCommandColumn  HeaderBadge-IconCssClass="fa fa-caret-down"  ShowEditButton="True" VisibleIndex="0" Caption=" " >
      
                    </dx:BootstrapGridViewCommandColumn>

                    <dx:BootstrapGridViewTextColumn FieldName="id_company" Visible="False" ReadOnly="True" VisibleIndex="1">
                    </dx:BootstrapGridViewTextColumn>
                    <dx:BootstrapGridViewTextColumn FieldName="company_name" Caption="Podjetje" VisibleIndex="2">
                    </dx:BootstrapGridViewTextColumn>
                    <dx:BootstrapGridViewTextColumn FieldName="database_name" Visible="False" Caption="Naziv konekcije" VisibleIndex="3">
                    </dx:BootstrapGridViewTextColumn>                    
                </Columns>



                    <SettingsSearchPanel Visible="True" />
                    <CssClasses Control="control" />


 
            



                    </dx:BootstrapGridView>
                     </div>
	            </div>
	        </div>




            <div class="action-buttons">
                 <button type="button" class="btn btn-primary actionButton" id="company" data-toggle="modal" data-target="#companyModal">
                    <i class="fas fa-plus"></i>
                </button>
                 <dx:BootstrapButton runat="server" Badge-IconCssClass="fas fa-trash" ID ="deleteCompany" UseSubmitBehavior="False" CssClasses-Control="actionButton" OnClick="DeleteCompany_Click" Text="">
                     <SettingsBootstrap RenderOption="Danger" />
                 </dx:BootstrapButton>
            </div>




           </div>


        <div class="inner-item user">
            <div class="control_obj">
            <div id="gridContainerUser" style="visibility: hidden">

 <asp:SqlDataSource 
    ID="usersGrid" runat="server" 
    ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" 
    SelectCommand="SELECT * FROM [users_groups];"
    UpdateCommand="UPDATE users SET group_id = @group WHERE uname = @uname">
    
    <UpdateParameters>
        <asp:Parameter Name="group" Type="Int32" />
        <asp:Parameter Name="uname" Type="String" />
    </UpdateParameters>
</asp:SqlDataSource>



            <dx:BootstrapGridView ID="usersGridView" SettingsPopup-EditForm-AllowResize="false" SettingsBehavior-AllowDragDrop="false" SettingsResizing-ColumnResizeMode="NextColumn"  DataSourceID="usersGrid" ClientInstanceName="userGrid" Settings-VerticalScrollableHeight="400"  AutoPostBack="false" runat="server" Settings-VerticalScrollBarMode="Visible"  Width="80%" AutoGenerateColumns="False"  KeyFieldName="uname"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj" CssClasses-Control="grid">
<CssClasses Control="grid"></CssClasses>
                              <SettingsEditing Mode="Batch" />

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>
                    <ClientSideEvents CustomButtonClick="function(s, e) { onCustomButtonClick(s, e); }" Init="function(s, e) { OnInitSpecific(s, e, 'user'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'user'); }" />

              <Settings  VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="15" Visible="False">
          </SettingsPager>

<SettingsText HeaderFilterCancelButton="Prekliči" CommandUpdate="Posodobi" HeaderFilterSelectAll ="Izberi vse" CommandCancel="Zapri" CommandEdit="Uredi" SearchPanelEditorNullText="Poiščite uporabnika"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
                <Templates>
          <StatusBar>
          </StatusBar>
</Templates>


          <Columns>
              <dx:BootstrapGridViewCommandColumn HeaderBadge-IconCssClass="fa fa-caret-down"   SelectAllCheckboxMode="Page" ShowSelectCheckbox="false" VisibleIndex="0" ShowEditButton="False" Caption=" ">
                    <CustomButtons >
                    <dx:BootstrapGridViewCommandColumnCustomButton IconCssClass="fas fa-edit"  Visibility="AllDataRows"  ID="EditBtn" Text="" CssClass="custom-edit-btn" />
                </CustomButtons>

              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="uname" Visible="true" Name="uname" ReadOnly="false" VisibleIndex="1" Caption="Uporabniško ime">
              <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="password"  Visible="false" Name="password" VisibleIndex="2" Caption="Password">
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="user_role" Visible="false" Name="user_role" VisibleIndex="3" Caption="UserRole">
              </dx:BootstrapGridViewTextColumn>
			   <dx:BootstrapGridViewTextColumn FieldName="view_allowed" Visible="false" Name="view_allowed" VisibleIndex="3" Caption="ViewState">
              </dx:BootstrapGridViewTextColumn>
			   <dx:BootstrapGridViewTextColumn FieldName="email" Visible="false" Name="email" VisibleIndex="3" Caption="Email">
              </dx:BootstrapGridViewTextColumn>

                <dx:BootstrapGridViewComboBoxColumn SettingsHeaderFilter-ListBoxSearchUISettings-EditorNullText="Iskanje" SettingsHeaderFilter-Mode="CheckedList"  FieldName="group_name" Name="group" VisibleIndex="3" Caption="Skupina">
                     <PropertiesComboBox ClientSideEvents-SelectedIndexChanged="groupChanged"  TextField="group_name" ValueField="group" EnableSynchronization="False"
                        IncrementalFilteringMode="StartsWith"  DataSourceID="GroupsDropdown">
                     </PropertiesComboBox>
             </dx:BootstrapGridViewComboBoxColumn>
       
          </Columns>
          <SettingsSearchPanel Visible="True" />
      </dx:BootstrapGridView>
                </div>
                </div>

    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />

                <div class="action-buttons">
                    <asp:HiddenField ID="IsInitialLoad" runat="server" Value="true" />
                    <button type="button" runat="server" onserverclick="NewUser_Click" id="new_user" class="btn btn-primary actionButton">
                    <i class="fas fa-plus"></i>
                    </button>
                    <dx:BootstrapButton Badge-IconCssClass="fas fa-trash" runat="server" ID="deleteUser" UseSubmitBehavior="False"  Text="" OnClick="DeleteUser_Click" CssClasses-Control="actionButton">
                    <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>
                </div>
		                        

   </div>

	   <div class="inner-item graphs">
           <div class="control_obj">
                                <div id="gridContainerDashboard" style="visibility: hidden">
<asp:ScriptManager runat="server" />


      <dx:BootstrapGridView SettingsBehavior-AllowDragDrop="false" SettingsText-CommandBatchEditPreviewChanges="Preveri spremembe" SettingsText-CommandBatchEditCancel="Prekliči" SettingsText-CommandBatchEditUpdate="Posodobi"   SettingsPopup-EditForm-AllowResize="false"  CssClasses-FocusedRow="focused_row" SettingsResizing-ColumnResizeMode="NextColumn" AutoPostBack="false" OnBeforeHeaderFilterFillItems="graphsGridView_BeforeHeaderFilterFillItems" ID="graphsGridView" Settings-ShowHeaderFilterButton="true" runat="server" ClientInstanceName="dashboardGrid" Settings-VerticalScrollableHeight="400"  AutoGenerateColumns="False" Settings-VerticalScrollBarMode="Visible"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj"  Width="100%" DataSourceID="query" KeyFieldName="id" >
<CssClasses Control="grid"></CssClasses>
                              <SettingsEditing Mode="Batch" />

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>
                    <ClientSideEvents Init="function(s, e) { OnInitSpecific(s, e, 'dashboard'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'dashboard'); }" />

              <Settings VerticalScrollBarMode="Visible" ShowFilterRow="false"/>
  
          <SettingsPager Mode="ShowAllRecords" PageSize="15" Visible="true">
          </SettingsPager>

<SettingsText HeaderFilterCancelButton="Prekliči" CommandUpdate="Posodobi" CommandCancel="Zapri" HeaderFilterSelectAll ="Izberi vse" CommandEdit="Uredi" SearchPanelEditorNullText="Poiščite graf"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
          <Columns>
              <dx:BootstrapGridViewCommandColumn HeaderBadge-IconCssClass="fa fa-caret-down"  SelectAllCheckboxMode="Page" ShowSelectCheckbox="True"  VisibleIndex="0" ShowEditButton="False" Caption=" ">
                              

              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="id" Settings-AllowHeaderFilter="False"  Visible="false" ReadOnly="True" VisibleIndex="1">

              <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>

              <dx:BootstrapGridViewTextColumn ExportCellStyle-BackColor="Black" FieldName="caption"  Settings-AllowHeaderFilter="False" Name="Graf" VisibleIndex="2" Caption="Naziv">
              </dx:BootstrapGridViewTextColumn>

              <dx:BootstrapGridViewTextColumn FieldName="custom_name" Settings-AllowHeaderFilter="False" Name="Podjetje" VisibleIndex="3" Caption="Interni naziv">
              </dx:BootstrapGridViewTextColumn>


              <dx:BootstrapGridViewComboBoxColumn SettingsHeaderFilter-ListBoxSearchUISettings-EditorNullText="Iskanje" SettingsHeaderFilter-DateRangeCalendarSettings-FastNavProperties-CancelButtonText="Prekliči"  SettingsHeaderFilter-Mode="CheckedList"  FieldName="meta_type" Name="Tip" VisibleIndex="3" Caption="Tip">
                      <PropertiesComboBox TextField="description" ValueField="id" EnableSynchronization="False"
                        IncrementalFilteringMode="StartsWith" AllowNull="true" DataSourceID="TypeFilterDataSource" >
                      </PropertiesComboBox>
              </dx:BootstrapGridViewComboBoxColumn>

              <dx:BootstrapGridViewComboBoxColumn SettingsHeaderFilter-ListBoxSearchUISettings-EditorNullText="Iskanje"  SettingsHeaderFilter-DateRangeCalendarSettings-FastNavProperties-CancelButtonText="Prekliči"  SettingsHeaderFilter-Mode="CheckedList"  FieldName="meta_company" Name="Podjetje" VisibleIndex="3" Caption="Podjetje">
                      <PropertiesComboBox TextField="description" ValueField="id" EnableSynchronization="False"
                        IncrementalFilteringMode="StartsWith" AllowNull="true"  DataSourceID="CompanyFilterDataSource">
                      </PropertiesComboBox>
              </dx:BootstrapGridViewComboBoxColumn>

              <dx:BootstrapGridViewComboBoxColumn SettingsHeaderFilter-ListBoxSearchUISettings-EditorNullText="Iskanje" SettingsHeaderFilter-DateRangeCalendarSettings-FastNavProperties-CancelButtonText="Prekliči"  SettingsHeaderFilter-Mode="CheckedList"  FieldName="meta_language" Name="Jezik" VisibleIndex="3" Caption="Jezik">
                     <PropertiesComboBox  TextField="description" ValueField="id" EnableSynchronization="False"
                        IncrementalFilteringMode="StartsWith" AllowNull="true"  DataSourceID="LanguageFilterDataSource">
                     </PropertiesComboBox>
             </dx:BootstrapGridViewComboBoxColumn>


          </Columns>
          <SettingsSearchPanel Visible="True" />

      </dx:BootstrapGridView>


       </div>
       </div>

        <asp:SqlDataSource ID="TypeFilterDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>"
            SelectCommand=
            "
                SELECT * FROM meta_options WHERE option_type = 'type'
                UNION ALL
                SELECT NULL AS id, 'type', '', 'Brez tipa'
                ORDER BY id ASC;
            ">
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="CompanyFilterDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>"
            SelectCommand=
            "
            SELECT * FROM meta_options WHERE option_type = 'company'
            UNION ALL SELECT NULL, 'company', '', 'Brez podjetja'
            ORDER BY id ASC;
            "
            >
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="LanguageFilterDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>"
            SelectCommand=
            "
            SELECT * FROM meta_options WHERE option_type = 'language'
             UNION ALL SELECT NULL, '', '', 'Brez' ORDER BY id ASC;"
            >
        </asp:SqlDataSource>

            <asp:SqlDataSource ID="GroupsDropdown" runat="server" 
             ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>"
             SelectCommand=
                "
                      SELECT group_id AS [group], group_name 
                      FROM groups WHERE company_id = @company
                      UNION ALL
                      SELECT NULL AS [group], 'Brez skupine' AS group_name
                      ORDER BY group_id ASC;
                "
            >

            <SelectParameters>
                  <asp:Parameter Name="company" Type="Int32" Direction="Input" />       
            </SelectParameters>
         </asp:SqlDataSource>

<asp:SqlDataSource
    EnableCaching="false"
    ID="query" 
    runat="server" 
    ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" 
    SelectCommand="sp_get_dashboards" 
    SelectCommandType="StoredProcedure"
    UpdateCommand="sp_upsert_dashboards_custom_names"
    UpdateCommandType="StoredProcedure"
    >
    
    <SelectParameters>
        <asp:Parameter Name="uname" Type="String" Direction="Input" />
        <asp:Parameter Name="company" Type="Int32" Direction="Input" />       
    </SelectParameters>
    
    <UpdateParameters>
        <asp:Parameter Name="dashboard_id" Type="Int32" />
        <asp:Parameter Name="company_id" Type="Int32" />
        <asp:Parameter Name="custom_name" Type="String" />
        <asp:Parameter Name="meta_type"   ConvertEmptyStringToNull="true" Type="Int32" />
        <asp:Parameter Name="meta_company" ConvertEmptyStringToNull="true" Type="Int32" />
        <asp:Parameter Name="meta_language" ConvertEmptyStringToNull="true" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>




           <div class="action-buttons">
                 <dx:BootstrapButton runat="server" CssClasses-Icon="fas fa-save" Text="" ID="saveGraphs" OnClick="SaveGraphs_Click" CssClasses-Control="actionButton" AutoPostBack="false">
                    <SettingsBootstrap RenderOption="Primary" />
                  </dx:BootstrapButton>



          <div class="toolbar-custom">

              <div class="special_buttons">

                    <dx:BootstrapButton CssClasses-Icon="fas fa-arrow-up" runat="server" Text="" ID="MoveUpButton" OnClick="MoveUpButton_Click"  AutoPostBack="false">
                     <SettingsBootstrap RenderOption="Primary" />
                   </dx:BootstrapButton>
              
              
                  <dx:BootstrapButton CssClasses-Icon="fas fa-arrow-down" runat="server" Text="" ID="MoveDownButton" OnClick="MoveDownButton_Click" AutoPostBack="false">
                     <SettingsBootstrap RenderOption="Primary" />
                   </dx:BootstrapButton>
                  
              </div>

        </div>


           </div>


         <br />
    <SettingsBootstrap RenderOption="Primary" />
           </div>
        </div>





	<section class="columns">
<!-- Bootstrap Modal Structure -->
<div class="modal fade" id="companyModal" tabindex="-1" role="dialog" aria-labelledby="companyModalLabel" aria-hidden="true">
  <div class="modal-dialog modal-lg" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="companyModalLabel"></h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <!-- Form starts here -->
          <div class="container">
            <div class="row">
              <div class="col-md-6">
                <!-- Company Part -->
                <div class="companyPart">
                  <div class="form-group">
                    <label for="companyName">Naziv podjetja</label>
                    <asp:TextBox ID="companyName" runat="server" placeholder="Ime" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="companyNumber">Kontaktna številka</label>
                    <asp:TextBox ID="companyNumber" runat="server" placeholder="Kontaktna številka" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="website">Spletna stran</label>
                    <asp:TextBox ID="website" runat="server" placeholder="Spletna stran" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>                  
                </div>
              </div>
              <div class="col-md-6">
                <!-- Connection Part -->
                <div class="connectionPart">
                  <div class="form-group">
                    <label for="dbDataSource">Data source</label>
                    <asp:TextBox ID="dbDataSource" runat="server" placeholder="Data source" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="dbUser">Uporabnik</label>
                    <asp:TextBox ID="dbUser" runat="server" placeholder="Uporabnik" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="dbPassword">Geslo</label>
                    <asp:TextBox ID="dbPassword" runat="server" TextMode="Password" placeholder="Geslo" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="dbNameInstance">Naziv baze</label>
                    <asp:TextBox ID="dbNameInstance" runat="server" placeholder="Ime" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="connName">Naziv povezave</label>
                    <asp:TextBox ID="connName" runat="server" placeholder="Ime" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                  </div>
                </div>
              </div>
            </div>
          </div>
        <!-- End of form -->
      </div>
      <div class="modal-footer">
        <asp:Button CssClass="btn btn-primary" ID="companyButton" ClientIDMode="AutoID" Enabled="true" style="display:none" runat="server" Text="Potrdi" OnClick="CompanyButton_Click" />
        <button type="button" class="btn btn-info" onclick="testConnection(); return false;" id="test">Testiraj</button>
        <button type="button" class="btn btn-primary" onclick="checkFields(); return false;" id="testing">Potrdi</button>
      </div>
    </div>
  </div>
</div>

<div class="modal fade" id="userFormModal" tabindex="-1" role="dialog" aria-labelledby="userFormModalLabel" aria-hidden="true">
  <div class="modal-dialog modal-lg" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="userFormModalLabel"></h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
          <div class="container">
            <div class="row">
              <div class="col-md-6">

                <!-- Hidden input for username -->
                <input type="text" name="Username" style="display:none;" autocomplete="on">

                <!-- Hidden input for password -->
                <input type="password" name="Password" style="display:none;" autocomplete="on">

                <div class="form-group">
                  <label for="TxtName">Ime in Priimek</label>
                  <asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="email">Email</label>
                  <asp:TextBox ID="email" runat="server" placeholder="Email" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="TxtUserName">Uporabniško ime</label>
                  <asp:TextBox  ID="TxtUserName" runat="server" placeholder="Uporabniško ime" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="referer">Komercialist</label>
                  <asp:TextBox ID="referrer" runat="server" placeholder="Komercialist" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="TxtPassword">Geslo</label>
                  <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" placeholder="Geslo" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="TxtRePassword">Ponovite geslo</label>
                  <asp:TextBox ID="TxtRePassword" runat="server" TextMode="Password" placeholder="Geslo še enkrat" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
              <div class="col-md-6">
                <!-- User Role and Rights -->
                <h5>Vloga uporabnika</h5>
                <div class="form-group">
                  <asp:RadioButtonList ID="userRole" runat="server" RepeatDirection="Horizontal" CssClass="form-check">
                    <asp:ListItem>Admin</asp:ListItem>
                    <asp:ListItem>User</asp:ListItem>
                  </asp:RadioButtonList>
                </div>
                <h5>Pravice uporabnika</h5>
                <div class="form-group">
                  <asp:DropDownList ID="userTypeList" runat="server" CssClass="form-control">
                    <asp:ListItem>Viewer</asp:ListItem>
                    <asp:ListItem>Viewer & Designer</asp:ListItem>
                  </asp:DropDownList>
                </div>
               
              </div>
            </div>
          </div>

        <!-- End of form -->
      </div>
      <div class="modal-footer">
        <asp:Button CssClass="btn btn-primary" ID="registrationButton" runat="server" Text="Potrdi" OnClick="RegistrationButton_Click" />
        <button type="button" class="btn btn-danger" id="closeUser" data-dismiss="modal">Zapri</button>
      </div>
    </div>
  </div>
</div>







</section>
  

<script>

    $("#newUser").click(function (e) {

        e.preventDefault();

    })

    function user() {

        var userForm = $("#userForm");
        userForm.show();
    }

    function company() {

        var userForm = $("#companyForm");
        userForm.css('display', 'flex');

    }

    $(document).ready(function () {
        $("#user").click(function () {
            $("#userForm").css('display', 'flex');

            var elem = document.getElementById("userForm");

            setTimeout(function () {
                elem.style.opacity = 1;
                //document.getElementById('overlay').style.backgroundColor = "gray";

            }, 100);

        });
    });
    function setCookie(name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }
    function getCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    $(document).ready(function () {
        $("#company").click(function () {

            document.getElementById("companyName").value = "";
            document.getElementById("companyNumber").value = "";
            document.getElementById("website").value = "";
            document.getElementById("dbDataSource").value = "";
            document.getElementById("dbUser").value = "";
            document.getElementById("dbPassword").value = "";
            document.getElementById("dbNameInstance").value = "";
            document.getElementById("connName").value = "";
            setCookie("Edit", "no", 365);
            $("#companyForm").css('display', 'flex');
            var x = document.getElementsByTagName("BODY")[0]
            var elem = document.getElementById("companyForm");

            setTimeout(function () {
                elem.style.opacity = 1;
                // document.getElementById('overlay').style.backgroundColor = "gray";
            }, 100);
        });
    });

    $(document).ready(function () {
        $("#closeCompany").click(function () {
            var elem = document.getElementById("companyForm");

            setTimeout(function () {
                elem.style.opacity = 0;
            }, 100);

            $("#companyForm").css('display', 'none');

        });
    });

    $(document).ready(function () {
        $("#closeUser").click(function () {

            var elem = document.getElementById("userForm");

            setTimeout(function () {
                elem.style.opacity = 0;
            }, 100);

            $("#userForm").css('display', 'none');
        });
    });
</script>

</asp:Content>