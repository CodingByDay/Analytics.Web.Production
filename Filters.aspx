<%@ Page Title="Filters" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Filters.aspx.cs" Inherits="Dash.Filters" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  

    <style>
        .square:hover {
            opacity: 0.8;
            font-weight: 700;
        }
        .newRow {
            margin-top: 1em;
        }
        .containerFilters {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 80vh; /* Center vertically within the viewport */
        }

        .square {
            width: 100px;
            height: 100px;
            margin: 20px;
            background-color: #f0f0f0;
            display: flex;
            flex-direction: column;
            justify-content: space-around;
            align-items: center;
            border: 1px solid black;
            box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.2);
            cursor: pointer;
            text-align: center;
        }

        .square img {
            max-width: 50%;
            max-height: 50%;
        }

        .square span {
            font-size: 12px;
            color: #333;
        }
    </style>

 <div class="containerFilters">
    <div class="square" onclick="openModal('modal1')">
        <img src="images/type.png" alt="Tip šifranta" />
        <span>Šifrant tipov</span>
    </div>
    <div class="square" onclick="openModal('modal2')">
        <img src="images/organization.png" alt="Šifrant podjetj" />
        <span>Šifrant podjetj</span>
    </div>
    <div class="square" onclick="openModal('modal3')">
        <img src="images/language.png" alt="Šifrant jezikov" />
        <span>Šifrant jezikov</span>
    </div>
</div>

<!-- Modal 1 (Šifrant tipov) -->
<div class="modal fade" id="modal1" tabindex="-1" aria-labelledby="modal1Label" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modal1Label">Šifrant tipov</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <dx:BootstrapGridView ID="gridViewTypes" ClientInstanceName="gridViewTypes" runat="server" 
                    DataSourceID="sqlDataSourceTypes" 
                    KeyFieldName="id"
                    SettingsDataSecurity-AllowEdit="true"
                    SettingsDataSecurity-AllowInsert="true"
                    SettingsDataSecurity-AllowDelete="true"
                    AutoGenerateColumns="False"
                    EnableRowsCache="False" >
                    
                    <Columns>
                        <dx:BootstrapGridViewCommandColumn ShowEditButton="True" Caption="Možnosti" ShowDeleteButton="True" VisibleIndex="0" />
                        <dx:BootstrapGridViewDataColumn FieldName="option_type" Visible="false" Caption="Option Type" />
                        <dx:BootstrapGridViewDataColumn FieldName="value" Caption="Vrednost" />
                        <dx:BootstrapGridViewDataColumn FieldName="description" Caption="Opis" />
                    </Columns>
                                                    <SettingsText CommandUpdate="Posodobi" CommandCancel="Zapri" CommandEdit=" " CommandDelete=" " ></SettingsText>

                    <SettingsEditing Mode="EditForm" />
                </dx:BootstrapGridView>

                <asp:SqlDataSource ID="sqlDataSourceTypes" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" 
                    SelectCommand="SELECT id, option_type, value, description FROM meta_options WHERE option_type = 'type'"
                    InsertCommand="INSERT INTO meta_options (option_type, value, description) VALUES ('type', @value, @description)"
                    UpdateCommand="UPDATE meta_options SET value = @value, description = @description WHERE id = @id"
                    DeleteCommand="DELETE FROM meta_options WHERE id = @id">
                    
                    <InsertParameters>
                        <asp:Parameter Name="value" Type="String" />
                        <asp:Parameter Name="description" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                        <asp:Parameter Name="value" Type="String" />
                        <asp:Parameter Name="description" Type="String" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>

                <!-- Add New Button -->
                <button class="btn btn-primary newRow" onclick="gridView_AddNewRow('gridViewTypes')">
                    <i class="fas fa-plus"></i>
                </button>

            </div>
        </div>
    </div>
</div>

