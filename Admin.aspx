<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="DisconnectedExample.Admin1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <center>
        <asp:Label ID="Label1" runat="server" Text="Welcome, Admin" Font-Size="XX-Large" ForeColor="#CC3300"></asp:Label>
        <br />
        <br />
        <hr />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Districts.aspx" Font-Size="Larger">Manage Districts</asp:HyperLink>
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="Routes.aspx" Font-Size="Larger">Manage Routes</asp:HyperLink>
        <br />
        <hr />
        <asp:Button ID="btnLogout" runat="server" OnClick="btnLogout_Click" style="height: 26px" Text="Logout" />
        <br />
    </center>
        </div>
    </form>
</body>
</html>