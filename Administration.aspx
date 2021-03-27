<%@ Page Title="Administracija" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="peptak.Administration" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
   
       <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
     <div class='app-layout'>
  <div class='box tweets'>  <center><h4>Izberite uporabnika, in odločite katere grafe lahko vidi.</h4></center>
    <hr />
  
  <asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged" ></asp:DropDownList>    

       <hr />
 
    <hr />
   
       
      <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
 


        <hr />
    
         <asp:Button type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" OnClick="Save_Click1" />
   
    <hr />
       <hr /></div>




  <div class='box replies'>            <table class="style1">  
                <tr>  
                    <center><h4>Registracija novega uporabnika.</h4></center>
                    <hr />
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
        
       <asp:Button ID="registrationButton" runat="server" Text="Potrdi" OnClick="Button1_Click"/>  

        <hr />
        <hr />  </div>




  <div class='box search'> 
      <center><h4>Registracija novega podjetja.</h4></center>  
      <hr />
             <h5>Ime:</h5> <asp:TextBox ID="companyName" runat="server"></asp:TextBox>
             <h5>Številka</h5> <asp:TextBox ID="companyNumber" runat="server"></asp:TextBox>                  
              <h5>Website podjetja:</h5><asp:TextBox ID="website" runat="server"></asp:TextBox>  
             <h5>Admin</h5> <asp:DropDownList ID="listAdmin" runat="server"  
               AppendDataBoundItems="true">  
              </asp:DropDownList>  
              <h5>Baza</h5><asp:DropDownList ID="ConnectionStrings" runat="server"  
               AppendDataBoundItems="true">  
              </asp:DropDownList>  
              <asp:Button ID="companyButton" runat="server" Text="Potrdi" OnClick="companyButton_Click"/>  
</div>



  <div class='box messages'> <center><h4>Obrišite uporabnika.</h4></center>
      <hr />
        <asp:DropDownList ID="DeleteUser" runat="server"  
               AppendDataBoundItems="true">  
        </asp:DropDownList>
        <asp:Button ID="delete" runat="server" Text="Obriši" OnClick="delete_Click"/>




      
     <center><h4>Izbrišite podjetje.</h4></center>

           <asp:DropDownList ID="deleteCompany" runat="server"  
               AppendDataBoundItems="true">  
        </asp:DropDownList>
        <asp:Button ID="companyDelete" runat="server" Text="Obriši podjetje"  OnClick="deleteCompanyButton_Click"/>
  </div>

</div>
 
</asp:Content>
