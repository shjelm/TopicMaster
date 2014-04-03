using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

/// <summary>
/// Summary description for SavedEventArgs
/// </summary>
public class SavedEventArgs : EventArgs
{
        #region Fält

        private Post _post;
        private Comment _comment;

        #endregion

        #region Egenskaper

        public Post Post
        {
            get
            {
                return this._post != null ? this._post.Clone() as Post : null;
            }

            private set
            {
                this._post = value != null ? value.Clone() as Post : null;
            }
        }
        public Comment Comment
        {
            get
            {
                return this._comment != null ? this._comment.Clone() as Comment : null;
            }

            private set
            {
                this._comment = value != null ? value.Clone() as Comment : null;
            }
        }

        #endregion

        #region Konstruktorer

        public SavedEventArgs(Post post)
        {
            Post = post;
        }

        public SavedEventArgs(Comment comment)
        {
            Comment = comment;
        }

        #endregion
    }