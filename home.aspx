<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Dash.home" %>


<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="utf-8">
  <meta content="width=device-width, initial-scale=1.0" name="viewport">
  <title>Dash</title>
  <meta name="description" content="">
  <meta name="keywords" content="">

  <!-- Favicons -->
  <link href="assets/img/favicon.ico" rel="icon">
  <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

  <!-- Fonts -->
  <link href="https://fonts.googleapis.com" rel="preconnect">
  <link href="https://fonts.gstatic.com" rel="preconnect" crossorigin>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&family=Raleway:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap" rel="stylesheet">

  <!-- Vendor CSS Files -->
  <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
  <link href="assets/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
  <link href="assets/vendor/aos/aos.css" rel="stylesheet">
  <link href="assets/vendor/swiper/swiper-bundle.min.css" rel="stylesheet">
  <link href="assets/vendor/glightbox/css/glightbox.min.css" rel="stylesheet">

  <!-- Main CSS File -->
  <link href="assets/css/main.css" rel="stylesheet">

 
</head>

<body class="index-page">
    <style>
        /* Cookie Consent Banner Styling */
    .cookie-consent-banner {
        position: fixed;
        bottom: 0;
        left: 0;
        width: 100%;
        background-color: #333;
        color: #fff;
        text-align: center;
        padding: 15px 0;
        font-size: 14px;
        z-index: 9999;
        display: none; /* Hidden by default */
    }

    .cookie-consent-banner .cookie-content {
        max-width: 800px;
        margin: 0 auto;
    }

    .cookie-consent-banner a {
        color: #f1c40f!important;
        text-decoration: none!important;
        cursor:pointer!important;
    }

    .cookie-consent-banner button {
        margin-left: 20px;
        background-color: #f1c40f;
        color: #333;
        border: none;
        padding: 8px 20px;
        font-size: 14px;
        cursor: pointer;
    }

    .cookie-consent-banner button:hover {
        background-color: #e0b60f;
    }
        /* Styling for disabled links */
    .cookie-disabled-link.disabled {
        color: #ccc; /* Light gray color */
        cursor: not-allowed; /* Change cursor to indicate the link is disabled */
        text-decoration: none; /* Remove underline */
    }
