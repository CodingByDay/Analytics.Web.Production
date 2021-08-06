<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="peptak.Logon" EnableEventValidation="false"%>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/css/custom.css" rel="stylesheet" type="text/css" />

    <title></title>
</head>
<body>
 
    <div class="wrapper fadeInDown">
        <div id="formContent">
            <form id="form1" runat="server">
                <div>
                    <div class="formTitle">Analytics<br /><span class="formSubTitle">Analytics by In.SIST d.o.o.</span></div>

                    <input id="txtUserName" type="text" runat="server">
                    <asp:RequiredFieldValidator ControlToValidate="txtUserName"
                        Display="Static" ErrorMessage="*" runat="server"
                        ID="vUserName" />

                    <input id="txtUserPass" type="password" runat="server">
                    <asp:RequiredFieldValidator ControlToValidate="txtUserPass"
                        Display="Static" ErrorMessage="*" runat="server"
                        ID="vUserPass" />
                    <br />

                    <div class="center">

                        <span style="color: #808080;">Zapomni si prijavo</span>

                        <asp:CheckBox ID="chkPersistCookie" runat="server" AutoPostBack="false" />

                        <img src="pass.png" />

                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ResetPassword.aspx" >

                                     Pozabili ste geslo?

                        </asp:HyperLink>


                        <asp:Button ID="cmdLogin" runat="server" Text="Prijava" type="submit" OnClick="cmdLogin_Click" />

                        <center>

                         <div id="database" runat="server" visible="false">

                         <asp:DropDownList ID="databaseList" runat="server" AutoPostBack="true" Width="100px" Height="50px"></asp:DropDownList>

                        </div>

                        </center>
                    </div>
                    
                    <asp:Label ID="lblMsg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" />

                    
                </div>
            </form>

        </div>
    </div>
</body>
</html>
