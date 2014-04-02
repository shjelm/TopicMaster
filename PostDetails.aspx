<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostDetails.aspx.cs" Inherits="PostDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>
        Details</h1>
    <asp:ValidationSummary ID="DetailsValidationSummary" runat="server" HeaderText="Fel inträffade. Korrigera det som är fel och försök igen."
        CssClass="validation-summary-errors-icon" />
    <%-- Post --%>
    <div class="editor-label">
        <asp:Label ID="Label1" runat="server" Text="Post:" />
        
    </div>
    <div class="editor-field">
        <asp:Label ID="PostLabel" runat="server" />
        
    </div>
    <p>
        <asp:LinkButton ID="EditButton" runat="server" Visible="false">Edit</asp:LinkButton>
        <asp:LinkButton ID="DeleteButton" runat="server" OnCommand="DeleteButton_Command" Visible="false">Delete</asp:LinkButton></p>
    
    <div>
    <div>
    <asp:ObjectDataSource ID="PostDataSource" runat="server" SelectMethod="GetCommentsByPostId"
        TypeName="Service" OnSelected="PostDataSource_Selected" DataObjectTypeName="Comment"
        DeleteMethod="DeleteComment" InsertMethod="SaveComment" UpdateMethod="SaveComment"
        OnUpdated="PostDataSource_Updated" OnInserted="PostDataSource_Inserted"
        OnInserting="PostDataSource_Inserting" OnUpdating="PostDataSource_Updating"
        OnDeleted="PostDataSource_Deleted">
        <SelectParameters>
            <asp:QueryStringParameter Name="postID" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:View ID="EditView" runat="server">
        <asp:ListView ID="PostListView" runat="server" DataKeyNames="PostId, MemberId, CommentId"
                DataSourceID="PostDataSource" InsertItemPosition="LastItem" OnItemInserting="PostListView_ItemInserting" >
                <LayoutTemplate>
                    <asp:ValidationSummary ID="CommentValidationSummary" runat="server" HeaderText="<%$ Resources:Strings, Validation_Header %>"
                        CssClass="validation-summary-errors-icon" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="<%$ Resources:Strings, Validation_Header %>"
                        CssClass="validation-summary-errors-icon" ValidationGroup="DetailsValidationSummary" />
                    <ul>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                    </ul>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <p>
                        No comments.</p>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <li>
                        <asp:TextBox ID="ValueTextBox" runat="server" Text='<%# Bind("Value") %>' MaxLength="500"
                            Enabled="false" />
                        <%-- "Kommandknappar" för att redigera och ta bort en kunduppgift . Kommandonamnen är VIKTIGA! --%>
                        <asp:LinkButton ID="EditLinkButton" runat="server" CommandName="Edit" Text="Redigera"
                            CausesValidation="false" />
                        <%-- Unobtrusive JavaScript ersätter OnClientClick='<%# String.Format("return confirm(\"Ta bort kunduppgiften &rsquo;{0}&rsquo; permanent?\" )", Eval("Value")) %>' --%>
                        <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" Text="Ta bort"
                            CausesValidation="false" CssClass="delete-action" data-type="<%$ Resources:Strings, Data_Type_Post %>"
                            data-value='<%# Eval("Value") %>' />
                    </li>
                </ItemTemplate>
                <EditItemTemplate>
                    <li>
                        <asp:TextBox ID="ValueTextBox" runat="server" Text='<%# Bind("Value") %>' MaxLength="500" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$ Resources:Strings, Post_Value_Required %>"
                            ControlToValidate="ValueTextBox" CssClass="field-validation-error" Display="Dynamic">*</asp:RequiredFieldValidator>
                        <%-- "Kommandknappar" för att uppdatera en kontaktuppgift och avbryta. Kommandonamnen är VIKTIGA! --%>
                        <asp:LinkButton ID="UpdateLinkButton" runat="server" CommandName="Update" Text="Spara" />
                        <asp:LinkButton ID="CancelLinkButton" runat="server" CommandName="Cancel" Text="Avbryt"
                            CausesValidation="false" />
                    </li>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <li>
                        <asp:TextBox ID="ValueTextBox" runat="server" Text='<%# Bind("Value") %>' MaxLength="50"
                            ValidationGroup="vgPostInsert" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$ Resources:Strings, Post_Value_Required %>"
                            ControlToValidate="ValueTextBox" CssClass="field-validation-error" ValidationGroup="vgPostInsert"
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                        <%-- "Kommandoknappar" för att uppdatera en kontaktuppgift och avbryta. Kommandonamnen är VIKTIGA! --%>
                        <asp:LinkButton ID="UpdateLinkButton" runat="server" CommandName="Insert" Text="Spara"
                            ValidationGroup="vgPostInsert" />
                    </li>
                </InsertItemTemplate>
            </asp:ListView>
        </asp:View>
        <asp:View ID="ReadOnlyView" runat="server">
            <%-- ListView som presenterar en kunds kontaktuppgifter. --%>
            <asp:ListView ID="PostReadOnlyListView" runat="server" DataKeyNames="PostId, MemberId, PostTypeId"
                DataSourceID="PostDataSource" >
                <LayoutTemplate>
                    <ul>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </ul>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <p>
                        Kontaktuppgifter saknas.</p>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <li>
                        <%-- Label-kontrollen uppgift är att visa vilken typ kontaktinformationen är av. --%>
                        <asp:Label ID="PostTypeNameLabel" runat="server" /><%# Eval("Value") %></li>
                </ItemTemplate>
            </asp:ListView>
        </asp:View>
    </div>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Write a comment" />
        </p>
        <p>
            <asp:TextBox ID="CommentBox" runat="server" Width="500px" Height="150" TextMode="MultiLine"></asp:TextBox>
        </p>
        <p>
            <asp:LinkButton ID="CommentButton" runat="server" OnClick="CommentButton_Click">Post comment</asp:LinkButton>
            <asp:LinkButton ID="ResetButton" runat="server" OnClick="ResetButton_Click">Reset</asp:LinkButton>
        </p>
        <div>
            
        </div>
    </div>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" Runat="Server">
    <script src="<%= ResolveClientUrl("~/Scripts/delete-confirm.js") %>" type="text/javascript"></script>
</asp:Content>--%>
