using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;

public partial class Account_ChangeEmail : System.Web.UI.Page
{
    public delegate void SavedEventHandler(object sender, SavedEventArgs e);

    public event SavedEventHandler Saved;
    public event EventHandler Canceled;

    public string Value
    {
        get { return EmailEdit.Text; }
        set { EmailEdit.Text = value; }
    }

    protected override void OnInit(EventArgs e)
    {
        Page.RegisterRequiresControlState(this);
        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (EmailEdit.Text != null)
            {
                try
                {
                    int userId = (int)Membership.GetUser().ProviderUserKey;

                    MembershipUser UserToUpdate = Membership.GetUser(userId);
                    UserToUpdate.Email = EmailEdit.Text;

                    Membership.UpdateUser(UserToUpdate);
                    UpdateEmail.Visible = true;
                }
                catch
                {
                    AddErrorMessage("An error occured while updating your email.");
                }
            }
            else
            {
                AddErrorMessage("You must enter an email.");
            }
        }

    }
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        if (Canceled != null)
        {
            Canceled(this, EventArgs.Empty);
        }
        else
        {
            Response.Redirect("~/", false);
        }
    }
    #region Privata hjälpmetoder

    private void AddErrorMessage(string message)
    {
        var validator = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = message,
            ValidationGroup = "ChangeEmailVg"
        };

        Page.Validators.Add(validator);
    }

    private void AddErrorMessage(IDataErrorInfo obj)
    {
        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (!String.IsNullOrWhiteSpace(obj[property.Name]))
            {
                AddErrorMessage(obj[property.Name]);
            }
        }
    }
}

    #endregion