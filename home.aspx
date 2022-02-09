<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="Dash.home" %>

<!DOCTYPE html>

<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<title>Analytics </title>
	<meta name="description" content="Analytics landing page for the dev express backend.">
	<meta name="keywords" content="website template, css3, one page, bootstrap, app template, web app, start-up">
	<meta name="author" content="IN SIST, trendNET.">
	<link rel="apple-touch-icon" sizes="57x57" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="60x60" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="72x72" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="76x76" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="114x114" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="120x120" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="144x144" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="152x152" href="images/logo.png">
	<link rel="apple-touch-icon" sizes="180x180" href="images/logo.png">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="32x32">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="194x194">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="96x96">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="192x192">
	<link rel="icon" type="image/png" href="images/logo.png" sizes="16x16">
	<link rel="manifest" href="favicons/manifest.json">
	<link rel="shortcut icon" href="images/logo.png">
	<meta name="msapplication-TileColor" content="#603cba">
	<meta name="msapplication-TileImage" content="images/logo.png">
	<meta name="msapplication-config" content="favicons/browserconfig.xml">
	<meta name="theme-color" content="#ffffff">
	<link rel="stylesheet" href="css/bootstrap.css">
	<link rel="stylesheet" href="fonts/font-awesome-4.3.0/css/font-awesome.min.css">
	<link rel="stylesheet" href="css/all.css">
	<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700|Source+Sans+Pro:400,700,400italic,700italic' rel='stylesheet' type='text/css'>
	 <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
</head>
	<!--Start of Tawk.to Script-->
<script type="text/javascript">
    var Tawk_API = Tawk_API || {}, Tawk_LoadStart = new Date();
    (function () {
        var s1 = document.createElement("script"), s0 = document.getElementsByTagName("script")[0];
        s1.async = true;
        s1.src = 'https://embed.tawk.to/61486979d326717cb6826050/1fg1do0lk';
        s1.charset = 'UTF-8';
        s1.setAttribute('crossorigin', '*');
        s0.parentNode.insertBefore(s1, s0);
	})();

	/**
	 * Detects if the user is on mobile. */
    function isMobileDevice() {
        var check = false;
        (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
        return check;
	};
	$(document).ready(function () {
        

		var isMobile = isMobileDevice();
		if (isMobile) {
              window.location.href = 'logon.aspx';
			}
    });
</script>
<!--End of Tawk.to Script-->
<body>
	

	<div id="cookieNotice" class="light display-right" style="display: none;">
    <div id="closeIcon" style="display: none;">
    </div>
    <div class="title-wrap">
        <h4>Cookie Consent</h4>
    </div>
    <div class="content-wrap">
        <div class="msg-wrap">
            <p>This website uses cookies or similar technologies, to enhance your browsing experience and provide personalized recommendations. By continuing to use our website, you agree with this.</p>
            <div class="btn-wrap">
                <button class="btn-primary" onclick="acceptCookieConsent();">Accept</button>
            </div>
        </div>
    </div>
</div>
	<form runat="server">

<div id="wrapper">
	<script>

        
    </script>
	<style>



	 #cookieNotice {

		 min-width:600px!important;

		 max-width:600px!important;

		 position:fixed;

		 background-color: white;
		 left: 10px!important;
		 bottom: 10px!important;
		 z-index: 1;
		 opacity: 1;



	 }



	 .btn {
		 background-color: #236fc4!important; 
	 }

	    .visual {
	        background: white!important;
	    }


		.visual h1 {
			color: #635C73!important;
		}

		#dark {
			width: 850px!important;
			height: 500px!important;
		}

	</style>
		<header id="header">
			<div class="container">
				<div class="logo"><a href="#"><img src="images/logo.png" alt="Analytics" style="top: 0;position: absolute;top: 0px !important; width: 100px;"></a></div>
				<nav id="nav">
					<div class="opener-holder">
						<a href="#" class="nav-opener"><span></span></a>
					</div>
					<asp:Button ID="login" runat="server" Text="Login" CssClass="btn btn-primary rounded" />
					<div class="nav-drop">
						<ul>
							<li class="active visible-sm visible-xs"><a href="#">Home</a></li>
							<li><a href="#">Overview</a></li>
							<li><a href="#cta">About Analytics</a></li>
							<%--<li><a href="<%= Page.ResolveUrl("/registration.aspx") %>">Registration</a></li>--%>
							<li><a href="#footer">Support</a></li>
						</ul>
					
					</div>
				</nav>
			</div>
		</header>
	<section class="visual">
		<div class="container">
			<div class="text-block">
				<div class="heading-holder">
					<h1>Analytics services for business of any size.</h1>
				</div>
				<p class="tagline">A real gamechanger in the world of data consumption.</p>
				<span class="info"></span>
			</div>
		</div>
		<img src="images/dashboard-panel.png" alt="" class="bg-stretch">
	</section>
	<section class="main">
		<div class="container">
			<div id="cta">
				<a href="<%= Page.ResolveUrl("/logon.aspx") %>" class="btn btn-primary rounded">Start your discovery!</a>
			</div>
			<div class="row">
				<div class="text-box col-md-offset-1 col-md-10">
					<h2>Revolutionary software</h2>
					<p>Data = Knowledge. Good data provides indisputable evidence, while anecdotal evidence, assumptions, or abstract observation might lead to wasted resources due to taking action based on an incorrect conclusion. </p>
				</div>
			</div>
		</div>
	</section>
	<section class="area">
		<div class="container">
			<div class="row">
				<div class="col-md-5">
					<h2 class="visible-xs visible-sm text-primary">Here is what you get!</h2>
					<ul class="visual-list">
						<li>
							<div class="img-holder">
								<img src="images/graph-04.svg" width="110" alt="">
							</div>
							<div class="text-holder">
								<h3>Created to Make A You Visualize Better</h3>
								<p>Using our web application you will be able to chart all areas of your business and get the correct picture of your data.</p>
							</div>
						</li>
						<li>
							<div class="img-holder">
								<img class="pull-left" src="images/graph-03.svg" width="90" alt="">
							</div>
							<div class="text-holder">
								<h3>Infinite Customization</h3>
								<p>Our software also offers the ability to customize users of our application within your company, meaning you get access to the administration panel that lets you decide everything about the usage and permisions. </p>
							</div>
						</li>
						<li>
							<div class="img-holder">
								<img src="images/graph-02.svg" height="84" alt="">
							</div>
							<div class="text-holder">
								<h3>Experimental Features</h3>
								<p>We are also developing! Which means you will get access to features as soon as they are fit for production</p>
							</div>
						</li>
						<li>
							<div class="img-holder">
								<img src="images/graph-01.svg" height="71" alt="">
							</div>
							<div class="text-holder">
								<h3>Time-Saving Power Tools</h3>
								<p>You can also choose to safe time by buying some of the dashboards we made and provide your data for it. The designer mode has tools that are made to save you some time.</p>
							</div>
						</li>
					</ul>
				</div>
				<div class="col-md-7">
					<div class="slide-holder">
						<h2 class="hidden-xs hidden-sm text-primary">&lt;Here is what you get&gt;</h2>
						<div class="img-slide scroll-trigger"><img src="images/frame.jpg" height="624" width="1184" alt="" id="dark"></div>
						<!-- Here is where the background goes. -->
					</div>
				</div>
			</div>
		</div>
	</section>
	
	
	<section class="area">
		<div class="container">
			<div class="subscribe">
				<h3>Subscribe to Our Newsletter</h3>
				<form class="form-inline">
					<button type="submit" class="btn btn-primary rounded">Subscribe</button>
					<div class="form-group">
						<input type="email" class="form-control rounded" id="exampleInputEmail2" placeholder="Email...">
						<!--Subscribe to the mailing list. -->
					</div>
				</form>
			</div>
		</div>
	</section>
	
	<footer id="footer">
		<div class="container">
			<div class="footer-holder">
				<div class="row">
					<div class="col-md-4">
						<div class="logo"><a href="#"><img src="images/logo.png" alt="Analytics" style="width:100px!important"></a></div>
						<p><br /> </p>
					</div>
					<div class="col-md-2">
						<h4>Navigation</h4>
						<ul>
							<li><a href="#">Home</a></li>
			
						</ul>
					</div>
					<div class="col-md-3">
						<div class="text-holder">
							<strong class="phone"><a href="tel:3475677890">347 567 78 90</a></strong>
							<span class="available">Available from 7:30 PM - 15:30 PM</span>
							<address>IN SIST d.o.o., Koroška cesta 62b, 3320 Velenje, Slovenia, EU | Tel: +386 35 863 033 | Fax: +386 35 861 970 |</address>
						</div>
					</div>
					<div class="col-md-3">
						<div class="text-frame">
							<h4>Grab your data today!</h4>
							<p></p>
						</div>
					</div>
				</div>
			</div>
		</div>
	</footer>
