<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Dash.admin" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v21.1, Version=21.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v21.1, Version=21.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css" />
	<link rel="stylesheet" href="css/all.css" />
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">
    	<link rel="stylesheet" href="css/admin.css" />
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
          <webopt:bundlereference runat="server" path="~/css/shared.css" />
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />

     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

       <webopt:bundlereference runat="server" path="~/css/adminpanel.css" />


    

         <script type="text/c#" runat="server">
             [System.Web.Services.WebMethod(EnableSession = true)]
             public static bool test(string InitialCatalog, string DataSource, string UserID, string Password)
             {

                 var result = Dash.Administration.HelpfullMethods.testSQL(InitialCatalog, DataSource, UserID, Password);



                 if(result)
                    {
                     return true;
                    } else
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
            console.log(JSON.stringify({ InitialCatalog: InitialCatalog, DataSource: DataSource, UserID: UserID, Password: Password }));
          
            $.ajax({
                type: 'POST',
                url: '<%= ResolveUrl("~/admin.aspx/test") %>',
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
                document.getElementById('overlay').style.backgroundColor = "gray";

            }, 100);

        }
        function showDialogSyncCompany() {


            $("#companyForm").css('display', 'flex');

            var elem = document.getElementById("companyForm");

            setTimeout(function () {
                elem.style.opacity = 1;
                document.getElementById('overlay').style.backgroundColor = "gray";

            }, 100);

        }




    </script>
    <style>

     .delete {

         float: right!important;

     }


        #overlay {

            width: 100%!important;
            height: 100%!important;
            z-index: 0!important;            
            
        }



        .radio input[type="radio"] {
             margin-left: 3px;
             margin-right: 3px;
        }
      
    </style>

	


	<div id="boot">

		</div>
	
<div class="wrapper">

	<header>
       

		<div id="overlay">



		</div>


<section class="columns">
	
	<div class="column">
		<h2></h2>
		<p></h1>
			
	</header>

<section class="columns">
	
	<div class="column">
        <div class="inv" style="z-index:0!important">

		    <asp:SqlDataSource ID="companiesGrid" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT [id_company], [company_name], [databaseName], [admin_id] FROM [companies]"></asp:SqlDataSource>

        <dx:BootstrapGridView ID="companiesGridView" runat="server" SettingsEditing-Mode="PopupEditForm" KeyFieldName="id_company" DataSourceID="companiesGrid" CssClasses-Control="control" Width="320px"  AutoGenerateColumns="False">
                <CssClassesEditor NullText="Urejaj"></CssClassesEditor>

              <Settings VerticalScrollBarMode="Visible" />
             <SettingsPager Mode="ShowAllRecords" PageSize="6" Visible="False">
             </SettingsPager>
            <SettingsText SearchPanelEditorNullText="Poiščite podjetje"></SettingsText>

<SettingsEditing Mode="PopupEditForm"></SettingsEditing>

                   <SettingsDataSecurity AllowEdit="True" />
                <Columns>
                      
                    <dx:BootstrapGridViewCommandColumn ShowEditButton="True" VisibleIndex="0">
                    </dx:BootstrapGridViewCommandColumn>
                      
                    <dx:BootstrapGridViewTextColumn FieldName="id_company" Visible="False" ReadOnly="True" VisibleIndex="1">
                    </dx:BootstrapGridViewTextColumn>
                    <dx:BootstrapGridViewTextColumn FieldName="company_name" Caption="Podjetje" VisibleIndex="2">
                    </dx:BootstrapGridViewTextColumn>
                    <dx:BootstrapGridViewTextColumn FieldName="databaseName" Caption="Naziv konekcije" VisibleIndex="3">
                    </dx:BootstrapGridViewTextColumn>
                      <dx:BootstrapGridViewTextColumn FieldName="admin_id" Caption="Admin" VisibleIndex="4">
                    </dx:BootstrapGridViewTextColumn>
                      
                </Columns>
                  <SettingsSearchPanel Visible="True"  />
            <CssClasses Control="control" />
            </dx:BootstrapGridView>
	</div>
		<br />
        <div class="companyButtons" style="position: relative;top:-15px!important">
		<center><button type="button" class="btn btn-primary" id="company">Dodaj</button></center>
        <dx:BootstrapButton runat="server" ID ="deleteCompany"  OnClick="deleteCompany_Click" Text="Briši">
    <SettingsBootstrap RenderOption="Danger" />
