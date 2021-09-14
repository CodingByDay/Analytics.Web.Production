<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="peptak.admin" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css" />
	<link rel="stylesheet" href="css/all.css" />

	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
          <webopt:bundlereference runat="server" path="~/css/shared.css" />

<link href= "~/css/shared.css" rel="stylesheet" runat="server" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />

    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

       <webopt:bundlereference runat="server" path="~/css/adminpanel.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
    <style>

      
    </style>
	<script>

        $("#newUser").click(function (e) {

            e.preventDefault();

        })

        function showOrHideDivCompany() {

            var v = document.getElementById("companyForm");
            var buttonCompany = document.getElementById("company");
            if (v.style.display === "none") {
                if (isAllowed()) {

                    v.style.display = "block";
                    buttonCompany.innerHTML = "Skrij";
                } else {
               
                }
            } else {
                v.style.display = "none";
                buttonCompany.innerHTML = "Dodaj";
            }

        }


        // user, company 


        $("#user").on("click", function () {
            alert("Heello");
        });

        // Shorthand for $( document ).ready()
        $(function () {
            alert("Heello");
        });


        $("#company").on("click", function () {
            alert("Heello");
        });

        function showConnection() {
            
            var divider = document.getElementById("ConnectionStringDiv");
            var button = document.getElementById("add");
            if (divider.style.display === "none") {
                divider.style.display = "block";
                button.innerHTML = "Skrij";
            } else {
                divider.style.display = "none";
                button.innerHTML = "Dodaj konekcijo";
            }
        }

        function showOrHideDivUser() {
            var buttonUser = document.getElementById("user");
            var v = document.getElementById("userForm");
            if (v.style.display === "none") {
                if (isAllowed()) {

                    v.style.display = "block";
                    buttonUser.innerHTML = "Skrij";
                } else {
                }
            } else {
                v.style.display = "none";
                buttonUser.innerHTML = "Dodaj/Spremeni";

            }

        }
        function showOrHideDivByUser() {

            var v = document.getElementById("by");
            var button = document.getElementById("byUser");

            if (v.style.display == "none") {

                console.log("none")
                v.style.display = "block";
                button.innerHTML = "Skrij";

            } else {
                v.style.display = "none";
                console.log("other")
                button.innerHTML = "Po uporabniku";

            }
        }


        function isAllowed() {
            var company = document.getElementById("companyForm");

            var user = document.getElementById("userForm");



            if (company.style.display == "block" || user.style.display == "block") {
                return false;

            } else if (company.style.display == "block" && user.style.display == "block") {
                return false;
            } else {
                return true;
            }
        }

    </script>


	<div id="boot">

		</div>
	
<div class="wrapper">

	<header>
		<h1></h1>
	</header>
		
<section class="columns">
	
	<div class="column">
		<h2></h2>
		<p></h1>
			
	</header>

<section class="columns">
	
	<div class="column">
		<dx:BootstrapListBox ID="companiesListBox" AutoPostBack="true" OnSelectedIndexChanged="companiesListBox_SelectedIndexChanged" AllowCustomValues="true" runat="server"  SelectionMode="Single"  FilteringSettings-EditorNullText="Poiščite podjetje" CssClasses-Control="control" FilteringSettings-UseCompactView="true" ClientEnabled="true"  ViewStateMode="Enabled" Rows="5">
        <CssClasses Control="control"  />
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
	
		<br />
		<center><button type="button" class="btn btn-primary" id="company" onclick="showOrHideDivCompany()">Dodaj</button></center>
        <dx:BootstrapButton runat="server" ID ="deleteCompany"  OnClick="deleteCompany_Click" Text="Briši">
    <SettingsBootstrap RenderOption="Danger" />
