<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="adminsitrationcompany.aspx.cs" Inherits="peptak.adminsitrationcompany" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
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
              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                <tr>
                  <td>
                    <h3>
                    
	<header>
		<center><asp:Label ID="welcome" runat="server" Text="Label" ></asp:Label></center>
	</header>
		
<section class="columns">
	<div class="column">
		  <center><h4>Izberite uporabnika, in odločite katere grafe lahko vidi.</h4></center>
      <hr style="color: black;" />

<center><asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged" ></asp:DropDownList></center>  

       <hr /> 
    <hr />
   <center><DIV style="OVERFLOW-Y:scroll; WIDTH:600px; HEIGHT:500px">
     <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
       </div>
       </center>
        <hr />
    
    <center><asp:Button CssClass="save" type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" OnClick="Save_Click1"/></center>
   
    <hr />
       <hr />
                    </h3>
                  </td>
                </tr>
              </table>
            </asp:View>
            <center><asp:View ID="View2" runat="server">
              <div style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; align-items:center; display:inline-block; font-size:larger;" >
                     <asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" ></asp:TextBox></center>
                   </center>  
              
                <center>  
                
                        <center><asp:TextBox ID="TxtUserName" runat="server" placeholder="Uporabniško ime"></asp:TextBox></center>  
               
               </center>  
              
                 
                       <center> <asp:TextBox ID="TxtPassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo"></asp:TextBox>  </center>
              
                  
                       <center> <asp:TextBox ID="TxtRePassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo še enkrat"></asp:TextBox> </center> 
                 
            <center><h4>Pozicija</h4></center>
                  
                       <center><asp:RadioButtonList ID="userRole" runat="server">  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  </center>
                
    
                <center><h5>Tip uporabnika</h5></center>
                <center><asp:DropDownList ID="userType" autopostback="true" runat="server"  >
                 </asp:DropDownList></center    
        
              
                   <center> <h4>  Podjetje:</h4></center>  
                   
                       <center> <asp:DropDownList ID="companiesList" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  </center>
              
        <hr />
     <center><asp:Button CssClass="registrationButton" ID="registrationButton" runat="server" Text="Potrdi"   OnClick="registrationButton_Click1" /></center>
        <hr />
        <hr /> 
                    </h3>
                  </td>
                </tr>
              </div>
            </asp:View></center>
            <asp:View ID="View3" runat="server">
              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                <tr>
                  <td>
                    <h3>
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