</dx:BootstrapButton>
            </div>
	</div>


   
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:graphsConnectionString %>" SelectCommand="SELECT Dashboards.Caption, Dashboards.belongsTo, Dashboards.ID FROM Dashboards " UpdateCommand="UPDATE Dashboards SET belongsTo = @belongsTo WHERE (ID = @ID)">
        <UpdateParameters>
            <asp:Parameter Name="belongsTo" />
            <asp:Parameter Name="ID" />
        </UpdateParameters>
    </asp:SqlDataSource>
	
	<div class="column">
	
          <dx:BootstrapGridView ID="usersGridView" runat="server" AutoGenerateColumns="False"  SettingsEditing-Mode="PopupEditForm" OnSelectionChanged="usersGridView_SelectionChanged"  KeyFieldName="uname"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj" CssClasses-Control="grid">
<CssClasses Control="grid"></CssClasses>

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>

          <Settings VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="6" Visible="False">
          </SettingsPager>

<SettingsText SearchPanelEditorNullText="Poiščite uporabnika"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
          <Columns>
              <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="false" VisibleIndex="0" ShowEditButton="True" Caption="*" >
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
        
		<button type="button"  runat="server" onserverclick="new_user_ServerClick2" id="new_user"  class="btn btn-primary">Registracija</button>

	

        <dx:BootstrapButton runat="server" ID="deleteUser"  Text="Briši" OnClick="deleteUser_Click" CssClasses-Control="delete">

        <SettingsBootstrap RenderOption="Danger" /></dx:BootstrapButton>

  
        

	</div>
  
  <div class="column">
	   
      <dx:BootstrapGridView ID="graphsGridView" runat="server" AutoGenerateColumns="False"  SettingsText-SearchPanelEditorNullText="Poiščite graf" CssClassesEditor-NullText="Urejaj"  DataSourceID="query" KeyFieldName="ID" CssClasses-Control="grid">
<CssClasses Control="grid"></CssClasses>

<CssClassesEditor NullText="Urejaj"></CssClassesEditor>

          <Settings VerticalScrollBarMode="Visible" />
          <SettingsPager Mode="ShowAllRecords" PageSize="6" Visible="False">
          </SettingsPager>

