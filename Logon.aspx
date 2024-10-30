<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Dash.Logon" EnableEventValidation="false"%>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.2, Version=23.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<!DOCTYPE html>
<meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0"/>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link rel="apple-touch-icon" sizes="57x57" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="60x60" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="72x72" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="76x76" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="114x114" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="120x120" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="144x144" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="152x152" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="180x180" href="images/logo.png">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="32x32">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="194x194">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="96x96">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="192x192">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="16x16">
	<link rel="shortcut icon" href="images/logo.png">
	<meta name="msapplication-TileColor" content="#603cba">
	<meta name="msapplication-TileImage" content="images/logo.png">
    <link href="~/Content/Css/Logon.css" rel="stylesheet" type="text/css" />

    <title></title>
</head>


<body>
    <script>
        function myKeyPress(e) {
            var keynum;

            if (window.event) { // IE                  
                keynum = e.keyCode;
            } else if (e.which) { // Netscape/Firefox/Opera                 
                keynum = e.which;
            }

        }

    </script>
     <style>
         /* Smartphones (portrait and landscape) ----------- */

.login {
    text-align: center!important;
    min-width: 60%!important;
    max-width: 60%!important;
}

     #form1 {
         width: 100%!important;
         height: 100%!important;
     }
     body {
    background-image: url('assets/img/hero-bg.jpg'); /* Change the path to your image */
    background-size: cover; /* This makes sure the image covers the entire background */
    background-position: center; /* Centers the image */
    background-repeat: no-repeat; /* Prevents the image from repeating */
    min-height: 100vh; /* Ensures the body takes up the full height of the viewport */
}

     html, body {
    height: 100%; /* Ensures the body takes full height */
    margin: 0; /* Removes default margin */
    overflow: hidden; /* Prevents scrolling */
}

    </style>
   
 
    <div class="wrapper fadeInDown">
        <div id="formContent">
            <form id="form1" runat="server">
                <div>
                    <div class="formTitle">Analytics<br /><span class="formSubTitle">Analytics by In.SIST d.o.o.</span></div>

                    <input id="txtUserName" type="text" runat="server">
                    <asp:RequiredFieldValidator ControlToValidate="txtUserName"
                        Display="Static" ErrorMessage="*" runat="server"
                        ID="vUserName" />

                    <input id="txtUserPass" type="password" runat="server" onkeydown="return myKeyPress(event);" >
                    <asp:RequiredFieldValidator ControlToValidate="txtUserPass"
                        Display="Static" ErrorMessage="*" runat="server"
                        ID="vUserPass" />
                    <br />

                    <div class="center">

                        <span style="color: #808080;">Zapomni si prijavo</span>

                        <asp:CheckBox ID="chkPersistCookie" runat="server" AutoPostBack="false" />

                        <img src="images/pass.png" />

                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ResetPassword.aspx" >

                                     Pozabili ste geslo?

                        </asp:HyperLink>


                        <asp:Button ID="cmdLogin" CssClass="login" runat="server" Text="Prijava" type="submit" OnClick="Login_Click" />

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
