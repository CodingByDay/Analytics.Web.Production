<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="peptak.Administration" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
   
       <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
     
    <hr />
    <center><h4>Izberite uporabnika, in se odločite katere grafe lahko vidi.</h4></center>
    <hr />
    <div align="center">
    <asp:DropDownList ID="usersPermisions" runat="server"></asp:DropDownList>    </div>

       <hr />
    <center><h1 id="test">Izberite grafe katere lahko vidi.</h1></center>
    <hr />
    <div align="center" id="graphBox">
       
        <dx:ASPxCheckBoxList ID="graphsFinal" runat="server" ValueType="System.String">
        </dx:ASPxCheckBoxList>
 


        <hr />
           <center><asp:Button type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" /></center>
    </div>
    
 
    



    </asp:Content>
