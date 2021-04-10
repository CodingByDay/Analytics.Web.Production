<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="peptak.ResetPassword" %>


<!DOCTYPE html>
<html>
   <head>
      <title>Resetiraj geslo.</title>
      <meta name="viewport" content="width=device-width, initial-scale=1">
      <link href="https://fonts.googleapis.com/css?family=Nunito+Sans:300i,400,700&display=swap" rel="stylesheet">
      <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css">
      <style>
         body {
         background-color: #17c0eb;
         font-family: Nunito Sans;
         }
         .btn {
         background-color: #17c0eb;
         width: 100%;
         color: #fff;
         padding: 10px;
         font-size: 18px;
         }
         .btn:hover {
         background-color: #2d3436;
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
            <h3>Pozabili geslo? </h3>
            <p>Če ste pozabili geslo, lahko ga resetirate.</p>
            <form class="pb-3" action="#" runat="server">
               <div class="form-group">
                   <asp:TextBox ID="username" runat="server" placeholder="Uporabniško ime" CssClass="form-control form-control-lg"></asp:TextBox>

               </div>
           
          <asp:Button ID="reset" runat="server" Text="Resetiraj" type="submit" OnClick="reset_Click" CssClass="btn" />
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