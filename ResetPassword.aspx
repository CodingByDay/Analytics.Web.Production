<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Dash.ResetPassword" %>

<!DOCTYPE html>
<html>
   <head>
      <title>Dash</title>
      <meta name="viewport" content="width=device-width, initial-scale=1">
      <link href="https://fonts.googleapis.com/css?family=Nunito+Sans:300i,400,700&display=swap" rel="stylesheet">
      <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css">
      <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
      <!-- Favicons -->
      <link href="assets/img/favicon.ico" rel="icon">
      <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">
      <link rel="shortcut icon" href="images/logo.png">
      <meta name="msapplication-TileColor" content="#603cba">
      <meta name="msapplication-TileImage" content="images/logo.png">

      <style>
            body {
                background-color: rgba(20, 46, 108, 0.9);
                background-size: cover;
                background-position: center;
                background-repeat: no-repeat;
                min-height: 100vh;
                font-family: 'Nunito Sans', sans-serif;
            }

            .btn-reset {
                background-color: rgba(20, 46, 108, 0.9);
                color: #fff;
                font-size: 16px; /* Reduced font size */
                border: none;
                border-radius: 5px; /* Added border radius for a modern look */
                width: 100%;
                transition: background-color 0.3s ease;
                padding-top:5px;
                padding-bottom:5px;
            }

            .btn:hover {
            }

            .bg-white.text-center.p-5.mt-3.center {
                border-radius: 8px;
                box-shadow: 0px 0px 15px rgba(0, 0, 0, 0.1);
            }

            input {
                font-size: 12px; /* Smaller font size */
            }

            .form-control:focus {
                border-color: #18dcff;
                box-shadow: none;
            }

            h3 {
                color: #17c0eb;
                font-size: 32px; /* Smaller heading size for a more balanced layout */
                margin-bottom: 20px;
            }

            .cw {
                width: 35%;
            }

            @media(max-width: 992px) {
                .cw {
                    width: 60%;
                }
            }

            @media(max-width: 768px) {
                .cw {
                    width: 80%;
                }

                h3 {
                    font-size: 24px;
                }
            }

            @media(max-width: 492px) {
                .cw {
                    width: 90%;
                }

                h3 {
                    font-size: 20px;
                }

                .bg-white.text-center.p-5.mt-3.center {
                    margin-top: -10em!important;
                }
            }

            .form-group {
                margin-bottom: 15px; /* Added space between form fields */
            }

            .form-control {
                margin-bottom: 15px; /* Added spacing below the input field */
            }
        </style>
   </head>
   <body>
      <div class="wrapper fadeInDown">
         <div class="container d-flex justify-content-center align-items-center vh-100">
            <div class="bg-white text-center p-5 mt-3 center">
               <form class="pb-3" action="#" runat="server">
                  <div class="form-group">
                     <asp:TextBox ID="username" runat="server" placeholder="Username" CssClass="form-control"></asp:TextBox>
                  </div>
                  <asp:Button ID="ResetButton" runat="server" Text="Send" type="submit" OnClick="reset_Click" CssClass="btn-reset" />
                  <br />
                  <br />
                  <hr />
                  <asp:Button ID="BackButton" runat="server" Text="Back" type="submit" OnClick="backButton_Click" CssClass="btn-reset" />
               </form>
            </div>
         </div>
      </div>
   </body>
</html>
