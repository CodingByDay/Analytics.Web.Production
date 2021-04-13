<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="peptak.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
    <div class="title">Registration</div>
    <div class="content">
                   
 #">
        <div class="user-details">
          <div class="input-box">
            <span class="details">Ime podjetja</span>
            <asp:TextBox ID="CompanyName" runat="server" placeholder="Ime podjetja"></asp:TextBox>
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
              <asp:TextBox ID="TextBox1" runat="server" placeholder="Ponovite Password"></asp:TextBox>
          </div>
        </div>
        <div class="gender-details">
          <input type="radio" name="gender" id="dot-1">
          <input type="radio" name="gender" id="dot-2">
          <input type="radio" name="gender" id="dot-3">
          <span class="gender-title">Gender</span>
          <div class="category">
            <label for="dot-1">
            <span class="dot one"></span>
            <span class="gender">Male</span>
          </label>
          <label for="dot-2">
            <span class="dot two"></span>
            <span class="gender">Female</span>
          </label>
          <label for="dot-3">
            <span class="dot three"></span>
            <span class="gender">Prefer not to say</span>
            </label>
          </div>
        </div>
        <div class="button">
                <asp:Button ID="Register" runat="server" Text="Prijava" type="submit"  OnClick="Register_Click1" />
        </div>
 
    </div>
  </div>

    </form>
</body>
</html>
