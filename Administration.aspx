<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="peptak.Administration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
   

    <hr />
    <center><h1>Izberite uporabnika, in se odločite katere grafe lahko vidi.</h1></center>
    <hr />
    <div align="center">
    <asp:ListBox  ID="users" runat="server" Height="162px" Width="327px"></asp:ListBox>
    </div>

       <hr />
    <center><h1>Izberite uporabnika, in se odločite katere grafe lahko vidi.</h1></center>
    <hr />
    <div align="center">
    <asp:ListBox  ID="ListBox1" runat="server" Height="162px" Width="327px"></asp:ListBox>
    </div>

    <center><asp:Button type="submit" Value="Save" runat="server" ID="save" OnClick="save_Click" Text="Shrani" /></center>
    



    </asp:Content>
