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

    // Definierar publika händelsemedlemmar.
    public event SavedEventHandler Saved;
    public event EventHandler Canceled;

    public string Value
    {
        get { return EmailEdit.Text; }
        set { EmailEdit.Text = value; }
    }

    protected override void OnInit(EventArgs e)
    {
        // Låt sidan veta att "control state" används.
        Page.RegisterRequiresControlState(this);
        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        // Om valideringen är OK så...
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
                    // ...visas ett felmeddelande.
                    AddErrorMessage("An error occured while inserting post");
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
        // Om någon abbonerar på händelsen Canceled...
        if (Canceled != null)
        {
            // ...utlöses händelsen Canceled.
            Canceled(this, EventArgs.Empty);
        }
        else
        {
            Response.Redirect("~/", false);
        }
    }
    #region Privata hjälpmetoder

    /// <summary>
    /// Lägger till ett CustomValidator-objekt till samlingen ValidatorCollection.
    /// </summary>
    /// <param name="message">Felmeddelande som ska visas av en ValidationSummary-kontroll.</param>
    private void AddErrorMessage(string message)
    {
        var validator = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = message,
            ValidationGroup = "vgPost"
        };

        Page.Validators.Add(validator);
    }

    /// <summary>
    /// Går igenom samtliga publika egenskaper för obj och undersöker om felmeddelande finns som i så
    /// fall läggs till samlingen ValidatorCollection.
    /// </summary>
    /// <param name="obj">Referens till affärslogikobjekt.</param>
    private void AddErrorMessage(IDataErrorInfo obj)
    {
        // Hämtar och loopar igenom samtliga publika, icke statiska, egenskaper objektet har.
        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            // Finns det ett felmeddelande associerat med egenskapens namn?
            if (!String.IsNullOrWhiteSpace(obj[property.Name]))
            {
                // Överför meddelandet till samlingen ValidatorCollection.
                AddErrorMessage(obj[property.Name]);
            }
        }
    }
}

    #endregion