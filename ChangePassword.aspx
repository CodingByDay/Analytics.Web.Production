<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Dash.ChangePassword" %>

<!DOCTYPE html>
<html>
<head>
    <title>Resetirajte geslo</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <!-- Favicons -->
    <link href="assets/img/favicon.ico" rel="icon">
    <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

    <style>
        body {
            background-image: url('assets/img/hero-bg.jpg'); /* Change the path to your image */
            background-size: cover; /* Ensures the image covers the entire background */
            background-position: center; /* Centers the image */
            background-repeat: no-repeat; /* Prevents the image from repeating */
            min-height: 100vh; /* Ensures the body takes up the full height of the viewport */
            font-family: Arial, sans-serif; /* Sets a default font for the page */
        }

        .container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            padding: 20px; /* Padding around the container */
        }

        .form-card {
            background-color: white;
            padding: 30px;
            border-radius: 8px; /* Rounded corners for the card */
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); /* Subtle shadow for depth */
            width: 100%;
            max-width: 400px; /* Maximum width for better readability */
        }

        h3 {
            color: #17c0eb;
            font-size: 28px; /* Adjusted font size */
            margin-bottom: 20px; /* Space below the title */
        }
          .btn:hover {
             background-color: #39ace7!important;
             color: #fff;
         }
        .form-group {
            margin-bottom: 15px; /* Space between input fields */
        }
        h3 {
            margin-top: 0;
        }
        .form-control {
            width:95%;
            padding:10px;
            border: 1px solid #ced4da; /* Default border color */
            border-radius: 4px; /* Rounded corners */
            font-size: 16px; /* Font size for input text */
        }

        .form-control:focus {
            border-color: #18dcff; /* Border color on focus */
            box-shadow: none; /* Remove default shadow on focus */
        }

        .btn {
            background-color: #17c0eb; /* Button background color */
            color: white; /* Button text color */
            width: 100%; /* Full-width button */
            padding: 10px;
            font-size: 18px; /* Font size for buttons */
            border: none; /* Remove default border */
            border-radius: 4px; /* Rounded corners */
            cursor: pointer; /* Cursor indicates button is clickable */
            transition: background-color 0.3s; /* Smooth background transition */
        }

        .btn:hover {
            background-color: #2d3436; /* Darker background on hover */
        }

        @media(max-width: 768px) {
            h3 {
                font-size: 24px; /* Smaller font size for smaller screens */
            }

            .form-card {
                margin-top: -20em!important;
            }
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="form-card">
            <h3>Spremeni geslo</h3>
            <form action="#" runat="server">
                <div class="form-group">
                    <asp:TextBox ID="pwd" runat="server" TextMode="Password" placeholder="Geslo" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:TextBox ID="REpwd" runat="server" TextMode="Password" placeholder="Še enkrat" CssClass="form-control" />
                </div>
                <asp:Button ID="change" runat="server" Text="Spremeni" OnClick="change_Click" CssClass="btn" />
                <br /><br />
                <asp:Button ID="backButton" runat="server" Text="Nazaj" OnClick="backButton_Click" CssClass="btn" />
            </form>
        </div>
    </div>
</body>
</html>
