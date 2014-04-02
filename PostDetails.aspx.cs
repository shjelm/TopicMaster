using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;
using Resources;

public partial class PostDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                    EditButton.Visible = true;
                    DeleteButton.Visible = true;
                }
            }
            catch
            {
                // Tom! "Äter upp" bara upp det eventuella undantaget!
            }

            // ...kontrollera om det verkligen finns några kunduppgifter, i så fall så...
            if (post != null)
            {

                // ...presentera dem.
                PostLabel.Text = Server.HtmlEncode(post.Value);

                // Kundnumret skickas med som en "querystring"-variabel.
                EditButton.PostBackUrl = String.Format("~/Edit.aspx?id={0}", post.PostId);
                EditButton.Enabled = true;

                //CommentButton.PostBackUrl = String.Format("~/Comment.aspx?id={0}", post.PostId);
                //CommentButton.Enabled = true;

                // Användaren måste bekräfta att kunduppgifterna ska tas bort. Kundnumret skickas med
                // som ett argument till händelsen Command (inte Click!).
                // *** Skulle kunna ersättas med ett dolt fält - RegisterHiddenField. ***
                DeleteButton.CommandArgument = post.PostId.ToString();
                DeleteButton.Enabled = true;

                // Användar unobtrusive JavaScript istället för följande två rader.
                string prompt = String.Format("return confirm(\"{0}\");", Strings.Member_Delete_Confirm);
                DeleteButton.OnClientClick = String.Format(prompt, post.Value);

                DeleteButton.CssClass = "delete-action";
                DeleteButton.Attributes.Add("data-type", Strings.Data_Type_Post);
                DeleteButton.Attributes.Add("data-value", post.Value);
            }
            else
            {
                // ...om inga kunduppgifter kunde hittas dirigeras 
                // användaren till en meddelandesida.
                Response.Redirect("~/NotFound.aspx", false);
            }
        }


        private int _postId;
        private int _memberId;
        private int _commentId;

        public int PostId
        {
            get { return this._postId; }
            set { this._postId = value; }
        }

        public int CommentId
        {
            get { return this._commentId; }
            set { this._commentId = value; }
        }

        public int MemberId
        {
            get { return this._memberId; }
            set { this._memberId = value; }
        }

        public string Value
        {
            get { return CommentBox.Text; }
            set { CommentBox.Text = value; }
        }

    public delegate void SavedEventHandler(object sender, SavedEventArgs e);

        // Definierar publika händelsemedlemmar.
        public event SavedEventHandler Saved;
        public event EventHandler Canceled;

        protected void DeleteButton_Command(object sender, CommandEventArgs e)
        {
            try
            {
                // Kunduppgifterna tas bort och användaren dirigeras till en
                // rättmeddelandesida, eller så...
                Service service = new Service();
                service.DeletePost(Convert.ToInt32(e.CommandArgument));
                Response.Redirect("~/Default.aspx");
            }
            catch
            {
                // ...visas ett felmeddelande.
                var validator = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = Strings.Post_Deleting_Error
                };

                Page.Validators.Add(validator);
            }        
            
        }
        protected void ResetButton_Click(object sender, EventArgs e)
        {
            CommentBox.Text = "";
        }

        protected void CommentButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    MembershipUser User = Membership.GetUser();

                    MemberId = (int)User.ProviderUserKey;

                    PostId = Convert.ToInt32(Request.QueryString["id"]);

                    Comment comment = new Comment
                    {
                        MemberId = MemberId,
                        PostId = PostId,
                        Value = Value,
                        CommentId = CommentId
                    };

                    

                    // ...veriferera att objektet uppfyller affärsreglerna...
                    if (!comment.IsValid)
                    {
                        // ...visa felmeddelanden om vad som
                        // orsakade att valideringen misslyckades.
                        AddErrorMessage(comment);
                        return;
                    }

                    // ...spara objektet.
                    Service service = new Service();
                    service.SaveComment(comment);

                    CommentBox.Text = "";
                    Response.Redirect(String.Format("~/PostDetails.aspx?id={0}"), false);


                    // Om någon abbonerar på händelsen Saved...
                    if (Saved != null)
                    {
                        // ...utlöses händelsen Saved och skickar med
                        // en referens till kunduppgifterna som sparats.
                        Saved(this, new SavedEventArgs(comment));

                        Response.Redirect(String.Format("~/PostDetails.aspx?id={0}"), false);
                    }
                }
                catch
                {
                    // ...visas ett felmeddelande.
                    AddErrorMessage("An error occured while inserting post");
                }


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

        #region Händelser

        public event EventHandler PostChanged;

        #endregion

        
        #region PostDataSource

        /// <summary>
        /// TODO: Skriv beskrivning till PostDataSource_Selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage(Strings.Post_Selecting_Error);
                e.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// TODO: Skriv beskrivning till PostDataSource_Inserting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            // e.InputParameters[0] refererar till ett Member-objekt som blivit skapat och initierat
            // med de värden som textfälten innhåller hos klienten; typomvandlas till en referens av typen Post.
            var comment = e.InputParameters[0] as Comment;

            // Om contact refererar till null eller om Post-objektet inte klarar valideringen så
            // visa ett felmeddelande, och hindra händelsen från att forsätta, d.v.s. det kommer inte ske någon INSERT i databasen.
            if (comment == null)
            {
                AddErrorMessage(Strings.Post_Inserting_Unexpected_Error);
                e.Cancel = true;
            }
            else if (!comment.IsValid)
            {
                AddErrorMessage(comment);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// TODO: Skriv beskrivning till PostDataSource_Inserted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage(Strings.Post_Inserting_Error);
                e.ExceptionHandled = true;
            }
            else
            {
                if (PostChanged != null)
                {
                    PostChanged(this, EventArgs.Empty);
                }

                // Kunduppgiften sparad varför användaren dirigeras till en rättmeddelandesida.
                string url = String.Format("~/Success.aspx?returnUrl=~/Edit.aspx?id={0}&action=Post_Saved",
                    Request.QueryString["id"]);
                Response.Redirect(url, false);
            }
        }

        /// <summary>
        /// TODO: Skriv beskrivning till PostDataSource_Updating.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            // e.InputParameters[0] refererar till ett Member-objekt som blivit skapat och initierat
            // med de värden som textfälten innhåller hos klienten; typomvandlas till en referens av typen Post.
            var comment = e.InputParameters[0] as Comment;

            // Om contact refererar till null eller om Post-objektet inte klarar valideringen så
            // visa ett felmeddelande, och hindra händelsen från att forsätta, d.v.s. det kommer inte ske någon UPDATE i databasen.
            if (comment == null)
            {
                AddErrorMessage(Strings.Post_Updating_Unexpected_Error);
                e.Cancel = true;
            }
            else if (!comment.IsValid)
            {
                AddErrorMessage(comment);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// TODO: Skriv beskrivning till PostDataSource_Updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage(Strings.Post_Updating_Error);
                e.ExceptionHandled = true;
            }
            else
            {
                if (PostChanged != null)
                {
                    PostChanged(this, EventArgs.Empty);
                }

                // Kunduppgiften sparad varför användaren dirigeras till en rättmeddelandesida.
                string url = String.Format("~/Success.aspx?returnUrl=~/Edit.aspx?id={0}&action=Post_Saved",
                    Request.QueryString["id"]);
                Response.Redirect(url, false);
            }
        }

        /// <summary>
        /// TODO: Skriv beskrivning till PostDataSource_Deleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostDataSource_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            // Kastades inget ett undatag refererar egenskapen Exception till null och
            // ett rättmeddelande ska visas annars ska ett felmeddelande ska visas och
            // ramverket meddelas att undantaget är hanterat varför ramverket inte behöver göra något.
            if (e.Exception != null)
            {
                AddErrorMessage(Strings.Post_Deleting_Error);
                e.ExceptionHandled = true;
            }
            else
            {
                if (PostChanged != null)
                {
                    PostChanged(this, EventArgs.Empty);
                }

                // Kunduppgiften borttagen varför användaren dirigeras till en rättmeddelandesida.
                string url = String.Format("~/Success.aspx?returnUrl=~/Edit.aspx?id={0}&action=Post_Deleted",
                    Request.QueryString["id"]);
                Response.Redirect(url, false);
            }
        }

        #endregion

        #region PostTypeDataSource

        /// <summary>
        /// TODO: Skriv beskrivning till PostTypeDataSource_Selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostTypeDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage(Strings.Post_Selecting_Error);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region PostListView

        /// <summary>
        /// TODO: Skriv beskrivning till PostListView_ItemInserting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PostListView_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Values["MemberId"] = Request.QueryString["id"];
        }

        #endregion

    }