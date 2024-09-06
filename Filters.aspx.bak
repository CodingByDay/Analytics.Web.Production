<%@ Page Title="Filters" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Filters.aspx.cs" Inherits="Dash.Filters" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  

    <style>

        .container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 80vh; /* Center vertically within the viewport */
        }

        .square {
            width: 150px;
            height: 150px;
            margin: 20px;
            background-color: #f0f0f0;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            border: 2px solid #ccc;
            box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.2);
            cursor: pointer;
            text-align: center;
        }

        .square img {
            max-width: 50%;
            max-height: 50%;
        }

        .square span {
            margin-top: 10px;
            font-size: 14px;
            color: #333;
        }
    </style>

    <div class="container">
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

    <!-- Modal 1 -->
    <div class="modal fade" id="modal1" tabindex="-1" aria-labelledby="modal1Label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal1Label">Šifrant tipov</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form>
                        <!-- Form content for Šifrant tipov -->
                        <div class="mb-3">
                            <label for="typeInput" class="form-label">Type</label>
                            <input type="text" class="form-control" id="typeInput">
                        </div>
                        <button type="submit" class="btn btn-primary">Submit</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal 2 -->
    <div class="modal fade" id="modal2" tabindex="-1" aria-labelledby="modal2Label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal2Label">Šifrant podjetj</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form>
                        <!-- Form content for Šifrant podjetj -->
                        <div class="mb-3">
                            <label for="organizationInput" class="form-label">Organization</label>
                            <input type="text" class="form-control" id="organizationInput">
                        </div>
                        <button type="submit" class="btn btn-primary">Submit</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal 3 -->
    <div class="modal fade" id="modal3" tabindex="-1" aria-labelledby="modal3Label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal3Label">Šifrant jezikov</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form>
                        <!-- Form content for Šifrant jezikov -->
                        <div class="mb-3">
                            <label for="languageInput" class="form-label">Language</label>
                            <input type="text" class="form-control" id="languageInput">
                        </div>
                        <button type="submit" class="btn btn-primary">Submit</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <script>
        function openModal(modalId) {
            var myModal = new bootstrap.Modal(document.getElementById(modalId));
            myModal.show();
        }
    </script>

</asp:Content>
