<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Settings" %>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Settings</h1>
        <asp:HyperLink ID="ChangePasswordLink" runat="server" NavigateUrl="Account/ChangePassword.aspx">Change password</asp:HyperLink>
        <asp:HyperLink ID="ChangeEmailLink" runat="server" NavigateUrl="Account/ChangeEmail.aspx">Change email</asp:HyperLink>

</asp:Content>
