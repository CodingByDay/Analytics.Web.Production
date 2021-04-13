<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="peptak.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
        <webopt:bundlereference runat="server" path="~/css/success.css" />
<link href= "~/css/success.css" rel="stylesheet" runat="server" type="text/css" />
<body>
    <form id="form1" runat="server">
        <div class="container">
    <div class="title">Regsitracija</div>
    <div class="content">
                   

        <div class="user-details">
          <div class="input-box">
            <span class="details">Full Name</span>
            <asp:TextBox ID="NameForm" runat="server" placeholder="Vaše ime"></asp:TextBox>
          </div>
          <div class="input-box">
            <span class="details">Username</span>
              <asp:TextBox ID="UsernameForm" runat="server" placeholder="Uporabniško ime"></asp:TextBox>
          </div>
          <div class="input-box">
            <span class="details">Email</span>
              <asp:TextBox ID="EmailForm" runat="server" placeholder="Vaš email"></asp:TextBox>
          </div>
       
          <div class="input-box">
            <span class="details">Password</span>
              <asp:TextBox ID="PasswordForm" runat="server" placeholder="Password"></asp:TextBox>
      
          </div>
          <div class="input-box">
            <span class="details">Confirm Password</span>
              <asp:TextBox ID="RePasswordForm" runat="server" placeholder="Ponovite Password"></asp:TextBox>
          </div>
        </div>
      
        <div class="button">
        <asp:Button ID="Register" runat="server" Text="Registracija" type="submit" OnClick="Register_Click" />
        </div>
 
    </div>
  </div>

    </form>
</body>
</html>
