﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TenantAdmin.aspx.cs" Inherits="Dash.tenantadmin" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css" />
	<link rel="stylesheet" href="css/all.css" />
	<link href='https://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
    <link rel="stylesheet" href="css/all.css" />
    <link rel="stylesheet" href="css/admin.css" />
	<link href='https://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
    <webopt:bundlereference runat="server" path="~/css/shared.css" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />


       <webopt:bundlereference runat="server" path="~/css/adminpanel.css" />
    <style>

         .outer_p {
         display: flex;
         gap: 3vh;
         justify-content: center;
         align-items: center;
         }

        .box {
            min-height:485px;
        }

        .radio input[type="radio"] {
        margin-left: 3px;
             margin-right: 3px;
        }
        body {
            background-color:white!important;
        }

         html {
            background-color: white!important;
        }
        
        .column {
            background-color: none!important;
        }


        #userForm {
            background-color: none!important;
        }
        .saveByUser {
            float: right!important;
        }

        .user {
            float: right!important;
        }

        .control {
            min-height: 300px!important;
            max-height: 300px!important;
        }
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
        .grid_w {
         margin-bottom: 35px;
        }
        .item {
            padding-left: 15px!important;

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

        #byUser, #saveGraphs {
            display: inline-block!important;
        }
        #byUser {
            float:right!important;
        }


        #user, #company, #byUser {
            float:right!important;
        }

       .wrapper {
           margin: auto!important;
       }
        
        #by {
            display:block;
            color: black;
        }
        .by {
            display:block;
            color: black;
        }

        .delete {
            float:right!important;
        }

    </style>
	<script>

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

        function refresh() {
        }

        function click() {
            document.getElementById("<%= hidden.ClientID %>").click();
        }


        function showDialogSync() {


            $("#userForm").css('display', 'flex');

            var elem = document.getElementById("userForm");

            setTimeout(function () {
                elem.style.opacity = 1;
                document.getElementById('overlay').style.backgroundColor = "gray";

            }, 100);

        }



        function showOrHideDivUser() {
            var v = document.getElementById("userForm");

            if (v.style.display === "none") {

                v.style.display = "block";
            } else {
                v.style.display = "none"
            }


        }
        function showOrHideDivByUser() {

            var v = document.getElementById("by");
            var button = document.getElementById("byUser");

            if (v.style.display === "none") {


                v.style.display = "block";
                button.innerHTML = "Skrij";

            } else {
                //pass
                v.style.display = "none";
                button.innerHTML = "Po uporabniku";

            }
        }


        function OnEndCallback() {
            document.getElementById('<%= hidden.ClientID %>').click();
        }

    </script>



	<div class="outer_p"> 
	        <div class="inner_p">

		          <dx:BootstrapGridView ID="usersGridView" runat="server" Width="400"  Settings-VerticalScrollableHeight="400"  Settings-VerticalScrollBarMode="Visible" AutoGenerateColumns="False" SettingsEditing-Mode="PopupEditForm"  OnSelectionChanged="usersGridView_SelectionChanged1" KeyFieldName="uname"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj" CssClasses-Control="grid">
<CssClasses Control="grid"></CssClasses>

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>

          <Settings VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="6" Visible="False">
          </SettingsPager>

