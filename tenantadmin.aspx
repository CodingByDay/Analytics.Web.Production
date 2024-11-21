<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TenantAdmin.aspx.cs" Inherits="Dash.TenantAdmin" %>
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

        function testConnection() {

            var InitialCatalog = document.getElementById("dbNameInstance").value;
            var DataSource = document.getElementById("dbDataSource").value;
            var UserID = document.getElementById("dbUser").value;
            var Password = document.getElementById("dbPassword").value;

            $.ajax({
                type: 'POST',
                url: '<%= ResolveUrl("~/Admin.aspx/test") %>',
                data: JSON.stringify({ InitialCatalog: InitialCatalog, DataSource: DataSource, UserID: UserID, Password: Password }),
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

        function groupChanged(s, e) {

            // Save batch changes
            userGrid.UpdateEdit(); // Calls SaveBatchChanges on the server side
        }


        function onCustomButtonClick(s, e) {
            if (e.buttonID === 'EditBtn') {
                var key = s.GetRowKey(e.visibleIndex);
                s.PerformCallback(key);
            }
        }
        function OnBatchEditEndEditing(s, e) {
            setTimeout(function () {
                if (s.batchEditApi.HasChanges()) {
                    dashboardGrid.UpdateEdit();
                }
            }, 1000);
        }
    </script>


	


    <div class="content-flex">








     


        <div class="inner-item user">
            <div class="control_obj">
            <div id="gridContainerUser" style="visibility: hidden">


         <asp:SqlDataSource 
            ID="usersGrid" runat="server" 
            ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" 
            SelectCommand="SELECT * FROM [users_groups] WHERE id_company = @company_id;"
            UpdateCommand="UPDATE users SET group_id = @group WHERE uname = @uname">
            <SelectParameters>
                 <asp:Parameter Name="company_id" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="group" Type="Int32" />
                <asp:Parameter Name="uname" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>         
                
                <dx:BootstrapGridView ID="usersGridView" SettingsResizing-ColumnResizeMode="NextColumn"  DataSourceID="usersGrid" ClientInstanceName="userGrid" Settings-VerticalScrollableHeight="400"  AutoPostBack="false" runat="server" Settings-VerticalScrollBarMode="Visible"  Width="70%" AutoGenerateColumns="False"  SettingsEditing-Mode="PopupEditForm" KeyFieldName="uname"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj" CssClasses-Control="grid">
<CssClasses Control="grid"></CssClasses>
                              <SettingsEditing Mode="Batch" />

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>
                    <ClientSideEvents CustomButtonClick="function(s, e) { onCustomButtonClick(s, e); }" Init="function(s, e) { OnInitSpecific(s, e, 'user'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'user'); }" />

              <Settings VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="15" Visible="False">
          </SettingsPager>

<SettingsText HeaderFilterCancelButton="Prekliči" HeaderFilterSelectAll ="Izberi vse" CommandUpdate="Posodobi" CommandCancel="Zapri" CommandEdit="Uredi" SearchPanelEditorNullText="Poiščite uporabnika"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
             <Templates>
          <StatusBar>
          </StatusBar>
</Templates>
          <Columns>
              <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="false" VisibleIndex="0" ShowEditButton="False" Caption=" " HeaderBadge-IconCssClass="fa fa-caret-down" >
                        <CustomButtons>
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
                <PropertiesComboBox ClientSideEvents-SelectedIndexChanged="groupChanged" TextField="group_name" ValueField="group" EnableSynchronization="False"
                   IncrementalFilteringMode="StartsWith" DataSourceID="GroupsDropdown">
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
                    <button type="button"  runat="server" onserverclick="NewUser_Click" id="new_user" class="btn btn-primary actionButton">
                           <asp:Literal runat="server" Text="<%$ Resources:Resource, Add%>" /><i class="fas fa-plus"></i>
                    </button>
                    <dx:BootstrapButton CssClasses-Icon="fas fa-trash" runat="server" ID="deleteUser" UseSubmitBehavior="False"  Text="Pobriši" OnClick="DeleteUser_Click" CssClasses-Control="actionButton">
                    <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>
                </div>
		                        

   </div>

	   <div class="inner-item graphs">
           <div class="control_obj">
                                <div id="gridContainerDashboard" style="visibility: hidden">
<asp:ScriptManager runat="server" />


      <dx:BootstrapGridView AutoPostBack="false" CssClasses-FocusedRow="focused_row" SettingsText-CommandBatchEditPreviewChanges="Preveri spremembe" SettingsText-CommandBatchEditCancel="Prekliči" SettingsText-CommandBatchEditUpdate="Posodobi"  SettingsResizing-ColumnResizeMode="NextColumn" OnBeforeHeaderFilterFillItems="graphsGridView_BeforeHeaderFilterFillItems" Settings-ShowHeaderFilterButton="true"  SettingsBehavior-AllowDragDrop="true" ID="graphsGridView" runat="server" ClientInstanceName="dashboardGrid" Settings-VerticalScrollableHeight="400"  AutoGenerateColumns="False" Settings-VerticalScrollBarMode="Visible"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj"  Width="100%" DataSourceID="query" KeyFieldName="id" CssClasses-Control="graph">
<CssClasses Control="grid"></CssClasses>
<SettingsEditing Mode="Batch" />

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>
                    <ClientSideEvents BatchEditEndEditing="OnBatchEditEndEditing"  Init="function(s, e) { OnInitSpecific(s, e, 'dashboard'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'dashboard'); }" />

              <Settings VerticalScrollBarMode="Visible" ShowFilterRow="false"/>
   <Toolbars>
      

    </Toolbars>
          <SettingsPager Mode="ShowAllRecords" PageSize="15" Visible="true">
          </SettingsPager>

<SettingsText HeaderFilterCancelButton="Prekliči" HeaderFilterSelectAll ="Izberi vse" CommandEdit="Uredi" CommandUpdate="Posodobi" CommandCancel="Zapri" SearchPanelEditorNullText="Poiščite graf"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
          <Columns>

              <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="True" VisibleIndex="0" ShowEditButton="False" Caption=" " HeaderBadge-IconCssClass="fa fa-caret-down" >

              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="id" Settings-AllowHeaderFilter="False"   Visible="false" ReadOnly="True" VisibleIndex="1">
                  <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn ReadOnly="true" FieldName="caption" Settings-AllowHeaderFilter="False"   Name="Graf" VisibleIndex="2" Caption="Naziv">
              </dx:BootstrapGridViewTextColumn>
                <dx:BootstrapGridViewTextColumn FieldName="custom_name" Settings-AllowHeaderFilter="False"  Name="Podjetje" VisibleIndex="3" Caption="Interni naziv">
                </dx:BootstrapGridViewTextColumn>


               <dx:BootstrapGridViewTextColumn ReadOnly="true" SettingsHeaderFilter-ListBoxSearchUISettings-EditorNullText="Iskanje" SettingsHeaderFilter-DateRangeCalendarSettings-FastNavProperties-CancelButtonText="Prekliči"  SettingsHeaderFilter-Mode="CheckedList"  FieldName="meta_type" Name="Tip" VisibleIndex="3" Caption="Tip">
                    <SettingsHeaderFilter>
                     <ListBoxSearchUISettings  EditorNullText='<%$ Resources:Resource, Search %>' />
                 </SettingsHeaderFilter>
             </dx:BootstrapGridViewTextColumn>


             <dx:BootstrapGridViewTextColumn ReadOnly="true" SettingsHeaderFilter-ListBoxSearchUISettings-EditorNullText="Iskanje" SettingsHeaderFilter-DateRangeCalendarSettings-FastNavProperties-CancelButtonText="Prekliči"  SettingsHeaderFilter-Mode="CheckedList"  FieldName="meta_language" Name="Jezik" VisibleIndex="3" Caption="Jezik">
                  <SettingsHeaderFilter>
                     <ListBoxSearchUISettings  EditorNullText='<%$ Resources:Resource, Search %>' />
                 </SettingsHeaderFilter>
            </dx:BootstrapGridViewTextColumn>
          </Columns>
          <SettingsSearchPanel Visible="True" />

                          <Templates>
          <StatusBar>
          </StatusBar>
</Templates>
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
        <asp:Parameter Name="company" Type="Int32" Direction="Input" />       
    </SelectParameters>
    
    <UpdateParameters>
        <asp:Parameter Name="dashboard_id" Type="Int32" />
        <asp:Parameter Name="company_id" Type="Int32" />
        <asp:Parameter Name="custom_name" Type="String" />
        <asp:Parameter Name="meta_type"   ConvertEmptyStringToNull="true" Type="Int32" />
        <asp:Parameter Name="meta_language" ConvertEmptyStringToNull="true" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>




           <div class="action-buttons">
                 <dx:BootstrapButton runat="server" CssClasses-Icon="fas fa-save" Text="Shrani" ID="saveGraphs" OnClick="SaveGraphs_Click" CssClasses-Control="actionButton" AutoPostBack="false">
                    <SettingsBootstrap RenderOption="Primary" />
                  </dx:BootstrapButton>





           </div>


         <br />
    <SettingsBootstrap RenderOption="Primary" />
           </div>
        </div>





	<section class="columns">
<!-- Bootstrap Modal Structure -->




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
                  <label for="TxtName"><asp:Literal runat="server" Text="<%$ Resources:Resource, Username%>" /></label>
                  <asp:TextBox ID="TxtName" runat="server" placeholder="<%$ Resources:Resource, Username%>" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="email"><asp:Literal runat="server" Text="<%$ Resources:Resource, Email%>" /></label>
                  <asp:TextBox ID="email" runat="server" placeholder="<%$ Resources:Resource, Email%>" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="TxtUserName"><asp:Literal runat="server" Text="<%$ Resources:Resource, Username%>" /></label>
                  <asp:TextBox ID="TxtUserName" runat="server" placeholder="<%$ Resources:Resource, Username%>" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="referer"><asp:Literal runat="server" Text="<%$ Resources:Resource, Commercialist%>" /></label>
                  <asp:TextBox ID="referrer" runat="server" placeholder="<%$ Resources:Resource, Commercialist%>" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="TxtPassword"><asp:Literal runat="server" Text="<%$ Resources:Resource, Password%>" /></label>
                  <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" placeholder="<%$ Resources:Resource, Password%>" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                  <label for="TxtRePassword"><asp:Literal runat="server" Text="<%$ Resources:Resource, RePassword%>" /></label>
                  <asp:TextBox ID="TxtRePassword" runat="server" TextMode="Password" placeholder="<%$ Resources:Resource, RePassword%>" CssClass="form-control"></asp:TextBox>
                </div>
              </div>
              <div class="col-md-6">
                <!-- User Role and Rights -->
                <h5><asp:Literal runat="server" Text="<%$ Resources:Resource, UserRole%>" /></h5>
                <div class="form-group">
                  <asp:RadioButtonList ID="userRole" runat="server" RepeatDirection="Horizontal" CssClass="form-check">
                    <asp:ListItem>Admin</asp:ListItem>
                    <asp:ListItem>User</asp:ListItem>
                  </asp:RadioButtonList>
                </div>
                <h5><asp:Literal runat="server" Text="<%$ Resources:Resource, UserRights%>" /></h5>
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
        <button type="button" class="btn btn-danger" id="closeUser" data-dismiss="modal"><asp:Literal runat="server" Text="<%$ Resources:Resource, Close%>" /></button>
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
                //document.getElementById('overlay').style.backgroundColor = "gray";
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

