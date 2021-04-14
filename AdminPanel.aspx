<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="peptak.AdminPanel" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>


       <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />


	
<div class="wrapper">

	<header>
		<h1>3 Column Responsive Layout</h1>
	</header>
		
<section class="columns">
	
	<div class="column">
		<h2>1st Content Area</h2>
		<p>Lorem ipsum dolor sit a3 Column Responsive Layout</h1>
	</header>
		
<section class="columns">
	
	<div class="column">
		<h2>1st Content Area</h2>
		<p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Sequi ratione architecto necessitatibus cum praesentium dolor totam voluptatibus recusandae?
        </p>
		<dx:BootstrapListBox ID="companiesListBox" runat="server" Heigth="150" Width="300px" FilteringSettings-UseCompactView="true" Rows="100">
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
	   

	</div>
	
	<div class="column">
		<h2>2nd Content Areae? Illo quod nemo ratione itaque dolores laudantium error vero laborum blanditiis nostrum.</p>
	</div>
  
  <div class="column">
		<h2>3rd Content Area</h2>
		<p>Illo quod nemo ratione itaque dolores laudantium error vero laborum blanditiis nostrum. Lorem ipsum dolor sit amet consectetur adipisicing elit. Sequi ratione architecto cum praesentium voluptatibus recusandae?</p>
	</div>
	
</section>	
	
	<footer>
		<h3>Footer</h3>
		<p>Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ipsam, porro. Doloribus vitae non dolores modi dolorum commodi perspiciatis dicta nostrum minus esse, totam officia, quibusdam amet quas tempora? Voluptatibus, esse.</p>
	</footer>

</div>

 
     </div>

 
</asp:Content>

