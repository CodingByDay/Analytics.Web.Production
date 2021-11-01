<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membership.aspx.cs" Inherits="Dash.membership" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/css/custom.css" rel="stylesheet" type="text/css" />
    <script src="https://js.stripe.com/v3/"></script>
	
	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
    <title></title>
</head>
<body>
    <div class="wrapper fadeInDown">
        <div id="formContent">
            <form id="membershipForm" runat="server">
                <div>
                    <div class="formTitle">Graphs<br /><span class="formSubTitle">Analytics by In.SIST d.o.o.</span></div>

             <h1>Stripe</h1>


     
               <p>

                   Placeholder for now.

               </p>
    
                    <button type="submit">Naročite</button>


                </div>
            </form>

        </div>
    </div>
    <script>
        var stripe = Stripe('pk_test_51IdsgFI2g0bX7R9apvkiAeJcdqLg64K7BT4GYLbdrgDNV1nYHRqkiw8MAScyKKzklbyKqzPCaMkXOSyS9L9jT9fP00LvSJIMcK');
        var form = document.getElementById("membershipForm");


        form.addEventListener('submit', function (e) {
            e.preventDefault();

            stripe.redirectToCheckout({

                sessionId: "<%= sessionId %>"
            }); 




        })





    </script>
    <script src="https://js.stripe.com/v3/"></script>
</body>
</html>
