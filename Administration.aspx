<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="peptak.Administration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

   
   
       <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
     
    <hr />
    <center><h1>Izberite uporabnika, in se odločite katere grafe lahko vidi.</h1></center>
    <hr />
    <div align="center">
    <asp:ListBox  ID="users" runat="server" Height="162px" Width="327px"></asp:ListBox>
    </div>

       <hr />
    <center><h1 id="test">Izberite grafe katere lahko vidi.</h1></center>
    <hr />
    <div align="center" id="graphBox">
    <asp:ListBox  ID="graphs" runat="server" Height="162px" Width="700px" SelectionMode="Multiple"></asp:ListBox>

        <hr />
           <center><asp:Button type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" /></center>
    </div>
    
 
    



    </asp:Content>
