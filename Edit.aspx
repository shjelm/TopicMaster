<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>
        Edit post</h1>
    <%-- "Edit post control. --%>
    <asp:Label ID="Label1" runat="server" Text="Edit your post:"></asp:Label>
    <p><asp:TextBox ID="EditValue" runat="server" textMode="MultiLine" height="100px" width="500px"></asp:TextBox></p>
    <p>
    <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click" ValidationGroup="vgPost">Save</asp:LinkButton>
    <asp:LinkButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" CausesValidation="false">Cancel</asp:LinkButton>
</asp:Content>

<%--<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" Runat="Server">
    <script src="<%= ResolveClientUrl("~/Scripts/delete-confirm.js") %>" type="text/javascript"></script>
</asp:Content>--%>
