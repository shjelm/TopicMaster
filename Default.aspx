<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Topic Master!
    </h2>
    <p>
       Here you can post topics and reply to others. To fully access the website you need to log in.
    </p>
    <p><asp:HyperLink ID="ViewPostsHyperLink" runat="server" NavigateUrl="~/ViewPosts.aspx">View posts</asp:HyperLink></p>
    <p><asp:HyperLink ID="CreateHyperLink" visible="false" runat="server" NavigateUrl="~/Create.aspx">Create post</asp:HyperLink></p>

</asp:Content>
