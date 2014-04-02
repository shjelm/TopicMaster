<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NotFound.aspx.cs" Inherits="NotFound" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Kund saknas</h1>
    <p>
        Tyvärr kunde kunden du efterfrågade inte hittas.</p>
    <p class="redirect">
        Om 5 sekunder förflyttas du automatiskt till
        <asp:HyperLink ID="RedirectHyperLink" runat="server" NavigateUrl="~/" Text="startsidan" />.</p>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="Server">
    <script type="text/javascript">
        $(function () {
            $.timer(5000, function () {
                window.location.href = '<%= ResolveClientUrl("~/") %>';
            });
        });      
    </script>--%>
<%--</asp:Content>--%>
