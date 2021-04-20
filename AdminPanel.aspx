﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="peptak.AdminPanel" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

       <webopt:bundlereference runat="server" path="~/css/adminpanel.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
    <style>



        .columns {
            margin: auto;

        }

        #companyForm, #userForm {
            display:none;
        }

        .column {
            display: inline;
        }

        #saveGraphs, #byUser {
       
       
       
            display: inline-block;
            
        }

        #saveGraphs, #byUser, #deleteCompany, #company, #user, #deleteUser {
            bottom: auto;
        }
        #deleteCompany, #company {
            display: inline-block;
        }


        #user, #deleteUser {
            display: inline-block;
        }



        #user, #company, #byUser {
            float:right;
        }

       .wrapper {
           margin: auto!important;
       }
        
        #by {
            display:none;
            color: black;
        }
        .by {
            display:none;
            color: black;
        }
    </style>
	<script>



        function showOrHideDivCompany() {
          
            var v = document.getElementById("companyForm");
          
            if (v.style.display === "none") {
                if (isAllowed()) {

                    v.style.display = "block";
                } else {
                    //pass
                }
            } else {
                    v.style.display = "none";
                }
             
            }
        
        function showOrHideDivUser() {
            var v = document.getElementById("userForm");
            if (v.style.display === "none") {
                if (isAllowed()) {

                    v.style.display = "block";
                } else {
                    //pass
                }
            } else {
                v.style.display = "none";
            }

        }
        function showOrHideDivByUser() {

            var v = document.getElementById("by");
            var button = document.getElementById("byUser");
            
            if (v.style.display === "none") {
               

                v.style.display = "block";
                button.innerHTML = "Po grafu";

                } else {
                //pass
                v.style.display = "none";
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
		<dx:BootstrapListBox ID="companiesListBox" AutoPostBack="true" OnSelectedIndexChanged="companiesListBox_SelectedIndexChanged" AllowCustomValues="true" runat="server"  SelectionMode="Single"  FilteringSettings-EditorNullText="Poiščite podjetje" FilteringSettings-UseCompactView="true" ClientEnabled="true"  ViewStateMode="Enabled" Rows="2">
        <CssClasses Control="companies"  />
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
	
		<br />
		<center><button type="button" class="btn btn-primary" id="company" onclick="showOrHideDivCompany()">Novo</button></center>
        <dx:BootstrapButton runat="server" ID ="deleteCompany"  OnClick="deleteCompany_Click" Text="Briši">
    <SettingsBootstrap RenderOption="Danger" />
</dx:BootstrapButton>
	</div>
	
	<div class="column">
		<dx:BootstrapListBox ID="usersListBox" runat="server" OnSelectedIndexChanged="usersListBox_SelectedIndexChanged" AllowCustomValues="true" FilteringSettings-EditorNullText="Poiščite uporabnika" SelectionMode="Single" FilteringSettings-UseCompactView="true"  ViewStateMode="Enabled" ClientEnabled="true" AutoPostBack="true" Rows="6">
        <CssClasses Control="users" />
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
		<br />
		<center><button type="button" id="user" class="btn btn-primary" onclick="showOrHideDivUser()">Nov</button></center>
           <dx:BootstrapButton runat="server" ID="deleteUser"  Text="Briši" OnClick="deleteUser_Click" AutoPostBack="true">
    <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>

	</div>
  
  <div class="column">
	<dx:BootstrapListBox ID="graphsListBox"  runat="server" SelectionMode="CheckColumn" AllowCustomValues="true" EnableSelectAll="true" FilteringSettings-UseCompactView="true" ViewStateMode="Enabled" ClientEnabled="true" FilteringSettings-EditorNullText="Poiščite graf">
       <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />

       
</dx:BootstrapListBox>
      
      <br />
      <center><button type="button"  id="byUser" class="btn btn-secondary" onclick="showOrHideDivByUser()">Po Uporabniku</button></center>

      <dx:BootstrapButton runat="server" Text="Shrani" ID="saveGraphs" OnClick="saveGraphs_Click" CssClasses-Control="saveGraphs" AutoPostBack="true">
    <SettingsBootstrap RenderOption="Primary" />
          </dx:BootstrapButton>
	</div>
	 <div class="column" id="by">
	 <div class="reverse">
     	<dx:BootstrapListBox ID="byUserListBox" runat="server"  OnSelectedIndexChanged="byUserListBox_SelectedIndexChanged" AllowCustomValues="true" FilteringSettings-EditorNullText="Poiščite uporabnika"  SelectionMode="Multiple"  FilteringSettings-UseCompactView="true"  ViewStateMode="Enabled" ClientEnabled="true" AutoPostBack="true" Rows="6">
        <CssClasses Control="users" />
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
	</div>
	</div>
</section>	

	
	<section class="columns">
	
	<div class="column" id="companyForm">
	    <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija novega podjetja.</h3></center>  
             <hr style="color: black;" />

             <center><asp:TextBox ID="companyName" runat="server" placeholder="Ime" CssClass="form-control"></asp:TextBox></center>
           <center> <asp:TextBox ID="companyNumber" runat="server" placeholder="Številka" CssClass="form-control"></asp:TextBox> </center>                 
           <center><asp:TextBox ID="website" runat="server" placeholder="Website podjetja:" CssClass="form-control"></asp:TextBox> </center>
<center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Admin</h3></center>    
           <center> <asp:DropDownList ID="listAdmin" runat="server" > 
               <%--AppendDataBoundItems="true">--%>  
              </asp:DropDownList>  </center>
              <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Baza.</h3></center>    

                  
                  <center><asp:DropDownList ID="ConnectionStrings" runat="server"  
               AppendDataBoundItems="true">  
              </asp:DropDownList></center>  
        <br />
             <center><asp:Button CssClass="btn btn-primary" ID="companyButton" runat="server" Text="Potrdi" OnClick="companyButton_Click"/></center> 

	</div>
	
	<div class="column" id="userForm">
		        <div style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; align-items:center; display:inline-block; font-size:larger; background-color: #c5d5cb" >
                         <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija uporabnika</h3></center>

                    <center> <asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" CssClass="form-control form-control-lg"></asp:TextBox></center>
                   </center>  
                  
                    <center> <asp:TextBox ID="email" runat="server" placeholder="Email" CssClass="form-control form-control-lg"></asp:TextBox></center>
                   </center> 
              
                <center>  
                
                        <center><asp:TextBox ID="TxtUserName" runat="server" placeholder="Uporabniško ime" CssClass="form-control form-control-lg"></asp:TextBox></center>  
               
               </center>  
              
                 
                       <center> <asp:TextBox ID="TxtPassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo" CssClass="form-control form-control-lg"></asp:TextBox>  </center>
              
                  
                       <center> <asp:TextBox ID="TxtRePassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo še enkrat" CssClass="form-control form-control-lg"></asp:TextBox> </center> 
                 
           <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Pozicija</h3></center>    

                  
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
	</div>
  
  <div class="column">
      	
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
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
     </div>
  
   
  
</asp:Content>

