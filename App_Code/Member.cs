using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Text.RegularExpressions;

    public class Member : BusinessObjectBase, ICloneable
    {
        #region Fält

        private string _username;


        #endregion

        #region Konstruktorer

        public Member()
        {
            this.Username = null;
        }

        #endregion

        #region Egenskaper
        public int userId { get; set; }

        public string Username
        {
            get { return this._username; }
            set
            {
                this.ValidationErrors.Remove("Username");

                if (String.IsNullOrWhiteSpace(value))
                {
                    this.ValidationErrors.Add("Username", "You need to enter a username");
                }
                else if (value.Length > 30)
                {
                    this.ValidationErrors.Add("Username", "Your username is too long");
                }
                this._username = value != null ? value.Trim() : null;
            }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
