<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Dash.ResetPassword" %>


<!DOCTYPE html>
<html>
   <head>
      <title>Pozabljeno geslo</title>
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
            background-image: url('assets/img/hero-bg.jpg'); /* Change the path to your image */
            background-size: cover; /* This makes sure the image covers the entire background */
            background-position: center; /* Centers the image */
            background-repeat: no-repeat; /* Prevents the image from repeating */
            min-height: 100vh; /* Ensures the body takes up the full height of the viewport */
        }
         .btn {
         background-color: #17c0eb;
         width: 100%;
         color: #fff;
         padding: 10px;
         font-size: 18px;
         }
         .btn:hover {
            background-color: #39ace7;
            color: #fff;
        }
         input {
         height: 50px !important;
         }
         .form-control:focus {
         border-color: #18dcff;
         box-shadow: none;
         }
         h3 {
         color: #17c0eb;
         font-size: 36px;
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
         }
         @media(max-width: 492px) {
         .cw {
         width: 90%;
         }
         }
      </style>
   </head>
   <body>
           <div class="wrapper fadeInDown">

      <div class="container d-flex justify-content-center align-items-center vh-100">
         <div class="bg-white text-center p-5 mt-3 center">
            <h3>Pozabiljeno geslo? </h3>
            <form class="pb-3" action="#" runat="server">
               <div class="form-group">
                   <asp:TextBox ID="username" runat="server" placeholder="Uporabniško ime" CssClass="form-control form-control-lg"></asp:TextBox>

               </div>
           
          <asp:Button ID="reset" runat="server" Text="Pošlji povezavo" type="submit" OnClick="reset_Click" CssClass="btn" />
                <br />
                <br />
                <hr />
          <asp:Button ID="backButton" runat="server" Text="Nazaj" type="submit"  OnClick="backButton_Click" CssClass="btn" />

            </form>
             </div>
         </div>
      </div>
   </body>
</html>