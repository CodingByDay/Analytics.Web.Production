<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="peptak.AdminPanel" %>
<%@ Register assembly="DevExpress.Web.Bootstrap.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

       <webopt:bundlereference runat="server" path="~/css/adminpanel.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
	


	<div id="boot">

		</div>
	
<div class="wrapper">

	<header>
		<h1></h1>
	</header>
		
<section class="columns">
	
	<div class="column">
		<h2></h2>
		<p></h1>
			
	</header>
		
<section class="columns">
	
	<div class="column">
		<dx:BootstrapListBox ID="companiesListBox" runat="server" FilteringSettings-EditorNullText="Poiščite podjetje" FilteringSettings-UseCompactView="true" ViewStateMode="Disabled" Rows="2">
        <CssClasses Control="companies" />
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
	

	</div>
	
	<div class="column">
		<dx:BootstrapListBox ID="usersListBox" runat="server" FilteringSettings-EditorNullText="Poiščite uporabnika" FilteringSettings-UseCompactView="true" ViewStateMode="Disabled" Rows="6">
        <CssClasses Control="users" />
    <FilteringSettings ShowSearchUI="true" EditorNullTextDisplayMode="Unfocused" />
</dx:BootstrapListBox>
	</div>
  
  <div class="column">
		<h2>3rd Content Area</h2>
		<p></p>
	</div>
	
</section>	
	
	<footer>
		<h3>Footer</h3>
		<p>Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ipsam, porro. Doloribus vitae non dolores modi dolorum commodi perspiciatis dicta nostrum minus esse, totam officia, quibusdam amet quas tempora? Voluptatibus, esse.</p>
	</footer>

</div>

 
     

 
</div>
     </div>
     </div>
</div>
     </div>
     </div>
     </div>
     </div>
     </div>
     </div>
</asp:Content>

