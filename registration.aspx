<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registration.aspx.cs" Inherits="peptak.registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kreiranje podjetja</title>
<webopt:bundlereference runat="server" path="~/css/success.css" />
<link href= "~/css/success.css" rel="stylesheet" runat="server" type="text/css" />

	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
 
</head>
<body>
       <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
       <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js"></script>
    <!-- multistep form -->
<form id="msform" runat="server">
  <!-- progressbar -->
  <ul id="progressbar">
    <li class="active">Kreiranje uporabnika</li>
    <li>Kreiranje podjetja</li>
    <li>Detalji</li>
  </ul>
  <!-- fieldsets -->
  <fieldset>
    <h2 class="fs-title">Kreirajte profil admina</h2>
    <h3 class="fs-subtitle">Uspešno plačilo!</h3>
    <asp:TextBox ID="UsernameForm" runat="server" placeholder="Uporabniško ime"></asp:TextBox>
    <asp:TextBox ID="PasswordForm" runat="server" placeholder="Geslo" TextMode="Password"></asp:TextBox>
    <asp:TextBox ID="RePasswordForm" runat="server" placeholder="Ponovite geslo" TextMode="Password"></asp:TextBox>
    <input type="button" name="next" class="next action-button" value="Next" />
  </fieldset>
  <fieldset>
    <h2 class="fs-title">Kreiranje podjetja</h2>
    <h3 class="fs-subtitle">Informacije o vašem podjetju</h3>
    <asp:TextBox ID="CompanyName" runat="server" placeholder="Ime podjetja"></asp:TextBox>
    <input type="button" name="previous" class="previous action-button" value="Prejšnja" />
    <input type="button" name="next" class="next action-button" value="Naslednja" />
  </fieldset>
 <fieldset>
    <h2 class="fs-title">Detalji</h2>
    <h3 class="fs-subtitle">o kontaktu</h3>
    <asp:TextBox ID="NameForm" runat="server" placeholder="Ime in priimek"></asp:TextBox>
    <asp:TextBox ID="EmailForm" runat="server" placeholder="Vaš email"></asp:TextBox>
    <asp:TextBox ID="PhoneForm" runat="server" placeholder="Telefon podjetja"></asp:TextBox>
     <asp:TextBox ID="WebsiteForm" runat="server" placeholder="Website podjetja"></asp:TextBox>

   
    <input type="button" name="previous" class="previous action-button" value="Prejšnja" />
      <asp:Button ID="Register" runat="server" Text="Prijava" type="submit"  OnClick="Register_Click" CssClass="previous action-button"/>
  </fieldset>
</form>
    <script>

        //jQuery time
        var current_fs, next_fs, previous_fs; // fieldsets
        var left, opacity, scale; // fieldset properties which we will animate
        var animating; // flag to prevent quick multi-click glitches

        $(".next").click(function () {
            if (animating) return false;
            animating = true;

            current_fs = $(this).parent();
            next_fs = $(this).parent().next();

            //activate next step on progressbar using the index of next_fs
            $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

            //show the next fieldset
            next_fs.show();
            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {
                    //as the opacity of current_fs reduces to 0 - stored in "now"
                    //1. scale current_fs down to 80%
                    scale = 1 - (1 - now) * 0.2;
                    //2. bring next_fs from the right(50%)
                    left = (now * 50) + "%";
                    //3. increase opacity of next_fs to 1 as it moves in
                    opacity = 1 - now;
                    current_fs.css({
                        'transform': 'scale(' + scale + ')',
                        'position': 'absolute'
                    });
                    next_fs.css({ 'left': left, 'opacity': opacity });
                },
                duration: 800,
                complete: function () {
                    current_fs.hide();
                    animating = false;
                },
                //this comes from the custom easing plugin
                easing: 'easeInOutBack'
            });
        });

        $(".previous").click(function () {
            if (animating) return false;
            animating = true;

            current_fs = $(this).parent();
            previous_fs = $(this).parent().prev();

            //de-activate current step on progressbar
            $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

            //show the previous fieldset
            previous_fs.show();
            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {
                    //as the opacity of current_fs reduces to 0 - stored in "now"
                    //1. scale previous_fs from 80% to 100%
                    scale = 0.8 + (1 - now) * 0.2;
                    //2. take current_fs to the right(50%) - from 0%
                    left = ((1 - now) * 50) + "%";
                    //3. increase opacity of previous_fs to 1 as it moves in
                    opacity = 1 - now;
                    current_fs.css({ 'left': left });
                    previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
                },
                duration: 800,
                complete: function () {
                    current_fs.hide();
                    animating = false;
                },
                //this comes from the custom easing plugin
                easing: 'easeInOutBack'
            });
        });

        $(".submit").click(function () {
            return false;
        })

    </script>
</body>
</html>
