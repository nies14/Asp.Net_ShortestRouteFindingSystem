<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Districts.aspx.cs" Inherits="DisconnectedExample.Districts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
        Add Districts:<br />
        <br />
        District Name:
        <asp:TextBox ID="txtDistrict" runat="server"></asp:TextBox>
        <asp:Label ID="lblerrormsg" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btnAddDistrict" runat="server" Text="Add District" OnClick="btnAddDistrict_Click" />
        <asp:Button ID="btnLoadDistricts" runat="server" Text="Load Districts" OnClick="btnLoadDistricts_Click" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        <asp:Button ID="btnGoback" runat="server" OnClick="btnGoback_Click" Text="Go Back" />
        <br />
        <br />
         <asp:Panel ID="PanelSearch" runat="server">
            <br />
            <asp:Label ID="Label1" runat="server" Text=" Enter Search key:  "></asp:Label>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btnGO" runat="server" Text="GO" OnClick="btnGO_Click" Width="84px" style="margin-left: 22px" />
        </asp:Panel>
        <br />
        <br />
        <asp:GridView ID="GridViewDistricts" runat="server" AutoGenerateColumns="False" DataKeyNames="DistrictId" OnRowCancelingEdit="GridViewDistricts_RowCancelingEdit" OnRowDeleting="GridViewDistricts_RowDeleting" OnRowEditing="GridViewDistricts_RowEditing" OnRowUpdating="GridViewDistricts_RowUpdating">
            <Columns>
                <asp:BoundField DataField="DistrictName" HeaderText="District Name" />
                <asp:CommandField HeaderText="Options" ShowDeleteButton="True" ShowEditButton="True" />
            </Columns>
        </asp:GridView>
        </center>
    </div>
    </form>
</body>
</html>