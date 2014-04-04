<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create.aspx.cs" Inherits="TopicMaster.Create" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ValidationSummary ID="PostValidationSummary" runat="server" HeaderText="Correct what's wrong"
    CssClass="validation-summary-errors-icon" ValidationGroup="CreatePostVg" />
    <h1>Create post</h1>

    <asp:Label ID="Label1" runat="server" Text="Enter your post:"></asp:Label>
    <p>
        <asp:TextBox ID="PostValueTextBox" runat="server" textMode="MultiLine" height="100px" width="500px"></asp:TextBox></p>
    <p>
    <p>Enter the text below:
    <asp:Label ID="lblmsg" runat="server" Font-Bold="True" 
	ForeColor="Red" Text=""></asp:Label>
         </p>
    <asp:TextBox ID="txtimgcode" runat="server"></asp:TextBox>
    <br />
    <asp:Image ID="Image1" runat="server" ImageUrl="~/CImage.aspx"/>
    <br />
        <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click" ValidationGroup="CreatePostVg">Save</asp:LinkButton>
        <asp:LinkButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" CausesValidation="false">Cancel</asp:LinkButton>
    </p>
</asp:Content>
