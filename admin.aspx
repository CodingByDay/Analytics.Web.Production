<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="peptak.admin" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css" />
	<link rel="stylesheet" href="css/all.css" />
    	<link rel="stylesheet" href="css/admin.css" />

	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
          <webopt:bundlereference runat="server" path="~/css/shared.css" />

<link href= "~/css/shared.css" rel="stylesheet" runat="server" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link href= "~/css/admin.css" rel="stylesheet" runat="server" type="text/css" />

     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

       <webopt:bundlereference runat="server" path="~/css/adminpanel.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
    <style>


      
    </style>

	


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
		<center><button type="button" class="btn btn-primary" id="company">Dodaj</button></center>
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
		<center><button type="button" id="user" class="btn btn-primary">Dodaj/Spremeni</button></center>
           <dx:BootstrapButton runat="server" ID="deleteUser"  Text="Briši" OnClick="deleteUser_Click">
    <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>

	</div>
  
  <div class="column">
	   
      <dx:BootstrapGridView ID="graphsGridView" runat="server" AutoGenerateColumns="False"  DataSourceID="query" KeyFieldName="ID" CssClasses-Control="grid">
<CssClasses Control="grid"></CssClasses>

          <Settings VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="6" Visible="False">
          </SettingsPager>
          <SettingsDataSecurity AllowEdit="True" />
          <Columns>
              <dx:BootstrapGridViewTextColumn FieldName="ID" Visible="false" ReadOnly="True" VisibleIndex="0">
                  <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="Caption"  Name="Graf" VisibleIndex="1" Caption="Naziv">
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="belongsTo" Name="Podjetje" VisibleIndex="2" Caption="Podjetje" >
              </dx:BootstrapGridViewTextColumn>
          </Columns>
          <SettingsSearchPanel Visible="True"  />
      </dx:BootstrapGridView>

       


    
      <asp:SqlDataSource ID="query" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT ID, Caption, belongsTo FROM Dashboards;" UpdateCommand="UPDATE Dashboards SET belongsTo=@belongsTo WHERE ID=@ID">
          <UpdateParameters>
              <asp:Parameter Name="belongsTo" />
              <asp:Parameter Name="ID" />
          </UpdateParameters>
      </asp:SqlDataSource>
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
	





<!-- COMPANY FORM -->



	<div class="column" id="companyForm"  >

        <div class="companyPart">
	    <h5 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija novega podjetja.</h5>
             <hr style="color: black;" />



                   <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Naziv podjetja</label>
           <asp:TextBox ID="companyName" runat="server" placeholder="Ime" CssClass="form-control"></asp:TextBox>
                       </div>


           <br />

                 <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Kontaktna številka</label>
            <asp:TextBox ID="companyNumber" runat="server" placeholder="Kontaktna številka" CssClass="form-control"></asp:TextBox>   
                       </div>



                <br />

                 <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Spletna stran</label>
           <asp:TextBox ID="website" runat="server" placeholder="Spletna stran:" CssClass="form-control"></asp:TextBox> 
                       </div>



                <br />
                <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Admin</label>
      
            <asp:DropDownList ID="listAdmin" runat="server" > 
               <%--AppendDataBoundItems="true">--%>  
              </asp:DropDownList> 
                       </div>


           
                <br />

               <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Izberite bazo</label>
            <asp:DropDownList ID="ConnectionStrings" runat="server"  
               AppendDataBoundItems="true">  
              </asp:DropDownList> 
                       </div>

       
            </div>
      <div class="connectionPart">
          <br />
          <br />
       
       
          <br />
            <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Connection string</label>
        <asp:TextBox ID="ConnectionString" runat="server" placeholder="Connection string:" Width="1200" CssClass="conn"></asp:TextBox>
                       </div>
          <br />
            <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Ime povezave</label>
                <asp:TextBox ID="connName" runat="server" placeholder="Ime" Width="500" CssClass="conn"></asp:TextBox> 
                       </div>

          <br />
     
      <dx:BootstrapButton runat="server" IconCssClass="bi bi-plus"  CssClasses-Control="plus" ID="AddConnection" OnClick="AddConnection_Click" Text="Testiraj konekcijo" AutoPostBack="false">
        <SettingsBootstrap RenderOption="Info" />
       </dx:BootstrapButton>
        <br />
        <br />

                       <asp:Button CssClass="btn btn-primary" ID="companyButton" runat="server" Text="Potrdi" OnClick="companyButton_Click"/> 

                      <div id="btnsCompany" style="position:absolute;float:right; right:0px;top:0px;">

          <button type="button" class="btn btn-danger" id="closeCompany" style="padding: 3px;">X</button>
                          </div>
        <br />
        <br />
      
	</div>
	


                    </div>




        <!-- -USER FORM-->

















	<div class="column" id="userForm" style="display: none">
                       
                    <br />
                   <div class ="auth">
                  
                                                <br />
                       <div id="new" style="position:absolute;left:0px;top:0px;">
                   <center><dx:BootstrapButton runat="server" ID="newUser"  Text="Novi uporabnik" OnClick="newUser_Click" UseSubmitBehavior="False" CausesValidation="False" AutoPostBack="false">
                    <SettingsBootstrap RenderOption="Success" /></dx:BootstrapButton></center></div>
               
                       <hr />
    <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Ime in Priimek</label>
                 <asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" CssClass="form-control form-control-lg"></asp:TextBox>
          </div>            
                          <br />



     <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Email</label>
                 <asp:TextBox ID="email" runat="server" placeholder="Email" CssClass="form-control form-control-lg"></asp:TextBox>
          </div>
                    
                      <br />
