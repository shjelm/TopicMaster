using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;
using System.Web.Security;

public partial class Administrators_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DeleteMemberButton_Command(object sender, CommandEventArgs e)
    {
        try
        {
            Membership.DeleteUser(e.CommandArgument.ToString());
            Response.Redirect("~/Administrators/Default.aspx");
        }
        catch
        {
            var validator = new CustomValidator
            {
                IsValid = false,
                ErrorMessage ="An error occured while deleting the member"
            };

            Page.Validators.Add(validator);
        }

    }
}