<SettingsText SearchPanelEditorNullText="Poiščite uporabnika"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
          <Columns>
              <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="false" VisibleIndex="0" ShowEditButton="True" Caption="*">
              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="uname" Visible="true" ReadOnly="false" VisibleIndex="1" Caption="Uporabniško ime">
                  <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="Pwd"  Visible="false" Name="Password" VisibleIndex="2" Caption="Password">
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="userRole" Visible="false" Name="UserRole" VisibleIndex="3" Caption="UserRole" >
              </dx:BootstrapGridViewTextColumn>
			   <dx:BootstrapGridViewTextColumn FieldName="ViewState" Visible="false" Name="ViewState" VisibleIndex="3" Caption="ViewState" >
              </dx:BootstrapGridViewTextColumn>
			   <dx:BootstrapGridViewTextColumn FieldName="email" Visible="false" Name="Email" VisibleIndex="3" Caption="Email" >
              </dx:BootstrapGridViewTextColumn>
			 
          </Columns>
          <SettingsSearchPanel Visible="True"  />
      </dx:BootstrapGridView>




    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
		<br />
		<button type="button"  runat="server" onserverclick="new_user_ServerClick" id="new_user" class="btn btn-success">Registracija</button>
           <dx:BootstrapButton runat="server" ID="deleteUser"  Text="Briši" CssClasses-Control="delete" OnClick="deleteUser_Click" AutoPostBack="true">
    <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>


   <dx:BootstrapButton runat="server" Visible="false" OnClick="hidden_Click" ID="hidden"  Text="hidden" CssClasses-Control="delete">
                                            <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>


                </div>








        <div class="inner_p">

	<dx:BootstrapListBox ID="graphsListBox" Width="400" SelectionMode="CheckColumn" runat="server" AllowCustomValues="true"   EnableSelectAll="true"   ViewStateMode="Enabled" ClientEnabled="true" CssClasses-Control="control" FilteringSettings-EditorNullText="Poiščite graf" >
                <CssClasses Control="box"  CheckBox="item"  />

       <FilteringSettings  ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />

       
</dx:BootstrapListBox>
      
      <br />       

      <dx:BootstrapButton runat="server" Text="Shrani" ID="saveGraphs" OnClick="saveGraphs_Click" CssClasses-Control="saveGraphs" AutoPostBack="true">
    <SettingsBootstrap RenderOption="Primary" />

          </dx:BootstrapButton>
    


            </div>









        <div class="inner_p">

	   <dx:BootstrapGridView ID="namesGridView" runat="server" Width="400"   Settings-VerticalScrollableHeight="400"  AutoGenerateColumns="False" Settings-VerticalScrollBarMode="Visible" KeyFieldName="ID"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj"    CssClasses-Control="graph">
<CssClasses Control="grid_w"></CssClasses>

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>

          <Settings VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="15" Visible="true">
          </SettingsPager>

<SettingsText SearchPanelEditorNullText="Poiščite graf"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
          <Columns>
              <dx:BootstrapGridViewCommandColumn VisibleIndex="0" ShowEditButton="True">
              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="ID" Name="ID" Caption="ID"  Visible="false" ReadOnly="True" VisibleIndex="1">
                  <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="Name" Name="Name" VisibleIndex="2" Caption="Naziv">
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="CustomName" Name="CustomName" VisibleIndex="3" Caption="Analiza">
              </dx:BootstrapGridViewTextColumn>
          </Columns>
          <SettingsSearchPanel Visible="True" />
      </dx:BootstrapGridView>
          
            </div>

</div>
</section>	

	
	<section class="columns">
	
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

                  
                       <asp:RadioButtonList ID="userRole" runat="server" RepeatDirection="Horizontal"  CellSpacing="8"  CellPadding="5">  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  
                <br />
          
<h3 style="text-decoration: solid; ">Pravice uporabnika.</h3>    
                    <br />
                 

              <asp:DropDownList ID="userTypeList" runat="server">
        <asp:ListItem>Viewer</asp:ListItem>
        <asp:ListItem>Viewer&amp;Designer</asp:ListItem>
    </asp:DropDownList>
        <br />
        <br />
             
              <h3 style="text-decoration: solid; ">Baza.</h3>    
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
            <script>

                $("#newUser").click(function (e) {

                    e.preventDefault();

                })


                function user() {

                    var userForm = $("#userForm");
                    userForm.show();
                }



                $(document).ready(function () {
                    $("#user").click(function () {
                        $("#userForm").css('display', 'flex');

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

