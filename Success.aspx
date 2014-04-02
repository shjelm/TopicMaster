<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Success.aspx.cs" Inherits="Success" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>
        <asp:Literal ID="MessageLiteral" runat="server" /></h1>
    <p class="redirect">
        Your request was handled correctly. Return to the 
        <asp:HyperLink ID="RedirectHyperLink" runat="server" NavigateUrl='~/Default.aspx'
            Text="startpage" />.</p>
</asp:Content>
