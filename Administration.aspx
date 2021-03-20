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
  
  <asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged"></asp:DropDownList>    

       <hr />
 
    <hr />
   
       
      <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
 


        <hr />
    <div id="save">
          <asp:Button type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" />
   </div>
    <hr />
       <hr />
 </div>
    



    </asp:Content>
