using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //To encrypt connectionString

        //Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        //ConnectionStringsSection connectionSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
        //connectionSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
        //config.Save();

    }
}
