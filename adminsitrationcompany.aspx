<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="adminsitrationcompany.aspx.cs" Inherits="peptak.adminsitrationcompany" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
     <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
 <div class="wrapper">

	<header>
		<center><asp:Label ID="welcome" runat="server" Text="Label" ></asp:Label></center>
	</header>
		
<section class="columns">
	<div class="column">
		  <center><h4>Izberite uporabnika, in odločite katere grafe lahko vidi.</h4></center>
      <hr style="color: black;" />

<asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged" ></asp:DropDownList>  

       <hr /> 
    <hr />
   <DIV style="OVERFLOW-Y:scroll; WIDTH:600px; HEIGHT:500px">
     <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
       </div>
        <hr />
    
    <center></center><asp:Button CssClass="save" type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" OnClick="Save_Click1"/>
   
    <hr />
       <hr />
	</div>
	
	<div class="column">
		 <div class='box replies'><table class="style1">  
                <tr>  
      <center><h4>Registracija novega uporabnika.</h4></center>
                 
      <center><asp:Button CssClass="completelyNewUser" ID="completelyNewUser" runat="server" Text="Novi uporabnik" OnClick="completelyNewUser_Click" /></center>  

      <hr style="color: black;" />
                    <td>     Ime in priimek</td>  
                    <td>  
                        <asp:TextBox ID="TxtName" placeholder= runat="server" ></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <center><td>     Uporabniško ime:</td></center>  
                    <td>  
                        <asp:TextBox ID="TxtUserName" runat="server"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <center><td>     Geslo:</td></center>  
                    <td>  
                        <asp:TextBox ID="TxtPassword" runat="server"  
                                     TextMode="Password"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <center><td>     Še enkrat:</td></center>  
                    <td>  
                        <asp:TextBox ID="TxtRePassword" runat="server"  
                                     TextMode="Password"></asp:TextBox>  
                    </td>  
                </tr>  
                 <td>Pozicija</td>  
                    <td>  
                        <asp:RadioButtonList ID="userRole" runat="server">  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  
                    </td> 
      <hr />
                <center><h5>Tip uporabnika</h5></center>
                 <asp:DropDownList ID="userType" autopostback="true" runat="server"  >
                 </asp:DropDownList>    
        
                <tr>  
                   <center><td>     Podjetje:</td></center>  
                    <td>  
                        <asp:DropDownList ID="companiesList" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  
                    </td>  
                </tr>  
            </table>  
        <hr />
     <asp:Button CssClass="registrationButton" ID="registrationButton" runat="server" Text="Potrdi"   OnClick="registrationButton_Click1"/>
        <hr />
        <hr />  </div>

	</div>
  

  <div class="column">
		 <center><h4>Brišite uporabnika.</h4></center>
      <hr />
        <center><asp:DropDownList ID="DeleteUser" runat="server"  
               AppendDataBoundItems="true">  
        </asp:DropDownList></center>
      <hr />
       <center> <asp:Button CssClass="delete" ID="delete" runat="server" Text="Briši" OnClick="delete_Click"/></center>
       

      <hr style="color: black;" />
	</div>
	
</section>	
	
	<footer>
		<h3></h3>
		<center><p>Dashboards @2021</p></center>
	</footer>

</div>
</asp:Content>
