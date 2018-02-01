<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Routes.aspx.cs" Inherits="DisconnectedExample.Routes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        Add Routes:<br />
        <br />
        Source:<asp:DropDownList ID="DropDownListSource" runat="server">
        </asp:DropDownList>
        <br />
        Destination:<asp:DropDownList ID="DropDownListDestination" runat="server">
        </asp:DropDownList>
        <asp:Label ID="lblErrorMsgDlist" runat="server" ForeColor="Red"></asp:Label>
        <br />
        Cost:
        <asp:TextBox ID="txtCost" runat="server" TextMode="Number"></asp:TextBox>
        <asp:Label ID="lblerrormsg" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <asp:Button ID="btnAdd" runat="server" Text="Add Route" OnClick="btnAdd_Click" />
        <asp:Button ID="btnLoad" runat="server" Text="Load Routes" OnClick="btnLoad_Click" />
        <asp:Button ID="btnGoback" runat="server" Text="Go back" OnClick="btnGoback_Click" />

        <br />
        <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="RouteId" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
            <Columns>
                <asp:BoundField DataField="Source" HeaderText="Source" ReadOnly="True" />
                <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="True" />
                <asp:BoundField DataField="Cost" HeaderText="Cost" />
                <asp:CommandField HeaderText="Options" ShowDeleteButton="True" ShowEditButton="True" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>