<div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Uporabniško ime</label>
                        <asp:TextBox ID="TxtUserName" runat="server" placeholder="Uporabniško ime" CssClass="form-control form-control-lg"></asp:TextBox>  
          </div>
                  
                
               
                 
                      <br />
                       <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Geslo</label>
<asp:TextBox ID="TxtPassword" runat="server"  TextMode="Password" placeholder="Geslo" CssClass="form-control form-control-lg"></asp:TextBox>           

                       </div>
                 
         
                      <br />
                        <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Ponovite geslo</label>
                        <asp:TextBox ID="TxtRePassword" runat="server"  TextMode="Password" placeholder="Geslo še enkrat" CssClass="form-control form-control-lg"></asp:TextBox>  

                       </div>
                  
                         <br />
            </div>
        <div class="other">

                            <br />
                            <br />
                          
                            <br />
                            <br />
                            <br />
                            <br />


           <h3 >Vloga uporabnika</h3>    
                            <br />

                  
                       <asp:RadioButtonList ID="userRole" runat="server" RepeatDirection="Horizontal"  CellPadding="5">  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  
                <br />
          
<h3 style="text-decoration: solid; ">Pravice uporabnika.</h3>    
                    <br />
               <asp:RadioButtonList ID="userTypeRadio" runat="server" RepeatDirection="Horizontal" CellPadding="5">  
                            <asp:ListItem>Viewer</asp:ListItem>  
                            <asp:ListItem>Designer</asp:ListItem>  
                            <asp:ListItem>Viewer&Designer</asp:ListItem>  
                        </asp:RadioButtonList>  
        <br />
        <br />
             
              
                    <h4 style="text-decoration: solid"Podjetje:</h4>  
                        <asp:DropDownList ID="companiesList" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  
                    <br />
                    <br />
                 <asp:Button CssClass="btn btn-primary" ID="registrationButton" runat="server" Text="Potrdi"  OnClick="registrationButton_Click" />


                        <div id="btns" style="position:absolute;float:right; right:0px;top:0px;">

                      <button type="button" class="btn btn-danger" id="closeUser" style="padding: 3px;">X</button>
                </div>
              </div>
                    <br />
	</div>
  
 
	
</section>

     </div>
     </div> 

 

 
<script>

    dragElement(document.getElementById("userForm"));
    dragElement(document.getElementById("companyForm"));

    function dragElement(elmnt) {
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
        if (document.getElementById(elmnt.id + "header")) {
            // if present, the header is where you move the DIV from:
            document.getElementById(elmnt.id + "header").onmousedown = dragMouseDown;
        } else {
            // otherwise, move the DIV from anywhere inside the DIV:
            elmnt.onmousedown = dragMouseDown;
        }

        function dragMouseDown(e) {
            e = e || window.event;
            e.preventDefault();
            // get the mouse cursor position at startup:
            pos3 = e.clientX;
            pos4 = e.clientY;
            document.onmouseup = closeDragElement;
            // call a function whenever the cursor moves:
            document.onmousemove = elementDrag;
        }

        function elementDrag(e) {
            e = e || window.event;
            e.preventDefault();
            // calculate the new cursor position:
            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;
            // set the element's new position:
            elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
            elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
        }

        function closeDragElement() {
            // stop moving when mouse button is released:
            document.onmouseup = null;
            document.onmousemove = null;
        }
    }
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

        });
    });


    $(document).ready(function () {
        $("#company").click(function () {
            $("#companyForm").css('display', 'flex');
        });
    });


    $(document).ready(function () {
        $("#closeCompany").click(function () {
            $("#companyForm").css('display', 'none');
        });
    });

    $(document).ready(function () {
        $("#closeUser").click(function () {
            $("#userForm").css('display', 'none');
        });
    });



</script>
 

 
 
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

