<%@ Page Title="Administracija" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="peptak.Administration" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
   
       <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
     
   <div id="permisionSettings"> <hr />
    <center><h4>Izberite uporabnika, in odločite katere grafe lahko vidi.</h4></center>
    <hr />
  
  <asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged" ></asp:DropDownList>    

       <hr />
 
    <hr />
   
       
      <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
 


        <hr />
    <div id="save">
          <asp:Button type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" OnClick="Save_Click1" />
   </div>
    <hr />
       <hr />
 </div>
    <div id="registration">
        <div>  
            <table class="style1">  
                <tr>  
                    <center><h4>Registracija novega uporabnika.</h4></center>
                    <td>     Ime in priimek</td>  
                    <td>  
                        <asp:TextBox ID="TxtName" runat="server"></asp:TextBox>  
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
                            <asp:ListItem>Manager</asp:ListItem>  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  
                    </td> 
           
                <tr>  
                   <center><td>     Podjetje:</td></center>  
                    <td>  
                        <asp:DropDownList ID="companiesList" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  
                    </td>  
                </tr>  
            </table>  
        </div>  
        <asp:Button ID="registrationButton" runat="server" Text="Potrdi" OnClick="Button1_Click"/>  

        <hr />
        <hr />


    </div>
    <div>

    </div>
    </asp:Content>
