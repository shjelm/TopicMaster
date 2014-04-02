using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Reflection;

namespace TopicMaster
{
    public partial class Create : System.Web.UI.Page
    {
        protected void PostEdit_Saved(object sender, SavedEventArgs e)
        {
            // Kunduppgifterna sparade varför användaren dirigeras till en
            // rättmeddelandesida.
            string url = String.Format("~/Success.aspx?returnUrl=~/PostDetails.aspx?id={0}&action=Post_Saved",
                e.Post.PostId);
            Response.Redirect(url, false);
        }

        protected void PostEdit_Canceled(object sender, EventArgs e)
        {
            // Kunduppgifterna inte sparade varför användaren dirigeras till startsidan.
            Response.Redirect("~/", false);
        }
        #region Händelser

        // Definierar ett nytt delegat som representerar signaturen som
        // händelsen Saved har.
        public delegate void SavedEventHandler(object sender, SavedEventArgs e);

        // Definierar publika händelsemedlemmar.
        public event SavedEventHandler Saved;
        public event EventHandler Canceled;

        #endregion

        #region Fält

        private int _postId;
        private int _memberId;

        #endregion

        #region Egenskaper

        //public bool PostVisible
        //{
        //    get { return CommentEdit1.Visible; }
        //    set { CommentEdit1.Visible = value; }
        //}

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
            get { return PostValueTextBox.Text; }
            set { PostValueTextBox.Text = value; }
        }
        #endregion

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Om valideringen är OK så...
            if (Page.IsValid)
            {
                try
                {
                    MemberId = (int)Membership.GetUser().ProviderUserKey;
                    // ...skapa ett nytt Member-objekt och initiera det
                    // med värdena från textfälten och...
                    Post post = new Post
                    {
                        MemberId = MemberId,
                        Value = Value,
                        PostId = PostId
                    };

                     

                    // ...veriferera att objektet uppfyller affärsreglerna...
                    if (!post.IsValid)
                    {
                        // ...visa felmeddelanden om vad som
                        // orsakade att valideringen misslyckades.
                        AddErrorMessage(post);
                        return;
                    }

                    // ...spara objektet.
                    Service service = new Service();
                    service.SavePost(post);

                    Response.Redirect("~/Success.aspx", false);

                    // Om någon abbonerar på händelsen Saved...
                    if (Saved != null)
                    {
                        // ...utlöses händelsen Saved och skickar med
                        // en referens till kunduppgifterna som sparats.
                        Saved(this, new SavedEventArgs(post));

                        Response.Redirect("~/Success.aspx", false);
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
        }
    }