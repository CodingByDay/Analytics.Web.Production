<%@ Page Title="Administracija" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="peptak.Administration" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
     <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>


       <webopt:bundlereference runat="server" path="~/css/graphs.css" />
<link href= "~/css/graphs.css" rel="stylesheet" runat="server" type="text/css" />
  
     <body>
    <table width="80%" align="center">
      <tr>
        <td>
          <asp:Button Text="Pravice" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
              OnClick="Tab1_Click" />
          <asp:Button Text="Registracija" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" />
          <asp:Button Text="Registracija podj." BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
              OnClick="Tab3_Click" />
         <asp:Button Text="Brisanje" BorderStyle="None" ID="Tab4" CssClass="Initial" runat="server"
              OnClick="Button1_Click1" />
          <asp:MultiView ID="MainView" runat="server">
            <asp:View ID="View1" runat="server">
              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid;background-color: #c5d5cb">
                <tr>
                  <td>
                    <h3>
                   <center><h3 style="font-style: italic;font-weight: bold">Izberite uporabnika, in odločite katere grafe lahko vidi.</h3></center>
      <hr style="color: black;" />
  <center><asp:DropDownList ID="usersPermisions" autopostback="true" runat="server" OnSelectedIndexChanged="usersPermisions_SelectedIndexChanged" ></asp:DropDownList>></center>    
     
   <center><DIV style="OVERFLOW-Y:scroll; WIDTH:600px; HEIGHT:500px">
     <center><dx:ASPxCheckBoxList  ID="graphsFinal" runat="server" ValueType="System.String" CaptionSettings-HorizontalAlign="Center" Border-BorderStyle="None">
        </dx:ASPxCheckBoxList></center>
       </div>
       </center>
     
        <center><asp:Button CssClass="btn btn-primary" type="submit" Value="Save" runat="server" ID="Save" Text="Shrani" OnClick="Save_Click1" /></center>
   
   
                    </h3>
                  </td>
                </tr>
              </table>
            </asp:View>
            <asp:View ID="View2" runat="server">
             

           
              <div style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; align-items:center; display:inline-block; font-size:larger; background-color: #c5d5cb" >
                         <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija uporabnika</h3></center>

                    <center> <asp:TextBox ID="TxtName" runat="server" placeholder="Ime in priimek" CssClass="form-control form-control-lg"></asp:TextBox></center>
                   </center>  
                  
                    <center> <asp:TextBox ID="email" runat="server" placeholder="Email" CssClass="form-control form-control-lg"></asp:TextBox></center>
                   </center> 
              
                <center>  
                
                        <center><asp:TextBox ID="TxtUserName" runat="server" placeholder="Uporabniško ime" CssClass="form-control form-control-lg"></asp:TextBox></center>  
               
               </center>  
              
                 
                       <center> <asp:TextBox ID="TxtPassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo" CssClass="form-control form-control-lg"></asp:TextBox>  </center>
              
                  
                       <center> <asp:TextBox ID="TxtRePassword" runat="server"  
                                     TextMode="Password" placeholder="Geslo še enkrat" CssClass="form-control form-control-lg"></asp:TextBox> </center> 
                 
           <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Pozicija</h3></center>    

                  
                       <center><asp:RadioButtonList ID="userRole" runat="server">  
                            <asp:ListItem>Admin</asp:ListItem>  
                            <asp:ListItem>User</asp:ListItem>  
                        </asp:RadioButtonList>  </center>
                
<center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Tip uporabnika.</h3></center>    
                <center><asp:DropDownList ID="userType" autopostback="true" runat="server"  >
                 </asp:DropDownList></center>   
        
              
                   <center> <h4 style="text-decoration: solid"Podjetje:</h4></center>  
                       <center> <asp:DropDownList ID="companiesList" runat="server"  
                                          AppendDataBoundItems="true">  
                           
                        </asp:DropDownList>  </center>
              
     <center><asp:Button CssClass="btn btn-primary" ID="registrationButton" runat="server" Text="Potrdi"   OnClick="Button1_Click" /></center>
                  <hr />
                    </h3>
                  </td>
                </tr>
              </div>
            </asp:View>
            </asp:View>
                    </h3>
                  </td>
                </tr>
              </table>
            </asp:View>
            <asp:View ID="View3" runat="server">
              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; background-color: #c5d5cb">
                <tr>
                  <td>
                    <h3>
                     <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Registracija novega podjetja.</h3></center>  
             <hr style="color: black;" />

             <center><asp:TextBox ID="companyName" runat="server" placeholder="Ime" CssClass="form-control"></asp:TextBox></center>
           <center> <asp:TextBox ID="companyNumber" runat="server" placeholder="Številka" CssClass="form-control"></asp:TextBox> </center>                 
           <center><asp:TextBox ID="website" runat="server" placeholder="Website podjetja:" CssClass="form-control"></asp:TextBox> </center>
<center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Admin</h3></center>    
           <center> <asp:DropDownList ID="listAdmin" runat="server" > 
               <%--AppendDataBoundItems="true">--%>  
              </asp:DropDownList>  </center>
              <center><h3 style="text-decoration: solid; font-style: italic;font-weight: bold">Baza.</h3></center>    

                  
                  <center><asp:DropDownList ID="ConnectionStrings" runat="server"  
               AppendDataBoundItems="true">  
              </asp:DropDownList></center>  
             <center><asp:Button CssClass="btn btn-primary" ID="companyButton" runat="server" Text="Potrdi" OnClick="companyButton_Click"/></center> 
                    </h3>
                  </td>
                </tr>
              </table>
            </asp:View>
<asp:View ID="View4" runat="server">
              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; background-color: #c5d5cb">
                <tr>
                  <td>
                    <hr />
                     <center><h4 style="text-decoration: solid; font-style: italic;font-weight: bold"">Brišite uporabnika.</h4></center>
        <center><asp:DropDownList ID="DeleteUser" runat="server"  
               AppendDataBoundItems="true">  
        </asp:DropDownList></center>
       <center> <asp:Button CssClass="btn btn-primary" ID="delete" runat="server" Text="Briši" OnClick="delete_Click"/></center>
       


      
     <center><h4 style="text-decoration: solid; font-style: italic;font-weight: bold">Izbrišite podjetje.</h4></center>
<hr />
           <center><asp:DropDownList ID="deleteCompany" runat="server"  
               AppendDataBoundItems="true"> 
        </asp:DropDownList></center> 
        <center><asp:Button  CssClass="btn btn-primary" ID="companyDelete" runat="server" Text="Briši podjetje"  OnClick="deleteCompanyButton_Click"/></center>
                        <hr />
       <center><h4 style="text-decoration: solid; font-style: italic;font-weight: bold">Spremenite admina podjetja</h4></center>

           <center><asp:DropDownList ID="ChooseCompany" runat="server"  
               AppendDataBoundItems="true">  
        </asp:DropDownList></center>
        <center> <asp:DropDownList ID="ChooseUser" runat="server"  
               AppendDataBoundItems="true">  
        </asp:DropDownList></center>
        <center><asp:Button CssClass="btn btn-primary" ID="changeCompany" runat="server" Text="Spremeni" OnClick="changeCompany_Click"/></center>
                      
                    </h3><hr />
                  </td>
                </tr>
              </table>
            </asp:View>
          </asp:MultiView>
        </td>
      </tr>
    </table>
</body>
 
</asp:Content>
