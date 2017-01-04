<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_SetupMaster.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.admin_SetupMaster"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Web Content Management Administration</title>
    <link rel="stylesheet" type="text/css" href="/css/default.css" />
    <script type="text/javascript" src="/javascript/global.js"></script>
    <script type="text/javascript" src="/javascript/default.js"></script>
    <script type="text/javascript">  
  
    </script>
    <meta http-equiv="refresh" content="60" />

</head>
<body style="margin-top:0; margin-left:0" >
    <form runat="server">

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Code, Type" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowCommand="GridView1_RowCommand" ShowFooter="True" OnRowDeleting="GridView1_RowDeleting">
<Columns>

<asp:TemplateField HeaderText="Name" SortExpression="Name"> <EditItemTemplate>
  <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
</EditItemTemplate>
<FooterTemplate>
  <asp:TextBox ID="txtNewName" runat="server"></asp:TextBox> </FooterTemplate>
<ItemTemplate>
  <asp:Label ID="Label2" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Gender">
<EditItemTemplate>
  <asp:DropDownList ID="cmbGender" runat="server" SelectedValue='<%# Eval("Gender") %>'>
    <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
    <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
  </asp:DropDownList>
</EditItemTemplate>
<ItemTemplate>
  <asp:Label ID="Label2" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
  <asp:DropDownList ID="cmbNewGender" runat="server" >
    <asp:ListItem Selected="True" Text="Male" Value="Male"></asp:ListItem>
    <asp:ListItem Text="Female" Value="Female"></asp:ListItem> </asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="City">
<EditItemTemplate>
  <asp:TextBox ID="txtCity" runat="server" Text='<%# Bind("City") %>'></asp:TextBox>
</EditItemTemplate>
<FooterTemplate>
  <asp:TextBox ID="txtNewCity" runat="server" ></asp:TextBox>
</FooterTemplate>
<ItemTemplate>
  <asp:Label ID="Label3" runat="server" Text='<%# Bind("City") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="State" SortExpression="State">
<EditItemTemplate>
  <asp:Label ID="Label1" runat="server" Text='<%# Eval("State") %>'></asp:Label>
</EditItemTemplate>
<FooterTemplate>
  <asp:TextBox ID="txtNewState" runat="server" ></asp:TextBox>
</FooterTemplate>
<ItemTemplate>
  <asp:Label ID="Label4" runat="server" Text='<%# Bind("State") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Type">
<EditItemTemplate>
  <asp:DropDownList ID="cmbType" runat="server" DataTextField="Type" DataValueField="Type"> </asp:DropDownList>
</EditItemTemplate>
<ItemTemplate>
  <asp:Label ID="Label5" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
  <asp:DropDownList ID="cmbNewType" runat="server" DataTextField="Type" DataValueField="Type"> </asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Edit" ShowHeader="False">
<EditItemTemplate>
  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
  <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
</EditItemTemplate>
<FooterTemplate>
  <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="AddNew" Text="Add New"></asp:LinkButton>
</FooterTemplate>
<ItemTemplate>
  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" />

</Columns>
</asp:GridView> 
    </form>
</body>
</html>
