<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create.aspx.cs" Inherits="TopicMaster.Create" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ValidationSummary ID="PostValidationSummary" runat="server" HeaderText="Correct what's wrong"
    CssClass="validation-summary-errors-icon" ValidationGroup="vgPost" />
    <h1>Create post</h1>

    <asp:Label ID="Label1" runat="server" Text="Enter your post:"></asp:Label>
    <p><asp:TextBox ID="PostValueTextBox" runat="server" textMode="MultiLine" height="100px" width="500px"></asp:TextBox></p>
    <p>
    <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click" ValidationGroup="vgPost">Save</asp:LinkButton>
    <asp:LinkButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" CausesValidation="false">Cancel</asp:LinkButton>
    </p>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" Runat="Server">
</asp:Content>--%>
