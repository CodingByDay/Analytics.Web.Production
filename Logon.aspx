<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="peptak.Logon" EnableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/css/custom.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <center><img src="5f31883b507a4399a9cafe7bd10c269c.png" /></center>
    <div class="wrapper fadeInDown">
        <div id="formContent">
            <form id="form1" runat="server">
                <div>
                    <div class="formTitle">Graphs<br /><span class="formSubTitle">Analytics by In.SIST d.o.o.</span></div>

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
                        <img src="pass.png" /><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ResetPassword.aspx" >
                      Pozabili ste geslo?
                     </asp:HyperLink>
                            <asp:Button ID="cmdLogin" runat="server" Text="Prijava" type="submit" OnClick="cmdLogin_Click" />

                    </div>
                    </p>
                    <asp:Label ID="lblMsg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" />

                                 <img src="stripe.png" width="300px" alt="Powered by Stripe"/><asp:Button  ID="membership" runat="server" Text="Vrste plačila" type="submit"  OnClick="membership_Click" CausesValidation="false"/>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/AdminPanelCompany.aspx" > Testing
                        </asp:HyperLink>
                </div>
            </form>

        </div>
    </div>
</body>
</html>