</dx:BootstrapButton>
	</div>


   
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT Dashboards.Caption, Dashboards.belongsTo, Dashboards.ID FROM Dashboards " UpdateCommand="UPDATE Dashboards SET belongsTo = @belongsTo WHERE (ID = @ID)">
        <UpdateParameters>
            <asp:Parameter Name="belongsTo" />
            <asp:Parameter Name="ID" />
        </UpdateParameters>
    </asp:SqlDataSource>
	
	<div class="column">
		<dx:BootstrapListBox ID="usersListBox" runat="server"  OnSelectedIndexChanged="usersListBox_SelectedIndexChanged" AllowCustomValues="true" FilteringSettings-EditorNullText="Poiščite uporabnika" SelectionMode="Single" FilteringSettings-UseCompactView="true" CssClasses-Control="control" ViewStateMode="Enabled" ClientEnabled="true" AutoPostBack="true" Rows="5">
                <CssClasses Control="control"  />




    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
		<br />    
		<center><button type="button" id="user" class="btn btn-primary" onclick="showOrHideDivUser()">Dodaj/Spremeni</button></center>
           <dx:BootstrapButton runat="server" ID="deleteUser"  Text="Briši" OnClick="deleteUser_Click">
    <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>

	</div>
  
  <div class="column">
	<dx:BootstrapListBox ID="graphsListBox"  runat="server" SelectionMode="CheckColumn" AllowCustomValues="true" EnableSelectAll="true" FilteringSettings-UseCompactView="true" ViewStateMode="Enabled" ClientEnabled="true" CssClasses-Control="control" FilteringSettings-EditorNullText="Poiščite graf"  Rows="4">
                <CssClasses Control="control"  />

       <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />

       
</dx:BootstrapListBox>


       <dx:ASPxGridView ID="graphsBox" runat="server" AutoGenerateColumns="False" KeyFieldName="ID" DataSourceID="SqlDataSource1" Theme="Office2003Blue">
           <Settings ShowFilterRow="True" />
<SettingsPopup>
<FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>
           <SettingsSearchPanel Visible="True" />
        <Columns>
            <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="Caption" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="belongsTo" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ID" Visible="false" ReadOnly="True" VisibleIndex="3">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
	
      <br />       

      <dx:BootstrapButton runat="server" Text="Shrani" ID="saveGraphs" OnClick="saveGraphs_Click" CssClasses-Control="saveGraphs" AutoPostBack="true">
    <SettingsBootstrap RenderOption="Primary" />

          </dx:BootstrapButton>
      <dx:BootstrapButton runat="server" ID ="byUser" CssClasses-Control="user"  OnClick="byUser_Click" Text="Po uporabniku">
    <SettingsBootstrap RenderOption="Info" />
</dx:BootstrapButton>
	</div>
	 <div class="column" id="by" runat="server">
	 <div class="reverse">
     	<dx:BootstrapListBox ID="byUserListBox" runat="server" OnSelectedIndexChanged="byUserListBox_SelectedIndexChanged" AllowCustomValues="true" FilteringSettings-EditorNullText="Poiščite uporabnika"  SelectionMode="Multiple"  FilteringSettings-UseCompactView="true" CssClasses-Control="control"  ViewStateMode="Enabled" ClientEnabled="true" AutoPostBack="true" Rows="5">
        <CssClasses Control="control"  />
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
         <br />
      <center><dx:BootstrapButton runat="server" ID ="saveByUser" OnClick="saveByuser_Click" Text="Shrani">
    <SettingsBootstrap RenderOption="Primary" />
</dx:BootstrapButton></center>
	</div>
	</div>
</section>	

	
	<section class="columns">
	
	<div class="column" id="companyForm">
	    <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija novega podjetja.</h3></center>  
             <hr style="color: black;" />

             <center><asp:TextBox ID="companyName" runat="server" placeholder="Ime" CssClass="form-control"></asp:TextBox></center>
        <br />
           <center> <asp:TextBox ID="companyNumber" runat="server" placeholder="Številka" CssClass="form-control"></asp:TextBox> </center>  
                <br />

           <center><asp:TextBox ID="website" runat="server" placeholder="Website podjetja:" CssClass="form-control"></asp:TextBox> </center>
                <br />

