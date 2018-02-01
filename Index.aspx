<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DisconnectedExample.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <center>
        <asp:Panel ID="PanelFinder" runat="server">
          
            <asp:Label ID="Label3" runat="server" Text="Route Finder" Font-Names="Segoe UI" Font-Size="X-Large" ForeColor="#3399FF"></asp:Label>
            <br />
            <br />
                      
            <asp:ImageButton ID="imbtnAdminLogin" runat="server" AlternateText="Admin Login" Height="39px" ImageUrl="~/images/AdminLogin_ok.png" Width="126px" OnClick="imbtnAdminLogin_Click" />
            <br /><br />
                        
            <asp:Label ID="Label1" runat="server" Text="Source"></asp:Label><br />
            <asp:DropDownList ID="DropDownListSource" runat="server"></asp:DropDownList><br />
            <br />
                
            <asp:Label ID="Label2" runat="server" Text="Destination"></asp:Label><br />
    <asp:DropDownList ID="DropDownListDestination" runat="server"></asp:DropDownList>
            <br />
               
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            <br /><br />
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" Font-Size="Medium" ForeColor="#009900" Width="99px" />
            <br />
                  
        </asp:Panel>
        
        <asp:Panel ID="PanelLogin" runat="server">
<asp:Label ID="Label6" runat="server" Text="Admin Login Panel" Font-Size="X-Large" ForeColor="#993300"></asp:Label><br />
            <asp:Label ID="lblMSG" runat="server" ForeColor="Red"></asp:Label>
            <br />
<asp:Label ID="Label4" runat="server" Text="User Name: "></asp:Label>
<asp:TextBox ID="txtUserName" runat="server" Height="21px"></asp:TextBox><br /><br />
            <asp:Label ID="Label5" runat="server" Text="Password: "></asp:Label>
<asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
            <br />
<asp:Button ID="btnlogin" runat="server" Text="Login" OnClick="btnlogin_Click" Width="89px"></asp:Button>
            <asp:Button ID="btnBack" runat="server" Text="Back " OnClick="btnBack_Click" Width="83px" />
        </asp:Panel>
        </center>
        </div>
    </form>
</body>
</html>