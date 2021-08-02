<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="membership.aspx.cs" Inherits="peptak.membership" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/css/custom.css" rel="stylesheet" type="text/css" />
    <script src="https://js.stripe.com/v3/"></script>
	
	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css' />
    <title></title>
</head>
<body>
    <center><img src="5f31883b507a4399a9cafe7bd10c269c.png"</center>
    <div class="wrapper fadeInDown">
        <div id="formContent">
            <form id="membershipForm" runat="server">
                <div>
                    <div class="formTitle">Graphs<br /><span class="formSubTitle">Analytics by In.SIST d.o.o.</span></div>

             <h1>Stran za placilo narocnine.</h1>


     
               <p>Z uporabo Stripe lahko placate membership za graph aplikacija ki znasa 3.99e/month. V vsakem trenutku lahko prekinete narocnino.
                   
               Kliknite spodaj za checkout.

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
