using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Account_Login : System.Web.UI.Page
{
    private bool AccountWasLocked = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == true)
        {
            MembershipUser tmp = Membership.GetUser(LoginUser.UserName);
            if (tmp != null)
                AccountWasLocked = tmp.IsLockedOut;
        }
        RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);   
    }

    protected void OnLoginError(object sender, EventArgs e)
    {
        Service.WriteToLog("Failed login:"+ LoginUser.UserName, 0);
    }

    protected void LoginUser_LoggedIn(object sender, EventArgs e)
    {
        if (LoginUser.UserName != null)
        {
            Service.WriteToLog("Successfull login:"+LoginUser.UserName, 0);
        }
        else
        {
            throw new ArgumentNullException();
        }
    }
}