<!-- Modal 2 (Šifrant podjetj) -->
<div class="modal fade" id="modal2" tabindex="-1" aria-labelledby="modal2Label" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modal2Label">Šifrant podjetj</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <dx:BootstrapGridView ID="gridViewOrganizations" ClientInstanceName="gridViewOrganizations" runat="server" 
                    DataSourceID="sqlDataSourceOrganizations" 
                    KeyFieldName="id"
                    SettingsDataSecurity-AllowEdit="true"
                    SettingsDataSecurity-AllowInsert="true"
                    SettingsDataSecurity-AllowDelete="true"
                    AutoGenerateColumns="False"
                    EnableRowsCache="False" >
                    
                    <Columns>
                        <dx:BootstrapGridViewCommandColumn ShowEditButton="True" ShowDeleteButton="True" VisibleIndex="0" Caption="Možnosti" />
                        <dx:BootstrapGridViewDataColumn FieldName="option_type" Visible="false" Caption="Option Type" />
                        <dx:BootstrapGridViewDataColumn FieldName="value" Caption="Vrednost" />
                        <dx:BootstrapGridViewDataColumn FieldName="description" Caption="Opis" />
                    </Columns>
                                <SettingsText CommandUpdate="Posodobi" CommandCancel="Zapri" CommandEdit=" " CommandDelete=" " ></SettingsText>

                    <SettingsEditing Mode="EditForm" />
                </dx:BootstrapGridView>

                <asp:SqlDataSource ID="sqlDataSourceOrganizations" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" 
                    SelectCommand="SELECT id, option_type, value, description FROM meta_options WHERE option_type = 'company'"
                    InsertCommand="INSERT INTO meta_options (option_type, value, description) VALUES ('company', @value, @description)"
                    UpdateCommand="UPDATE meta_options SET value = @value, description = @description WHERE id = @id"
                    DeleteCommand="DELETE FROM meta_options WHERE id = @id">
                    
                    <InsertParameters>
                        <asp:Parameter Name="value" Type="String" />
                        <asp:Parameter Name="description" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                        <asp:Parameter Name="value" Type="String" />
                        <asp:Parameter Name="description" Type="String" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>

                <button class="btn btn-primary newRow" onclick="gridView_AddNewRow('gridViewOrganizations')">
                    <i class="fas fa-plus"></i>
                </button>

            </div>
        </div>
    </div>
</div>

<!-- Modal 3 (Šifrant jezikov) -->
<div class="modal fade" id="modal3" tabindex="-1" aria-labelledby="modal3Label" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modal3Label">Šifrant jezikov</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <dx:BootstrapGridView ID="gridViewLanguages" ClientInstanceName="gridViewLanguages" runat="server" 
                    DataSourceID="sqlDataSourceLanguages" 
                    KeyFieldName="id"
                    SettingsDataSecurity-AllowEdit="true"
                    SettingsDataSecurity-AllowInsert="true"
                    SettingsDataSecurity-AllowDelete="true"
                    AutoGenerateColumns="False"
                    EnableRowsCache="False" >
                    
                    <Columns>
                        <dx:BootstrapGridViewCommandColumn ShowEditButton="True" ShowDeleteButton="True" VisibleIndex="0" Caption="Možnosti" />
                        <dx:BootstrapGridViewDataColumn FieldName="option_type" Visible="false" Caption="Option Type" />
                        <dx:BootstrapGridViewDataColumn FieldName="value" Caption="Vrednost" />
                        <dx:BootstrapGridViewDataColumn FieldName="description" Caption="Opis" />
                    </Columns>
                                <SettingsText CommandUpdate="Posodobi" CommandCancel="Zapri" CommandEdit=" " CommandDelete=" " ></SettingsText>

                    <SettingsEditing Mode="EditForm" />
                </dx:BootstrapGridView>

                <asp:SqlDataSource ID="sqlDataSourceLanguages" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" 
                    SelectCommand="SELECT id, option_type, value, description FROM meta_options WHERE option_type = 'language'"
                    InsertCommand="INSERT INTO meta_options (option_type, value, description) VALUES ('language', @value, @description)"
                    UpdateCommand="UPDATE meta_options SET value = @value, description = @description WHERE id = @id"
                    DeleteCommand="DELETE FROM meta_options WHERE id = @id">
                    
                    <InsertParameters>
                        <asp:Parameter Name="value" Type="String" />
                        <asp:Parameter Name="description" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                        <asp:Parameter Name="value" Type="String" />
                        <asp:Parameter Name="description" Type="String" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>

                <!-- Add New Button -->
                
                <button class="btn btn-primary newRow" onclick="gridView_AddNewRow('gridViewLanguages')">
                    <i class="fas fa-plus"></i>
                </button>



            </div>
        </div>
    </div>
</div>


    <script>
        window.addEventListener('resize', function () {
            location.reload();
        });
        function openModal(modalId) {
            var myModal = new bootstrap.Modal(document.getElementById(modalId));
            myModal.show();
        }


        function gridView_AddNewRow(gridViewId) {

            event.preventDefault();

            if (gridViewId == "gridViewTypes") {

                // Add a new row to the grid
                gridViewTypes.AddNewRow();
                // Start editing the last added row (the new row)
                var newRowIndex = gridViewTypes.cpTotalRows - 1; // Get the index of the new row
            } else if (gridViewId == "gridViewOrganizations") {
                // Add a new row to the grid
                gridViewOrganizations.AddNewRow();
                // Start editing the last added row (the new row)
                var newRowIndex = gridViewOrganizations.cpTotalRows - 1; // Get the index of the new row
            } else if (gridViewId == "gridViewTypes") {

                // Add a new row to the grid
                gridViewLanguages.AddNewRow();
                // Start editing the last added row (the new row)
                var newRowIndex = gridViewLanguages.cpTotalRows - 1; // Get the index of the new row
            }
        }
    </script>

</asp:Content>
