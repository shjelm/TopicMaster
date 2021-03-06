﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Register : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
    }

    protected void RegisterUser_CreatedUser(object sender, EventArgs e)
    {
        FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

        Roles.AddUserToRole(RegisterUser.UserName, "member");

        string continueUrl = RegisterUser.ContinueDestinationPageUrl;
        if (String.IsNullOrEmpty(continueUrl))
        {
            continueUrl = "~/";
        }
        Response.Redirect(continueUrl);
    }

    public void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
    {

        TextBox txtimgcode = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("txtimgcode");
        Label lblmsg = (Label)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("lblmsg");
        if (txtimgcode.Text == this.Session["CaptchaImageText"].ToString())
        {
            e.Cancel = false;
        }
        else
        {
            e.Cancel = true;
        }
        txtimgcode.Text = "";
    }
}
