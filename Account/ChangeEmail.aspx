<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeEmail.aspx.cs" Inherits="Account_ChangeEmail" %>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h2>
        Change Email
    </h2>
    <p>
        Use the form below to change your email.
    </p>
    <asp:ValidationSummary ID="ChangeUserEmailValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="ChangeEmailVg" />
    <p><asp:Label ID="UpdateEmail" runat="server" Visible="false" Text="Your email was updated."></asp:Label></p>
    <asp:Label ID="Label1" runat="server" Text="New email:"></asp:Label>
    <asp:TextBox ID="EmailEdit" runat="server" ValidationGroup="ChangeEmailVg"></asp:TextBox>
    <asp:RequiredFieldValidator ID="NewEmailRequired" runat="server" ControlToValidate="EmailEdit" 
                             CssClass="failureNotification" ErrorMessage="An email is required." ToolTip="An email is required." 
                             ValidationGroup="ChangeEmailVg" SetFocusOnError=true>*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="You must enter a valid email."
        Display="Dynamic" Text="*" ControlToValidate="EmailEdit" SetFocusOnError="True"
        ValidationExpression="^[a-zA-Z0-9.!#$%&amp;'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$"
        CssClass="field-validation-error" ValidationGroup="ChangeEmailVg" />
    <p>
        <asp:LinkButton ID="SaveButton" runat="server"  OnClick="SaveButton_Click" ValidationGroup="ChangeEmailVg" >Save</asp:LinkButton>
        <asp:LinkButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" CausesValidation="false">Cancel</asp:LinkButton>
    </p>
</asp:Content>
