<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPosts.aspx.cs" Inherits="ViewPosts" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ObjectDataSource ID="ServiceObjectDataSource" runat="server" SelectMethod="GetPosts"
        TypeName="Service" />
    <h1>
        Posts</h1>
    <asp:ListView ID="PostListView" runat="server" DataSourceID="ServiceObjectDataSource">
        <LayoutTemplate>
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            <%-- "Paging" --%>
            <div id="pager">
                <asp:DataPager ID="DataPager1" runat="server" PageSize="6" QueryStringField="page">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="True" ShowNextPageButton="False"
                            ShowPreviousPageButton="False" FirstPageText="First" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="False"
                            ShowPreviousPageButton="False" LastPageText="Last" />
                    </Fields>
                </asp:DataPager>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <dl class="member-card">
                <dt>
                    <asp:HyperLink ID="DetailsLink" runat="server" NavigateUrl='<%# Eval("PostId", "~/PostDetails.aspx?id={0}") %>'><%# Eval("PostId") %></asp:HyperLink></dt>
                <dd>
                    <%# Eval("Value") %></dd>
            </dl>
        </ItemTemplate>
    </asp:ListView>
    </asp:Content>
