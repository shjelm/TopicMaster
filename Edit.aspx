<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ValidationSummary ID="PostValidationSummary" runat="server" HeaderText="Correct what's wrong"
    CssClass="validation-summary-errors-icon" ValidationGroup="EditPostVg" />
    <h1>
        Edit post
    </h1>
    <asp:Label ID="Label1" runat="server" Text="Edit your post:"></asp:Label>
    <p>
        <asp:TextBox ID="EditValue" runat="server" textMode="MultiLine" height="100px" width="500px"></asp:TextBox></p>
    <p>
        <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click" ValidationGroup="EditPostVg">Save</asp:LinkButton>
        <asp:LinkButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" CausesValidation="false">Cancel</asp:LinkButton>
</asp:Content>