</div>
<script src="js/jquery-1.11.2.min.js"></script>
<script src="js/bootstrap.js"></script>
<script src="js/jquery.main.js"></script>
</form>


	<!-- Cookie Consent by https://www.FreePrivacyPolicy.com -->
<script type="text/javascript" src="//www.freeprivacypolicy.com/public/cookie-consent/4.0.0/cookie-consent.js" charset="UTF-8"></script>
<script type="text/javascript" charset="UTF-8">
document.addEventListener('DOMContentLoaded', function () {
});
</script>

<!-- End Cookie Consent -->
	<script>
        function setCookie(cname, cvalue, exdays) {
            const d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            let expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        }

        // Delete cookie
        function deleteCookie(cname) {
            const d = new Date();
            d.setTime(d.getTime() + (24 * 60 * 60 * 1000));
            let expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=;" + expires + ";path=/";
        }

        // Read cookie
        function getCookie(cname) {
            let name = cname + "=";
            let decodedCookie = decodeURIComponent(document.cookie);
            let ca = decodedCookie.split(';');
            for (let i = 0; i < ca.length; i++) {
                let c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }

        // Set cookie consent
        function acceptCookieConsent() {
            deleteCookie('user_cookie_consent');
            setCookie('user_cookie_consent', 1, 30);
            document.getElementById("cookieNotice").style.display = "none";
		}




        let cookie_consent = getCookie("user_cookie_consent");
        if (cookie_consent != "") {
            document.getElementById("cookieNotice").style.display = "none";
        } else {
            document.getElementById("cookieNotice").style.display = "block";
        }
    </script>
</body>

</html>