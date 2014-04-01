using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Management;
using MySql.Web.Security;


public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CreateHyperLink.Visible = User.Identity.IsAuthenticated;
        ViewPostsHyperLink.Visible = User.Identity.IsAuthenticated;
    }
}
