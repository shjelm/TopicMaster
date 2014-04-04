using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;

public partial class PostDetails : System.Web.UI.Page
    {
        public event EventHandler CommentChanged;

        public delegate void SavedEventHandler(object sender, SavedEventArgs e);

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

        protected void Page_Load(object sender, EventArgs e)
        {

            Post post = null;

            try
            {
                int postId = Convert.ToInt32(Request.QueryString["id"]);

                Service service = new Service();
                post = service.GetPostByPostId(postId);

                post.Author = service.GetUserName(post.MemberId);


                if ((int)Membership.GetUser().ProviderUserKey == post.MemberId || Roles.IsUserInRole("administrator"))
                {
                    EditPostButton.Visible = true;
                    DeletePostButton.Visible = true;
                    
                }
            }
            catch
            {
                //Empty. "Eats" the exception.
            }

            if (post != null)
            {
                PostLabel.Text = Server.HtmlEncode(post.Value);
                AuthorLabel.Text = Server.HtmlEncode(post.Author);

                EditPostButton.PostBackUrl = String.Format("~/Edit.aspx?id={0}", post.PostId);
                EditPostButton.Enabled = true;

                DeletePostButton.CommandArgument = post.PostId.ToString();
                DeletePostButton.Enabled = true;

                string prompt = String.Format("return confirm(\"{0}\");", "Are you sure you want to delete '{0}'?");
                DeletePostButton.OnClientClick = String.Format(prompt, post.Value);

                DeletePostButton.CssClass = "delete-action";
                DeletePostButton.Attributes.Add("data-type", "the post");
                DeletePostButton.Attributes.Add("data-value", post.Value);
            }
            else
            {
                Response.Redirect("~/NotFound.aspx", false);
            }
        }

        protected void DeletePostButton_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Service service = new Service();
                service.DeletePost(Convert.ToInt32(e.CommandArgument));
                Response.Redirect("~/Default.aspx");
            }
            catch
            {
                var validator = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "An error occured while deleing post."
                };

                Page.Validators.Add(validator);
            }        
            
        }

        protected void CommentDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage("An error occured while selecting post.");
                e.ExceptionHandled = true;
            }
        }

        protected void CommentDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var comment = e.InputParameters[0] as Comment;
            comment.MemberId = (int)Membership.GetUser().ProviderUserKey;
            comment.PostId = Convert.ToInt32(Request.QueryString["id"]);

            if (comment == null)
            {
                AddErrorMessage("An unexpected error occured");
                e.Cancel = true;
            }
            else if (!comment.IsValid)
            {
                AddErrorMessage(comment);
                e.Cancel = true;
            }
        }

        protected void CommentDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage("An error occured while inserting post");
                e.ExceptionHandled = true;
            }
            else
            {
                if (CommentChanged != null)
                {
                    CommentChanged(this, EventArgs.Empty);
                }

                string url = String.Format("~/PostDetails.aspx?id={0}",
                    Request.QueryString["id"]);
                Response.Redirect(url, false);
            }
        }

        protected void CommentDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var comment = e.InputParameters[0] as Comment;

            if (comment == null)
            {
                AddErrorMessage("An unexpected error occured");
                e.Cancel = true;
            }
            else if (!comment.IsValid)
            {
                AddErrorMessage(comment);
                e.Cancel = true;
            }
        }

        protected void CommentDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage("An error occured while updating the post.");
                e.ExceptionHandled = true;
            }
            else
            {
                if (CommentChanged != null)
                {
                    CommentChanged(this, EventArgs.Empty);
                }

                string url = String.Format("~/PostDetails.aspx?id={0}",
                    Request.QueryString["id"]);
                Response.Redirect(url, false);
            }
        }

        protected void CommentDataSource_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                AddErrorMessage("An error occured while deleting the post");
                e.ExceptionHandled = true;
            }
            else
            {
                if (CommentChanged != null)
                {
                    CommentChanged(this, EventArgs.Empty);
                }

                string url = String.Format("~/PostDetails.aspx?id={0}",
                    Request.QueryString["id"]);
                Response.Redirect(url, false);
            }
        }

        protected void CommentListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            LinkButton EditButton;
            LinkButton DeleteButton;

            EditButton = (LinkButton)e.Item.FindControl("EditButton");
            DeleteButton = (LinkButton)e.Item.FindControl("DeleteButton");

            Comment activeComment = (Comment)e.Item.DataItem;


            if (!Page.IsPostBack)
            {
                if (activeComment.MemberId == (int)Membership.GetUser().ProviderUserKey || Roles.IsUserInRole("administrator"))
                {
                    EditButton.Visible = true;
                    DeleteButton.Visible = true;
                }
            }

            Service service = new Service();
            string name = service.GetUserName(activeComment.MemberId);

            Label label = new Label();
            label = (Label)e.Item.FindControl("AuthorCommentLabel");
            if (label != null)
            {
                label.Text = name;
            }

            
        }
    
        
        #region Privata hjälpmetoder

        private void AddErrorMessage(string message)
        {
            var validator = new CustomValidator
            {
                IsValid = false,
                ErrorMessage = message,
                ValidationGroup = "PostDetailsVg"
            };

            Service.WriteToLog(message, (int)Membership.GetUser().ProviderUserKey);

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

        #endregion
 
    }