</style>
  <header id="header" class="header d-flex align-items-center fixed-top">
    <div class="container-fluid container-xl position-relative d-flex align-items-center justify-content-between">

      <a href="<%= Page.ResolveUrl("/Home.aspx") %>" class="logo d-flex align-items-center">
        <!-- Uncomment the line below if you also wish to use an image logo -->
        <!-- <img src="assets/img/logo.png" alt=""> -->
        <h1 class="sitename">Dash</h1>
      </a>

      <nav id="navmenu" class="navmenu">
        <ul>
          <li><a href="#hero" class="active">Home</a></li>
          <li><a href="#featured">Features</a></li>
          <li><a href="#contact">About us</a></li>
          <li><a href="<%= Page.ResolveUrl("/Logon.aspx") %>" id="logInLink" class="cookie-disabled-link">Log in</a></li>
        </ul>
        <i class="mobile-nav-toggle d-xl-none bi bi-list"></i>
      </nav>

    </div>
  </header>

  <main class="main">

    <!-- Hero Section -->
    <section id="hero" class="hero section dark-background">

   <div class="container">
      <div class="row gy-4">
        <div class="col-lg-4 order-lg-last hero-img d-flex align-items-center justify-content-center" data-aos="zoom-out">
          <img src="assets/img/devices.png" alt="Naprave" style="width: 90%; height: auto;" class="devices-img">
        </div>
        <div class="col-lg-8 d-flex flex-column justify-content-center align-items text-center text-md-start" data-aos="fade-up">
          <h2>Instant Insights at Your Fingertips</h2>
          <p>Make informed decisions with real-time analytics and dashboards tailored to your needs.</p>
          <div class="d-flex mt-4 justify-content-center justify-content-md-start">
            <a onclick="window.open('App_Data/Administracija.pdf', '_blank', 'fullscreen=yes'); return false;"  class="download-btn"><i class="bi bi-book"></i> <span>Documentation</span></a>
          </div>
        </div>
      </div>
    </div>


    </section><!-- /Hero Section -->

   

    <!-- Featured Section -->
    <section id="featured" class="featured section">

      <!-- Section Title -->
      <div class="container section-title" data-aos="fade-up">
        <h2>Empower Your Business with Dash</h2>
        <p>Take control of your analytics and drive success. With Dash’s comprehensive features, you’ll unlock new opportunities and insights that propel your business forward.</p>
      </div><!-- End Section Title -->

      <div class="container">

        <div class="row gy-4" data-aos="fade-up" data-aos-delay="100">

          <div class="col-md-4">
            <div class="card">
              <div class="img">
                <img src="assets/img/cards-4.png" alt="" class="img-fluid">
                <div class="icon"><i class="bi bi-hdd-stack"></i></div>
              </div>
              <h2 class="title">Collaborate Seamlessly</h2>
              <p>
                Work together in real time with shared dashboards and collaborative features that enhance teamwork.
              </p>
            </div>
          </div><!-- End Card Item -->

          <div class="col-md-4" data-aos="fade-up" data-aos-delay="200">
            <div class="card">
              <div class="img">
                <img src="assets/img/cards-2.png" alt="" class="img-fluid">
                <div class="icon"><i class="bi bi-brightness-high"></i></div>
              </div>
              <h2 class="title">Interactive Data Exploration</h2>
              <p>
                 Dive deep into your data with interactive filters and drill-down capabilities.
              </p>
            </div>
          </div><!-- End Card Item -->

          <div class="col-md-4" data-aos="fade-up" data-aos-delay="300">
            <div class="card">
              <div class="img">
                <img src="assets/img/cards-6.png" alt="" class="img-fluid">
                <div class="icon"><i class="bi bi-calendar4-week"></i></div>
              </div>
              <h2 class="title">Tailored User Experience</h2>
              <p>
               Create a personalized dashboard that reflects your priorities and data needs.
              </p>
            </div>
          </div><!-- End Card Item -->

        </div>

      </div>

    </section><!-- /Featured Section -->




  
<!-- Contact Section -->
<section id="contact" class="contact section">

  <!-- Section Title -->
  <div class="container section-title" data-aos="fade-up">
    <h2>Contact</h2>
  </div><!-- End Section Title -->

  <div class="container" data-aos="fade" data-aos-delay="100">

    <div class="row gy-4">

      <div class="col-lg-4">
        <div class="info-item d-flex" data-aos="fade-up" data-aos-delay="200">
          <i class="bi bi-geo-alt flex-shrink-0"></i>
          <div>
            <h3>Address</h3>
            <p>Koroska cesta 62b, Velenje</p>
          </div>
        </div><!-- End Info Item -->

        <div class="info-item d-flex" data-aos="fade-up" data-aos-delay="300">
          <i class="bi bi-telephone flex-shrink-0"></i>
          <div>
            <h3>Call Us</h3>
            <p>+386 35 863 033</p>
          </div>
        </div><!-- End Info Item -->

        <div class="info-item d-flex" data-aos="fade-up" data-aos-delay="400">
          <i class="bi bi-envelope flex-shrink-0"></i>
          <div>
            <h3>Email Us</h3>
            <p>developer@in-sist.si</p>
          </div>
        </div><!-- End Info Item -->
      </div>

      <div class="col-lg-8">
        <!-- Google Map Embed -->
        <div class="map-responsive" data-aos="fade-up" data-aos-delay="200">
          <iframe
            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2943.106365159162!2d15.103065415395215!3d46.42851027914084!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x4776b51c9f63743d%3A0x40ae8c4a65a32c89!2sKoroska%20cesta%2062b%2C%20Velenje%2C%20Slovenia!5e0!3m2!1sen!2suk!4v1635511234567!5m2!1sen!2suk"
            width="100%" height="400" style="border:0;" allowfullscreen="" loading="lazy"></iframe>
        </div>
      </div><!-- End Google Map Embed -->

    </div>

  </div>

