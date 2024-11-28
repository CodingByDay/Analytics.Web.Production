<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Dash.Logon" EnableEventValidation="false"%>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v24.1, Version=24.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<!DOCTYPE html>
<meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0"/>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
       <link href="assets/img/favicon.ico" rel="icon">
        <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">
    <link href="~/Content/Css/Logon.css" rel="stylesheet" type="text/css" />

    <title>Dash</title>
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
         @media (max-width: 767px) {
             #formContent {
                 margin-top:-70%;
             }
             input[type=button], input[type=submit], input[type=reset] {

                padding: 10px;
            
            }

         }
         


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
    background-color: rgba(20, 46, 108, 0.9);
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
     .pwd-rest {
    display: flex;
    justify-content: center;
    gap: 1em;
    margin-top: 1em;
}
    </style>
   
 
    <div class="wrapper fadeInDown">
        <div id="formContent">
            <form id="form1" runat="server">
                <div>
                    <div class="formTitle">Dash<br /><span class="formSubTitle">Analytics by In.SIST d.o.o.</span></div>

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

                        <div class="pwd-rest">
                            <img src="images/pass.png" />

                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ResetPassword.aspx" >

                                         Forgot your password?

                            </asp:HyperLink>
                        </div>

                        <asp:Button ID="cmdLogin" CssClass="login" runat="server" Text="Login" type="submit" OnClick="Login_Click" />

                        <center>

                         <div id="database" runat="server" visible="false">


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
