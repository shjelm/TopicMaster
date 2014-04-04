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
                    int postId = Convert.ToInt32(Request.QueryString["id"]);

                    Service service = new Service();
                    post = service.GetPostByPostId(postId);

                    if ((int)Membership.GetUser().ProviderUserKey == post.MemberId 
                        || Roles.IsUserInRole("administrator"))
                    {
                        SaveButton.Visible = true;
                        CancelButton.Visible = true;
                    }
                }
                catch
                {
                    //Empty. To "eat" exception.
                }

                if (post != null)
                {
                    EditValue.Text = post.Value;
                }
                else
                {
                    Response.Redirect("~/NotFound.aspx", false);
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Post post = null;
                try
                {
                    int postId = Convert.ToInt32(Request.QueryString["id"]);

                    Service service = new Service();
                    post = service.GetPostByPostId(postId);

                    post.Value = EditValue.Text;
                    post.MemberId = (int)Membership.GetUser().ProviderUserKey;

                    if (!post.IsValid)
                    {
                        AddErrorMessage(post);
                        return;
                    }

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
                ValidationGroup = "EditPostVg"
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
