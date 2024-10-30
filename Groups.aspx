<%@ Page Title="Groups" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="Dash.Groups" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">


         <script type="text/c#" runat="server">

          

    </script>

    <script>
        window.addEventListener('resize', function () {
            location.reload();
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



        function showDialogSyncGroup() {
            $('#groupFormModal').modal('show');

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

            var height = Math.max(0, document.documentElement.clientHeight - (0.2 * document.documentElement.clientHeight)); // 10vh bottom margin 06.09.2024 Janko Jovičić

            if (gridName == "company") {
                companyGrid.SetHeight(height);
            } else if (gridName == "user") {
                groupsGrid.SetHeight(height);
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



    </script>


	


    <div class="content-flex">
        <div class="inner-item companies">
		    <asp:SqlDataSource ID="companiesGrid" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT id_company, company_name, database_name FROM companies"></asp:SqlDataSource>
            <div class="control_obj">
             <div class="grid-container full-height">
                 <div id="gridContainerCompanies" style="visibility: hidden">

            <dx:BootstrapGridView ID="companiesGridView" SettingsResizing-ColumnResizeMode="Control" ClientInstanceName="companyGrid" Settings-VerticalScrollableHeight="400"  runat="server"  KeyFieldName="id_company" Settings-VerticalScrollBarMode="Visible" DataSourceID="companiesGrid" Width="100%" CssClasses-Control="control" AutoGenerateColumns="False">
                <CssClassesEditor NullText="Urejaj"></CssClassesEditor>
                    <ClientSideEvents Init="function(s, e) { OnInitSpecific(s, e, 'company'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'company'); }" />

              <Settings VerticalScrollBarMode="Visible" />
             <SettingsPager  Mode="ShowAllRecords" PageSize="15" Visible="False">
             </SettingsPager>
            <SettingsText SearchPanelEditorNullText="Poiščite podjetje"></SettingsText>

                <SettingsEditing Mode="PopupEditForm"></SettingsEditing>

                   <SettingsDataSecurity AllowEdit="True" />
                <Columns>

                    <dx:BootstrapGridViewCommandColumn ShowEditButton="False" VisibleIndex="0" Caption="Actions">
      
                    </dx:BootstrapGridViewCommandColumn>

                    <dx:BootstrapGridViewTextColumn FieldName="id_company" Visible="False" ReadOnly="True" VisibleIndex="1">
                    </dx:BootstrapGridViewTextColumn>
                    <dx:BootstrapGridViewTextColumn FieldName="company_name" Caption="Podjetje" VisibleIndex="2">
                    </dx:BootstrapGridViewTextColumn>
                    <dx:BootstrapGridViewTextColumn FieldName="database_name" Caption="Naziv konekcije" VisibleIndex="3">
                    </dx:BootstrapGridViewTextColumn>
                </Columns>
                    <SettingsSearchPanel Visible="True" />
                    <CssClasses Control="control" />
                    </dx:BootstrapGridView>
                     </div>
	            </div>
	        </div>









           </div>


        <div class="inner-item user">
            <div class="control_obj">
            <div id="gridContainerUser" style="visibility: hidden">

            <asp:SqlDataSource ID="groupsGrid" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT * FROM [groups]"></asp:SqlDataSource>
            <dx:BootstrapGridView ID="groupsGridView" SettingsResizing-ColumnResizeMode="Control" DataSourceID="groupsGrid" ClientInstanceName="groupsGrid" Settings-VerticalScrollableHeight="400"  AutoPostBack="false" runat="server" Settings-VerticalScrollBarMode="Visible"  Width="70%" AutoGenerateColumns="False"  KeyFieldName="group_id"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj" CssClasses-Control="grid">
           <CssClasses Control="grid"></CssClasses>

          <CssClassesEditor NullText="Urejaj"></CssClassesEditor>
          <ClientSideEvents Init="function(s, e) { OnInitSpecific(s, e, 'user'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'user'); }" />

              <Settings VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="15" Visible="False">
          </SettingsPager>

        <SettingsText SearchPanelEditorNullText="Poiščite skupine"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
          <Columns>
              <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="false" VisibleIndex="0" ShowEditButton="True" Caption="Actions">
              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="group_id" Visible="false" Name="id" ReadOnly="false" VisibleIndex="1" Caption="Skupina">
              <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="group_name"  Visible="true" Name="Name" VisibleIndex="2" Caption="Naziv">
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="group_description" Visible="false" Name="Description" VisibleIndex="3" Caption="Opis">
              </dx:BootstrapGridViewTextColumn>
			   <dx:BootstrapGridViewTextColumn FieldName="company_id" Visible="false" Name="Company" VisibleIndex="3" Caption="Podjetje">
              </dx:BootstrapGridViewTextColumn>
          </Columns>
          <SettingsSearchPanel Visible="True" />
      </dx:BootstrapGridView>
                </div>
                </div>

    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />

                <div class="action-buttons">
                    <asp:HiddenField ID="IsInitialLoad" runat="server" Value="true" />
                    <button type="button"  runat="server" onserverclick="NewGroup_ServerClick" id="NewGroup" class="btn btn-primary actionButton">Kreiranje</button>
                    <dx:BootstrapButton runat="server" ID="DeleteGroup" UseSubmitBehavior="False"  Text="Briši" OnClick="DeleteGroup_Click" CssClasses-Control="actionButton">
                    <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>
                </div>
		                        

   </div>

	   <div class="inner-item graphs">
           <div class="control_obj">
                                <div id="gridContainerDashboard" style="visibility: hidden">

      <dx:BootstrapGridView ID="graphsGridView" runat="server" SettingsResizing-ColumnResizeMode="Control" ClientInstanceName="dashboardGrid" Settings-VerticalScrollableHeight="400"  AutoGenerateColumns="False" Settings-VerticalScrollBarMode="Visible"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj"  Width="100%" DataSourceID="query" KeyFieldName="id" CssClasses-Control="graph">
<CssClasses Control="grid"></CssClasses>

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>
                    <ClientSideEvents Init="function(s, e) { OnInitSpecific(s, e, 'dashboard'); }"  EndCallback="function(s, e) { OnEndCallback(s, e, 'dashboard'); }" />

              <Settings VerticalScrollBarMode="Visible" ShowFilterRow="false"/>
   <Toolbars>
        <dx:BootstrapGridViewToolbar Name="FilterToolbar">
            <Items>

                <dx:BootstrapGridViewToolbarItem  Text="Filter">
                     <Template>
                        <div class="dropdown">
                            <button class="btn btn-secondary" type="button" data-toggle="modal" data-target="#checkboxModal">
                                <i class="fas fa-filter"></i> Filter
                            </button>      
                        </div>
                    </Template>
                </dx:BootstrapGridViewToolbarItem>

                <dx:BootstrapGridViewToolbarItem Text="Remove Filters" Name="RemoveFilter">
                    <Template>
                        <dx:BootstrapButton runat="server" CssClasses-Icon="fas fa-times" CssClasses-Control="btn btn-danger" Text="Remove Filters" ID="ClearFilterButton" OnClick="ClearFilterButton_Click" AutoPostBack="false" Visible='<%# (Session["ActiveFilter"] != null && (bool)Session["ActiveFilter"]) %>'>
                    </dx:BootstrapButton>
                    </Template>
                 </dx:BootstrapGridViewToolbarItem>

            </Items>
        </dx:BootstrapGridViewToolbar>

    </Toolbars>
          <SettingsPager Mode="ShowAllRecords" PageSize="15" Visible="true">
          </SettingsPager>

<SettingsText SearchPanelEditorNullText="Poiščite graf"></SettingsText>

          <SettingsDataSecurity AllowEdit="False" />
          <Columns>
              <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="True"  VisibleIndex="0" ShowEditButton="False" Caption="Action">
              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="id"  Visible="false" ReadOnly="True" VisibleIndex="1">
                  <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="caption"  Name="Graf" VisibleIndex="2" Caption="Naziv">
              </dx:BootstrapGridViewTextColumn>

          </Columns>
          <SettingsSearchPanel Visible="True" />
      </dx:BootstrapGridView>
       </div>
       </div>

      <asp:SqlDataSource ID="query" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT id, caption, meta_data FROM dashboards;" >

 
      </asp:SqlDataSource>




           <div class="action-buttons">
                 <dx:BootstrapButton runat="server" Text="Shrani" ID="saveGraphs" OnClick="SaveGraphs_Click" CssClasses-Control="actionButton" AutoPostBack="false">
                    <SettingsBootstrap RenderOption="Primary" />
                  </dx:BootstrapButton>
           </div>


         <br />
    <SettingsBootstrap RenderOption="Primary" />
           </div>
        </div>





<section class="columns">

        <div class="modal fade" id="groupFormModal" tabindex="-1" role="dialog" aria-labelledby="groupFormModalLabel" aria-hidden="true">
  <div class="modal-dialog modal-lg" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="groupFormModalLabel">Skupina</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <div class="container">
          <div class="row">
            <!-- Group Name and Description -->
            <div class="col-md-12">
              <div class="form-group">
                <label for="groupName">Ime Skupine</label>
                <asp:TextBox ID="groupName" runat="server" placeholder="Vnesite ime skupine" CssClass="form-control"></asp:TextBox>
              </div>
              <div class="form-group">
                <label for="groupDescription">Opis Skupine</label>
                <asp:TextBox ID="groupDescription" runat="server" TextMode="MultiLine" placeholder="Vnesite opis skupine" CssClass="form-control"></asp:TextBox>
              </div>
            </div>
          </div>
          
            <div class="row" id="GroupsGrids" runat="server">
            <!-- Users in the group -->
            <div class="col-md-5">
                <h5>Uporabniki v skupini</h5>
                <dx:BootstrapGridView ID="usersInGroupGrid" runat="server" DataSourceID="UsersInGroupDataSource" KeyFieldName="uname">
                    <Columns>
                <dx:BootstrapGridViewCommandColumn ShowSelectCheckbox="true" SelectAllCheckboxMode="AllPages"  />
                        <dx:BootstrapGridViewTextColumn FieldName="uname" Caption="Uporabnik" />
                    </Columns>
                </dx:BootstrapGridView>
                <asp:SqlDataSource 
                    ID="UsersInGroupDataSource" 
                    runat="server"
                    ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>"
                    SelectCommand="SELECT * FROM users WHERE id_company = @id_company AND group_id = @group_id">
                    <SelectParameters>
                        <asp:Parameter Name="id_company" Type="Int32" />
                        <asp:Parameter Name="group_id" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

            <!-- Stacked buttons -->
            <div class="col-md-2 text-center d-flex flex-column align-items-center justify-content-center">
                <!-- Button to move from left to right -->
            <button type="submit" class="btn btn-primary my-2" runat="server" id="moveToNotInGroupButton" onserverclick="moveToNotInGroupButton_Click">
                <i class="fa fa-arrow-right"></i>
            </button>

            <!-- Button to move from right to left -->
            <button type="submit" class="btn btn-primary my-2" runat="server" id="moveToInGroupButton" onserverclick="moveToInGroupButton_Click">
                <i class="fa fa-arrow-left"></i>
            </button>
            </div>

            <!-- Users not in the group -->
            <div class="col-md-5">
                <h5>Uporabniki izven skupine</h5>
                <dx:BootstrapGridView ID="usersNotInGroupGrid" runat="server" DataSourceID="UsersNotInGroupDataSource" KeyFieldName="uname">
                    <Columns>
                <dx:BootstrapGridViewCommandColumn ShowSelectCheckbox="true" SelectAllCheckboxMode="AllPages" />
                        <dx:BootstrapGridViewTextColumn FieldName="uname" Caption="Uporabnik" />
                    </Columns>
                </dx:BootstrapGridView>
                <asp:SqlDataSource 
                    ID="UsersNotInGroupDataSource" 
                    runat="server"
                    ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>"
                    SelectCommand="SELECT * FROM users WHERE id_company = @id_company AND (group_id != @group_id OR group_id IS NULL);">
                    <SelectParameters>
                        <asp:Parameter Name="id_company" Type="Int32" />
                        <asp:Parameter Name="group_id" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </div>

        <div class="modal-footer">
             <asp:Button CssClass="btn btn-primary" ID="saveGroupButton" runat="server" Text="Shrani"  OnClick="SaveGroupButton_Click"/>
        </div>
    </div>
  </div>
</div>


<div class="modal fade" id="checkboxModal" tabindex="-1" role="dialog" aria-labelledby="checkboxModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-xl" role="document"> <!-- Changed modal-lg to modal-xl -->
    <div class="modal-content">
        <div class="modal-header">
            <h5 class="modal-title" id="assignMetadataModalLabel">Izberite metapodatke</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="modal-body">
            <div class="container-fluid"> <!-- Use container-fluid for wider content -->
                <div class="row">
                    <!-- Checkbox Group 1 -->
                    <div class="col-md-4">
                        <h4>Področje</h4>
                        <div class="form-group type">
                            <dx:BootstrapGridView ID="TypeGroup" ClientInstanceName="TypeGroup" AutoPostBack="false" runat="server" Settings-VerticalScrollBarMode="Visible" Width="100%" AutoGenerateColumns="False" DataSourceID="queryTypeGroup" KeyFieldName="id">
                  

                                <SettingsDataSecurity AllowEdit="False" />
                                <Columns>
                                    <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="true" VisibleIndex="0" ShowEditButton="False" Width="40px" />
                                    <dx:BootstrapGridViewTextColumn FieldName="description" Visible="true" Name="value" ReadOnly="false" VisibleIndex="1" Caption="Vrednost" />
                                    <dx:BootstrapGridViewTextColumn FieldName="value" Visible="false" Name="value" ReadOnly="false" VisibleIndex="1" Caption="Vrednost" />
                                </Columns>
                                <SettingsSearchPanel Visible="False" />
                            </dx:BootstrapGridView>
                            <asp:SqlDataSource ID="queryTypeGroup" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT id, value, description FROM meta_options WHERE option_type = 'type';" />
                        </div>
                    </div>

                    <!-- Checkbox Group 2 -->
                    <div class="col-md-4">
                        <h4>Podjetje</h4>
                        <div class="form-group company">
                            <dx:BootstrapGridView ID="CompanyGroup"  ClientInstanceName="CompanyGroup" AutoPostBack="false" runat="server" Settings-VerticalScrollBarMode="Visible" Width="100%" AutoGenerateColumns="False" DataSourceID="queryCompanyGroup" KeyFieldName="id">
                                <SettingsDataSecurity AllowEdit="False" />
                                <Columns>
                                    <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="true" VisibleIndex="0" ShowEditButton="False" Width="40px" />
                                    <dx:BootstrapGridViewTextColumn FieldName="description" Visible="true" Name="value" ReadOnly="false" VisibleIndex="1" Caption="Vrednost" />
                                    <dx:BootstrapGridViewTextColumn FieldName="value" Visible="false" Name="value" ReadOnly="false" VisibleIndex="1" Caption="Vrednost" />
                                </Columns>
                                <SettingsSearchPanel Visible="False" />
                            </dx:BootstrapGridView>
                            <asp:SqlDataSource ID="queryCompanyGroup" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT id, value, description FROM meta_options WHERE option_type = 'company';" />
                        </div>
                    </div>

                    <!-- Checkbox Group 3 -->
                    <div class="col-md-4">
                        <h4>Jezik</h4>
                        <div class="form-group language">
                            <dx:BootstrapGridView ID="LanguageGroup" ClientInstanceName="LanguageGroup" AutoPostBack="false" runat="server" Settings-VerticalScrollBarMode="Visible" Width="100%" AutoGenerateColumns="False" DataSourceID="queryTypeLanguage" KeyFieldName="id">
                                <SettingsDataSecurity AllowEdit="False" />
                                <Columns>
                                    <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="true" VisibleIndex="0" ShowEditButton="False" Width="40px" />
                                    <dx:BootstrapGridViewTextColumn FieldName="description" Visible="true" Name="value" ReadOnly="false" VisibleIndex="1" Caption="Vrednost" />
                                    <dx:BootstrapGridViewTextColumn FieldName="value" Visible="false" Name="value" ReadOnly="false" VisibleIndex="1" Caption="Vrednost" />
                                </Columns>
                                <SettingsSearchPanel Visible="False" />
                            </dx:BootstrapGridView>
                            <asp:SqlDataSource ID="queryTypeLanguage" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT id, value, description FROM meta_options WHERE option_type = 'language';" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Zapri</button>
            <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filtriraj" OnClick="BtnFilter_Click" />
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