</section><!-- End Contact Section -->

  </main>

  <footer id="footer" class="footer dark-background">
    <div class="container">
      <h3 class="sitename">Dash</h3>
      <p>Leverage cutting-edge components that seamlessly integrate with your workflow. Dash’s modern design ensures an efficient and enjoyable user experience, empowering your team to achieve more.</p>
      <div class="social-links d-flex justify-content-center">

      </div>
      <div class="container">
        <div class="copyright">
          <span>Copyright</span> <strong class="px-1 sitename">In.Sist d.o.o.</strong> <span>All Rights Reserved</span>
        </div>

      </div>
    </div>
  </footer>



                    <!-- Cookie Consent Banner -->
                <div id="cookieConsent" class="cookie-consent-banner" style="display:none;">
                    <div class="cookie-content">
                        <p>We use cookies to ensure you get the best experience on our website. By continuing, you agree to our use of cookies. 
                            <a onclick="window.open('App_Data/Privacy Policy.pdf', '_blank', 'fullscreen=yes'); return false;">Learn more</a>
                        </p>
                        <button id="acceptCookiesBtn">Accept</button>
                    </div>
                </div>




  <!-- Scroll Top -->
  <a href="#" id="scroll-top" class="scroll-top d-flex align-items-center justify-content-center"><i class="bi bi-arrow-up-short"></i></a>

  <!-- Preloader -->
  <div id="preloader"></div>

  <!-- Vendor JS Files -->
  <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
  <script src="assets/vendor/php-email-form/validate.js"></script>
  <script src="assets/vendor/aos/aos.js"></script>
  <script src="assets/vendor/swiper/swiper-bundle.min.js"></script>
  <script src="assets/vendor/glightbox/js/glightbox.min.js"></script>

  <!-- Main JS File -->
  <script src="assets/js/main.js"></script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            // Check if cookies have been accepted
            if (!getCookie("cookieConsent")) {
                // Show the cookie consent banner if cookies are not accepted
                document.getElementById("cookieConsent").style.display = "block";

                // Disable the log in link if cookies are not accepted
                var logInLink = document.getElementById("logInLink");
                if (logInLink) {
                    logInLink.classList.add("disabled"); // Add disabled class
                    logInLink.style.pointerEvents = "none"; // Prevent clicks
                }
            } else {
                // Enable the login link if cookies are accepted
                var logInLink = document.getElementById("logInLink");
                if (logInLink) {
                    logInLink.classList.remove("disabled"); // Remove disabled class
                    logInLink.style.pointerEvents = "auto"; // Enable clicks
                }
            }

            // Event listener to handle acceptance of cookies
            document.getElementById("acceptCookiesBtn").addEventListener("click", function () {
                setCookie("cookieConsent", "true", 365); // Set the cookie for 1 year
                document.getElementById("cookieConsent").style.display = "none"; // Hide the banner

                // Enable the login link after cookies are accepted
                var logInLink = document.getElementById("logInLink");
                if (logInLink) {
                    logInLink.classList.remove("disabled"); // Remove disabled class
                    logInLink.style.pointerEvents = "auto"; // Enable clicks
                }
            });
        });

        // Function to get a cookie value by name
        function getCookie(name) {
            var nameEq = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i].trim();
                if (c.indexOf(nameEq) === 0) return c.substring(nameEq.length, c.length);
            }
            return null;
        }

        // Function to set a cookie
        function setCookie(name, value, days) {
            var expires = "";
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                expires = "; expires=" + date.toUTCString();
            }
            document.cookie = name + "=" + (value || "") + expires + "; path=/";
        }


    </script>
</body>

</html>