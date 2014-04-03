<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Administrators_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ObjectDataSource ID="ServiceObjectDataSource" runat="server" SelectMethod="GetMembers"
        TypeName="Service" />
    <h1>
        Members</h1>
    <asp:ListView ID="MemberListView" runat="server" DataSourceID="ServiceObjectDataSource">
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
                    <%# Eval("Username")%></dt>
            </dl>
            <asp:LinkButton ID="DeleteMemberButton" runat="server" OnCommand="DeleteMemberButton_Command" 
            CommandArgument='<%#Eval("Username") %>' > Delete member</asp:LinkButton>
        </ItemTemplate>
    </asp:ListView>
    </asp:Content>
