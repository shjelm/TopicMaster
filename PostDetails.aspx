<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostDetails.aspx.cs" Inherits="PostDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>
        Details</h1>
    <asp:ValidationSummary ID="MemberValidationSummary" runat="server" HeaderText="Fel inträffade. Korrigera det som är fel och försök igen."
        CssClass="validation-summary-errors-icon" />
    <%-- Post --%>
    <div class="editor-label">
        <asp:Label ID="Label1" runat="server" Text="Post:" />
    </div>
    <div class="editor-field">
        <asp:Label ID="PostLabel" runat="server" />
    </div>
    <p>
        <asp:LinkButton ID="EditButton" runat="server">Edit</asp:LinkButton>
        <asp:LinkButton ID="DeleteButton" runat="server" OnCommand="DeleteButton_Command">Delete</asp:LinkButton></p>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" Runat="Server">
    <script src="<%= ResolveClientUrl("~/Scripts/delete-confirm.js") %>" type="text/javascript"></script>
</asp:Content>--%>