<SettingsText SearchPanelEditorNullText="Poiščite graf"></SettingsText>

          <SettingsDataSecurity AllowEdit="True" />
          <Columns>
              <dx:BootstrapGridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="True" VisibleIndex="0" ShowEditButton="True">
              </dx:BootstrapGridViewCommandColumn>
              <dx:BootstrapGridViewTextColumn FieldName="ID"  Visible="false" ReadOnly="True" VisibleIndex="1">
                  <SettingsEditForm Visible="False" />
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="Caption"  Name="Graf" VisibleIndex="2" Caption="Naziv">
              </dx:BootstrapGridViewTextColumn>
              <dx:BootstrapGridViewTextColumn FieldName="belongsTo" Name="Podjetje" VisibleIndex="3" Caption="Podjetje" >
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



	<div class="column" id="companyForm" tabindex="0" >

        <div class="companyPart">
             <hr style="color: black;" />



                   <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Naziv podjetja</label>
           <asp:TextBox ID="companyName" runat="server" placeholder="Ime" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                       </div>


           <br />

                 <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Kontaktna številka</label>
            <asp:TextBox ID="companyNumber" runat="server" placeholder="Kontaktna številka" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>   
                       </div>



                <br />

                 <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Spletna stran</label>
           <asp:TextBox ID="website" runat="server" placeholder="Spletna stran:" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox> 
                       </div>



                <br />
                <div class="form-row">
             <label class="col-sm-2 col-form-label" runat="server" id="labl" visible="false" for="name">Admin</label>
      
            <asp:DropDownList ID="listAdmin" runat="server" Enabled="true" Visible="false"> 
               <%--AppendDataBoundItems="true">--%>  
              </asp:DropDownList> 
                       </div>


           
                <br />

          

       
            </div>
      <div class="connectionPart">
          <br />
     
       
       
    
           <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Data source</label>
           <asp:TextBox ID="dbDataSource" runat="server" placeholder="Data source" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox>
                       </div>
                          <br />

             


        

                 <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Uporabnik</label>
           <asp:TextBox ID="dbUser" runat="server" placeholder="Uporabnik:" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox> 
                       </div>
          <br />
          <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Geslo</label>
           <asp:TextBox ID="dbPassword" runat="server" placeholder="Geslo:" CssClass="form-control" Enabled="true" ClientIDMode="Static"></asp:TextBox> 
                       </div>
          <br />

    
            <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Ime baze</label>
                <asp:TextBox ID="dbNameInstance" runat="server" Enabled="true" placeholder="Ime" Width="500" CssClass="conn" ClientIDMode="Static"></asp:TextBox> 
                       </div>

          <br />

            <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Ime povezave</label>
                <asp:TextBox ID="connName" runat="server" Enabled="true" placeholder="Ime" Width="500" CssClass="conn" ClientIDMode="Static"></asp:TextBox> 
                       </div>
          <br />
       

     
        <br />

        <br />

                       <asp:Button CssClass="btn btn-primary" ID="companyButton" Enabled="true" runat="server" Text="Potrdi" OnClick="companyButton_Click"/> 

                       <button type="button" class="btn btn-primary" onclick="testConnection(); return false;" id="test" style="padding: 3px;">Testiraj</button>

                      <div id="btnsCompany" style="position:absolute;float:right; right:0px;top:0px;">

          <button type="button" class="btn btn-danger"  id="closeCompany" style="padding: 3px;">X</button>
                          </div>
        <br />
        <br />
      
	</div>
	


                    </div>




        <!-- -USER FORM-->





	<div class="column" id="userForm" style="display: none" tabindex="0">
                       
                    <br />
                   <div class ="auth">
                  
                                                <br />
                     
               
                       <hr />
    <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Ime in Priimek</label>
                 <asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" Enabled="true" CssClass="form-control form-control-lg"></asp:TextBox>
          </div>            
                          <br />



     <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Email</label>
                 <asp:TextBox ID="email" runat="server" placeholder="Email" Enabled="true" CssClass="form-control form-control-lg"></asp:TextBox>
          </div>
                    
                      <br />
<div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Uporabniško ime</label>
                        <asp:TextBox ID="TxtUserName" runat="server" Enabled="true" placeholder="Uporabniško ime" CssClass="form-control form-control-lg"></asp:TextBox>  
          </div>
                  
                
               
                 
                      <br />
                       <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Geslo</label>
<asp:TextBox ID="TxtPassword" runat="server"  TextMode="Password" Enabled="true" placeholder="Geslo" CssClass="form-control form-control-lg"></asp:TextBox>           

                       </div>
                 
         
                      <br />
                        <div class="form-row">
             <label class="col-sm-2 col-form-label" for="name">Ponovite geslo</label>
                        <asp:TextBox ID="TxtRePassword" runat="server" Enabled="true" TextMode="Password" placeholder="Geslo še enkrat" CssClass="form-control form-control-lg"></asp:TextBox>  

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

                  
                       <asp:RadioButtonList ID="userRole" runat="server" Enabled="true" RepeatDirection="Horizontal" CellSpacing="8"  CssClass="radio">  
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
             
              
                    <h4 style="text-decoration: solid"Podjetje:</h4>  
                        <asp:DropDownList ID="companiesList" Enabled="true" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  
                    <br />
                    <br />
                 <asp:Button CssClass="btn btn-primary" Enabled="true" ID="registrationButton" runat="server" Text="Potrdi"  OnClick="registrationButton_Click" />


                        <div id="btns" style="position:absolute;float:right; right:0px;top:0px;">

                      <button type="button"  class="btn btn-danger" id="closeUser" style="padding: 3px;">X</button>
                </div>
              </div>
                    <br />
	</div>
  
 
	
</section>

     </div>
     </div> 

 

 
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
                document.getElementById('overlay').style.backgroundColor = "gray";

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
            setCookie("EDIT", "no", 365);
            $("#companyForm").css('display', 'flex');
            var x = document.getElementsByTagName("BODY")[0]
            var elem = document.getElementById("companyForm");

            setTimeout(function () {
                elem.style.opacity = 1;
                document.getElementById('overlay').style.backgroundColor = "gray";
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

