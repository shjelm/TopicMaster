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
        #region Händelser

        public delegate void SavedEventHandler(object sender, SavedEventArgs e);

        public event SavedEventHandler Saved;
        public event EventHandler Canceled;

        #endregion

        #region Fält

        private int _postId;
        private int _memberId;
        private string _author;

        #endregion

        #region Egenskaper

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

        public string Author
        {
            get { return this._author; }
            set { this._author = value; }
        }
        #endregion

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    MemberId = (int)Membership.GetUser().ProviderUserKey;
                    Post post = new Post
                    {
                        MemberId = MemberId,
                        Value = Value,
                        PostId = PostId,
                        Author = Author

                    };

                    if (!post.IsValid)
                    {
                        AddErrorMessage(post);
                        return;
                    }

                    Service service = new Service();
                    service.SavePost(post);

                    Response.Redirect("~/ViewPosts.aspx", false);

                    if (Saved != null)
                    {
                        Saved(this, new SavedEventArgs(post));

                        Response.Redirect("~/ViewPosts.aspx", false);
                    }
                }
                catch
                {
                    AddErrorMessage("An error occured while inserting post");
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
                ValidationGroup = "CreatePostVg"
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

        #endregion
        }
    }