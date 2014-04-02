using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.ComponentModel;
using System.Reflection;

    public partial class Edit : System.Web.UI.Page
    {
        public delegate void SavedEventHandler(object sender, SavedEventArgs e);

        // Definierar publika händelsemedlemmar.
        public event SavedEventHandler Saved;
        public event EventHandler Canceled;

        private int _postId;
        private int _memberId;

        public int PostId
        {
            get { return this._postId; }
            set { this._postId = value; }
        }

        public int MemberId
        {
            get { return this._memberId; }
            set { this._memberId = value; }
        }

        public string Value
        {
            get { return EditValue.Text; }
            set { EditValue.Text = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            // Låt sidan veta att "control state" används.
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Post post = null;
                try
                {
                    // ...hämta kundnumret från "query string"-variabeln,...
                    int postId = Convert.ToInt32(Request.QueryString["id"]);

                    // ...hämta kunduppgifterna och...
                    Service service = new Service();
                    post = service.GetPostByPostId(postId);

                    if ((int)Membership.GetUser().ProviderUserKey == post.MemberId || Roles.IsUserInRole("administrator"))
                    {
                        SaveButton.Visible = true;
                        CancelButton.Visible = true;
                    }
                }
                catch
                {
                    // Tom! "Äter upp" bara upp undantaget!
                }

                // ...kontrollera om det verkligen finns några kunduppgifter, i så fall så...
                if (post != null)
                {
                    EditValue.Text = post.Value;
                }
                else
                {
                    // ...om inga kunduppgifter kunde hittas dirigeras 
                    // användaren till en meddelandesida.
                    Response.Redirect("~/NotFound.aspx", false);
                }
            }
        }

        protected void MemberEdit_Saved(object sender, SavedEventArgs e)
        {
            // Kunduppgifterna sparade varför användaren dirigeras till en
            // rättmeddelandesida.
            Response.Redirect(String.Format("~/Success.aspx?returnUrl=Details.aspx?id={0}&action=Post_Saved",
                e.Post.PostId), false);
        }

        protected void MemberEdit_Canceled(object sender, EventArgs e)
        {
            // Kunduppgifterna inte sparade varför användaren dirigeras till detaljsidan.
            Response.Redirect(String.Format("~/Details.aspx?id={0}", Convert.ToInt32(Request.QueryString["id"])), false);
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Om valideringen är OK så...
            if (Page.IsValid)
            {
                Post post = null;
                try
                {
                    int postId = Convert.ToInt32(Request.QueryString["id"]);

                    // ...hämta kunduppgifterna och...
                    Service service = new Service();
                    post = service.GetPostByPostId(postId);

                    // ...skapa ett nytt Member-objekt och initiera det
                    // med värdena från textfälten och...
                    post.Value = EditValue.Text;
                    post.MemberId = (int)Membership.GetUser().ProviderUserKey;

                    // ...veriferera att objektet uppfyller affärsreglerna...
                    if (!post.IsValid)
                    {
                        // ...visa felmeddelanden om vad som
                        // orsakade att valideringen misslyckades.
                        AddErrorMessage(post);
                        return;
                    }

                    // ...spara objektet.
                    service.SavePost(post);

                    Response.Redirect("~/ViewPosts.aspx", false);

                    // Om någon abbonerar på händelsen Saved...
                    if (Saved != null)
                    {
                        // ...utlöses händelsen Saved och skickar med
                        // en referens till kunduppgifterna som sparats.
                        Saved(this, new SavedEventArgs(post));

                        Response.Redirect("~/ViewPosts.aspx", false);
                    }
                }
                catch
                {
                    // ...visas ett felmeddelande.
                    AddErrorMessage("An error occured while inserting post");
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

        #endregion

        protected override void LoadControlState(object savedState)
        {
            if (savedState != null)
            {
                Pair p = savedState as Pair;
                if (p != null)
                {
                    base.LoadControlState(p.First);
                    this._postId = (int)p.Second;
                }
                else
                {
                    if (savedState is int)
                    {
                        this._postId = (int)savedState;
                    }
                    else
                    {
                        base.LoadControlState(savedState);
                    }
                }
            }
        }

        protected override object SaveControlState()
        {
            object obj = base.SaveControlState();

            if (this._postId != 0)
            {
                if (obj != null)
                {
                    return new Pair(obj, this._memberId);
                }
                else
                {
                    return (this._postId);
                }
            }
            else
            {
                return obj;
            }
        }
    }
