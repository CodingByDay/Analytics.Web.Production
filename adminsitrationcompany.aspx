<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="adminsitrationcompany.aspx.cs" Inherits="peptak.adminsitrationcompany" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
   
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
     <div class='app-layout'>
  <div class='box tweets'>  <center><h4>Izberite uporabnika, in odločite katere grafe lahko vidi.</h4></center>
      <hr style="color: black;" />
  
  <center><asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged" ></asp:DropDownList></center>    

       <hr />
 
    <hr />
   
       
      <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
 


        <hr />
    
        <center><asp:Button CssClass="save" type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" OnClick="Save_Click" /></center>
   
    <hr />
       <hr /></div>




 





    </div>
</asp:Content>
