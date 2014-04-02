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
                // Skapar en kopia av objektet som _member refererar till. Undviker 
                // på så sätt en "privacy leak".
                return this._post != null ? this._post.Clone() as Post : null;
            }

            private set
            {
                // Skapar en kopia av objektet som value refererar till. Undviker 
                // på så sätt en "privacy leak".
                this._post = value != null ? value.Clone() as Post : null;
            }
        }
        public Comment Comment
        {
            get
            {
                // Skapar en kopia av objektet som _member refererar till. Undviker 
                // på så sätt en "privacy leak".
                return this._comment != null ? this._comment.Clone() as Comment : null;
            }

            private set
            {
                // Skapar en kopia av objektet som value refererar till. Undviker 
                // på så sätt en "privacy leak".
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