<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="adminsitrationcompany.aspx.cs" Inherits="Dash.adminsitrationcompany" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
      <link rel="stylesheet" href="css/bootstrap.css" />
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css" />
	<link rel="stylesheet" href="css/all.css" />
	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' /> 

     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

  <script src="https://js.stripe.com/v3/"></script>
     <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
 <body style="font-family: tahoma">
    <div id="tabs" width="80%" align="center";>
      <tr>
        <td>
          <asp:Button Text="Pravice" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
              OnClick="Tab1_Click" />
          <asp:Button Text="Registracija uporabnikov" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" />
          <asp:Button Text="Brisanje" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
              OnClick="Tab3_Click" />
          <asp:MultiView ID="MainView" runat="server">
            <asp:View ID="View1" runat="server">
              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; background-color: #c5d5cb">
                <tr>
                  <td>
                    <h3>
                    
	<header>
		<center><asp:Label ID="welcome" runat="server" Text="Label" ></asp:Label></center>
	</header>
		
<section class="columns">
	<div class="column">
		  <center><h4 style="font-style: italic;font-weight: bold">Izberite uporabnika, in odločite katere grafe lahko vidi.</h4></center>
      <hr style="color: black;" />

<center><asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged" ></asp:DropDownList></center>  

       <hr /> 
    <hr />
   <center><DIV style="OVERFLOW-Y:scroll; WIDTH:600px; HEIGHT:500px">
     <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
       </div>
       </center>
    
    <center><asp:Button CssClass="btn btn-primary" type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" OnClick="Save_Click1"/></center>
   
  
                    </h3>
                  </td>
                </tr>
              </table>
            </asp:View>
            <center><asp:View ID="View2" runat="server">
              <div style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; align-items:center; display:inline-block; font-size:larger;background-color: #c5d5cb" >
                                           <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija/sprememba uporabnika</h3></center>
                  <br />
                     <center><asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" CssClass="form-control form-control-lg"></asp:TextBox></center>
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
                                     TextMode="Password" placeholder="Geslo" CssClass="form-control"></asp:TextBox>  </center>
              
                                    <br />

                       <center> <asp:TextBox ID="TxtRePassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo še enkrat" CssClass="form-control form-control-lg"></asp:TextBox> </center> 
                                   <br />

            <center><h4>Pozicija</h4></center>
                  
                       <center><asp:RadioButtonList ID="userRole" runat="server">  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  </center>
                                  <br />

    
<center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Tip uporabnika.</h3></center>    
                <center><asp:DropDownList ID="userType" autopostback="true" runat="server"  >
                 </asp:DropDownList></center    
        
                                <br />

                   <center> <h4>  Podjetje:</h4></center>  
                   
                       <center> <asp:DropDownList ID="companiesList" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  </center>
                                <br />

     <center><asp:Button CssClass="btn btn-primary" ID="registrationButton" runat="server" Text="Potrdi"   OnClick="registrationButton_Click1" /></center>
                        <br />

                    </h3>
                  </td>
                </tr>
              </div>
            </asp:View></center>
            <asp:View ID="View3" runat="server">
              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; background-color: #c5d5cb">
                <tr>
                  <td>
                    <h3>
                     	 <center><h2 style="font-style: italic;font-weight: bold">Brišite uporabnika.</h2></center>
      <hr />
        <center><asp:DropDownList ID="DeleteUser" runat="server"  
               AppendDataBoundItems="true">  
        </asp:DropDownList></center>
       <center> <asp:Button CssClass="btn btn-primary" ID="delete" runat="server" Text="Briši" OnClick="delete_Click"/></center>
                        <br />
       

      <hr style="color: black;" />
	</div>
	
</section>	
	
	<footer>
		<h3></h3>
	</footer>
                    </h3>
                  </td>
                </tr>
              </table>
            </asp:View>
          </asp:MultiView>
        </td>
      </tr>
    </table>
</body>


</asp:Content>
