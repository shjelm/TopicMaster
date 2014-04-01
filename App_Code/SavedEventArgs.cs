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

        #endregion

        #region Konstruktorer

        public SavedEventArgs(Post post)
        {
            Post = post;
        }

        #endregion
    }