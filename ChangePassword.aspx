<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Dash.ChangePassword" %>

<!DOCTYPE html>
<html>
   <head>
      <title>Resetirajte geslo.</title>
      <meta name="viewport" content="width=device-width, initial-scale=1">

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
            <h3>Spremeni geslo. </h3>
            <form class="pb-3" action="#" runat="server">
               <div class="form-group">
                   
                       <center> <asp:TextBox ID="pwd" runat="server"  
                                     TextMode="Password" placeholder="Geslo" CssClass="form-control form-control-lg"></asp:TextBox>  </center>
                   
                       <center> <asp:TextBox ID="REpwd" runat="server"  
                                     TextMode="Password" placeholder="Še enkrat" CssClass="form-control form-control-lg"></asp:TextBox>  </center>
             

               </div>
           
          <asp:Button ID="change" runat="server" Text="Spremeni" type="submit" OnClick="change_Click" CssClass="btn" />
                <br />
                <br />
          <asp:Button ID="backButton" runat="server" Text="Začetna strana" type="submit"  OnClick="backButton_Click" CssClass="btn" />

            </form>
             </div>
         </div>
      </div>
   </body>
</html>
