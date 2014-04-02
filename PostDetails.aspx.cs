using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

        protected void DeleteButton_Command(object sender, CommandEventArgs e)
        {
            try
            {
                // Kunduppgifterna tas bort och användaren dirigeras till en
                // rättmeddelandesida, eller så...
                Service service = new Service();
                service.DeleteMember(Convert.ToInt32(e.CommandArgument));
                Response.Redirect("~/Success.aspx?returnUrl=~/ShowPosts.aspx&action=Post_Deleted");
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
    }