<center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Admin</h3></center> 
                <br />

           <center> <asp:DropDownList ID="listAdmin" runat="server" > 
               <%--AppendDataBoundItems="true">--%>  
              </asp:DropDownList>  </center>
                <br />

              <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Baza.</h3></center>    
                <br />

                  
                  <center><asp:DropDownList ID="ConnectionStrings" runat="server"  
               AppendDataBoundItems="true">  
              </asp:DropDownList></center> 
        <br />
        <center><button type="button" id="add" class="btn btn-info" onclick="showConnection()">Dodaj konekcijo</button></center>
        <br /> <div id="ConnectionStringDiv">
      <center><dx:BootstrapButton runat="server" IconCssClass="bi bi-plus"  CssClasses-Control="plus" ID="AddConnection" OnClick="AddConnection_Click" Text="Testiraj konekcijo" AutoPostBack="false">
        <SettingsBootstrap RenderOption="Info" />
       </dx:BootstrapButton>
        <asp:TextBox ID="ConnectionString" runat="server" placeholder="Connection string:" Width="1200" CssClass="conn"></asp:TextBox>
                <asp:TextBox ID="connName" runat="server" placeholder="Ime:" Width="500" CssClass="conn"></asp:TextBox></center> 
            </div>
        <br />
             <center><asp:Button CssClass="btn btn-primary" ID="companyButton" runat="server" Text="Potrdi" OnClick="companyButton_Click"/></center> 

	</div>
	
	<div class="column" id="userForm">
		        <div style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; align-items:center; display:inline-block; font-size:larger; background-color: #c5d5cb" >
                         <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija/sprememba uporabnika</h3></center>
                  
                                                <br />
                   <center> <dx:BootstrapButton runat="server" ID="newUser"  Text="Novi uporabnik" OnClick="newUser_Click"  AutoPostBack="false">
    <SettingsBootstrap RenderOption="Success" /></dx:BootstrapButton></center>
                    <br />
                    <center> <asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" CssClass="form-control form-control-lg"></asp:TextBox></center>
                   </center>  
                          <br />

                    <center> <asp:TextBox ID="email" runat="server" placeholder="Email" CssClass="form-control form-control-lg"></asp:TextBox></center>
                   </center> 
                      <br />

                <center>  
                
                        <center><asp:TextBox ID="TxtUserName" runat="server" placeholder="Uporabniško ime" CssClass="form-control form-control-lg"></asp:TextBox></center>  
               
               </center>  
                      <br />

                 
                       <center> <asp:TextBox ID="TxtPassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo" CssClass="form-control form-control-lg"></asp:TextBox>  </center>
                      <br />

                  
                       <center> <asp:TextBox ID="TxtRePassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo še enkrat" CssClass="form-control form-control-lg"></asp:TextBox> </center> 
                         <br />

           <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Pozicija</h3></center>    
                            <br />

                  
                       <center><asp:RadioButtonList ID="userRole" runat="server">  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  </center>
                <br />
<center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Tip uporabnika.</h3></center>    
                    <br />
                <center><asp:DropDownList ID="userType" autopostback="false" runat="server"  >
                 </asp:DropDownList></center>   
        <br />
              
                   <center> <h4 style="text-decoration: solid"Podjetje:</h4></center>  
                       <center> <asp:DropDownList ID="companiesList" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  </center>
                    <br />
              
     <center><asp:Button CssClass="btn btn-primary" ID="registrationButton" runat="server" Text="Potrdi"  OnClick="registrationButton_Click" /></center>
                    <br />
	</div>
  
 
	
</section>

     </div>
     </div> 
    </div>
    </div>
    </div>
    </div>
    </div>
    </div>
</asp:Content>

