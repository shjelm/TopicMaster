<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostDetails.aspx.cs" Inherits="PostDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ValidationSummary ID="DetailsValidationSummary" runat="server" HeaderText="Fel inträffade. Korrigera det som är fel och försök igen."
        CssClass="validation-summary-errors-icon" ValidationGroup="PostDetailsVg" />
    <%-- Post --%>
    <div class="editor-label">
        <asp:Label ID="Label1" runat="server" Text="Post:" Font-Size="Large" />
        
    </div>
    <div class="editor-field">
        <asp:Label ID="PostLabel" runat="server" />
        <div>
        <p>Created by: 
            <asp:Label ID="AuthorLabel" runat="server" /></p>
        </div>
    </div
    <p>
        <asp:LinkButton ID="EditPostButton" runat="server" Visible="false">Edit</asp:LinkButton>
        <asp:LinkButton ID="DeletePostButton" runat="server" OnCommand="DeletePostButton_Command" Visible="false">Delete</asp:LinkButton></p>
    
    <div>

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            DataObjectTypeName="Comment" DeleteMethod="DeleteComment" 
            OnUpdated="CommentDataSource_Updated" OnInserted="CommentDataSource_Inserted"
            OnInserting="CommentDataSource_Inserting" OnUpdating="CommentDataSource_Updating"
            OnDeleted="CommentDataSource_Deleted"
            InsertMethod="SaveComment" OldValuesParameterFormatString="original_{0}" 
            SelectMethod="GetCommentsByPostId" TypeName="Service" 
            UpdateMethod="SaveComment">
        <SelectParameters>
            <asp:QueryStringParameter Name="postId" QueryStringField="id" Type="Int32" />
        </SelectParameters>
        </asp:ObjectDataSource>

    </div>
    <div>
    <asp:ListView ID="CommentListView" runat="server" DataSourceID="ObjectDataSource1" 
            InsertItemPosition="LastItem" DataKeyNames="CommentId, PostId, MemberId" OnItemDataBound="CommentListView_ItemDataBound">
        <AlternatingItemTemplate>
            <span style="">
            <h3>Comment:</h3>
            </br>
            <asp:Label ID="ValueLabel" runat="server" Text='<%# Eval("Value") %>' />
            <br />
            <div>
                <p>Created by: <asp:Label ID="AuthorCommentLabel" runat="server" /></p>
            </div>
            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
            <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete"  Visible="false" 
                Text="Delete"  OnClientClick="return confirm('Are you sure you want to delete your comment?');"/>
<br /><br /></span>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <span style="">
            <h3>Comment:</h3>
            </br>
            <asp:TextBox ID="ValueTextBox" runat="server" Text='<%# Bind("Value") %>' Width="500px" Height="150px" TextMode="MultiLine" />
            <br />
            <div>
                <p>Created by: <asp:Label ID="AuthorCommentLabel" runat="server" /></p>
            </div>
            <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update"
                Text="Update" />
            <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" 
                Text="Cancel" />
            <br /><br /></span>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <span>No comments.</span>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <br /><h2>Write a comment:</h2>
            </br>
            <asp:TextBox ID="ValueTextBox" runat="server" Text='<%# Bind("Value") %>' Width="500px" Height="150px" TextMode="MultiLine"/>
            <br />
            <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert"
                Text="Insert" />
            <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel"
                Text="Clear" />
            <br /><br /></span>
        </InsertItemTemplate>
        <ItemTemplate>
        <span style="">
            <h3>Comment:</h3>
            </br>
            <asp:Label ID="ValueLabel" runat="server" Text='<%# Eval("Value") %>' />
            <br />
            <div>
                <p>Created by: <asp:Label ID="AuthorCommentLabel" runat="server" /></p>
            </div>
            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Visible="false" Text="Edit" />
            <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Visible="false" 
                Text="Delete" OnClientClick="return confirm('Are you sure you want to delete your comment?');"/>
<br /><br /></span>
        </ItemTemplate>
        <LayoutTemplate>
            <div ID="itemPlaceholderContainer" runat="server" style="">
                <span runat="server" id="itemPlaceholder" />
            </div>
            <div style="">
            </div>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <span style="">
            <h3>Comment:</h3>
            </br>
            <asp:Label ID="ValueLabel" runat="server" Text='<%# Eval("Value") %>' />
            <br />
            <div>
                <p>Created by: <asp:Label ID="AuthorCommentLabel" runat="server" /></p>
            </div>
            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="Edit" Visible="false" />
            <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Visible="false" 
                Text="Delete" OnClientClick="return confirm('Are you sure you want to delete your comment?');"/>
<br /><br /></span>
        </SelectedItemTemplate>
    </asp:ListView>
    
    <asp:LinkButton ID="GoBackButton" runat="server" PostBackUrl="~/ViewPosts.aspx" >Go back</asp:LinkButton>
</asp:Content>
