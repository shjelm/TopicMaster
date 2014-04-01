using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Resources;

    public class Member : BusinessObjectBase, ICloneable
    {
        #region Fält

        private string _name;
        private string _username;
        private string _mail;
        private string _password; 


        #endregion

        #region Konstruktorer

        public Member()
        {
            this.Name = null;
            this.Username = null;
            this.Mail = null;
            this.Password = null;
        }

        #endregion

        #region Egenskaper

        // Egenskapernas namn och typ ges av tabellen
        // Member i databasen.
        public int MemberId { get; set; }

        public string Name
        {
            get { return this._name; }
            set
            {
                // Antar att värdet är korrekt.
                this.ValidationErrors.Remove("Name");

                // Undersöker om värdet är korrekt beträffande om strängen är null
                // eller tom för i så fall...
                if (String.IsNullOrWhiteSpace(value))
                {
                    // ...är det ett fel varför nyckel Name (namnet på egenskapen)
                    // mappas mot ett felmeddelande.
                    this.ValidationErrors.Add("Name", Strings.Member_Name_Required);
                }
                else if (value.Length > 30)
                {
                    // Om strängen innehåller fler än 30 tecken kan inte det fullständiga 
                    // datat inte sparas i databastabellen vilket är att betrakta som ett fel.
                    this.ValidationErrors.Add("Name", Strings.Member_Name_MaxLength);
                }

                // Tilldelar fältet värdet, oavsett om det är ett korrekt värde 
                // enligt affärsreglerna eller inte.
                this._name = value != null ? value.Trim() : null;
            }
        }

        public string Username
        {
            get { return this._username; }
            set
            {
                // Antar att värdet är korrekt.
                this.ValidationErrors.Remove("Username");

                // Undersöker om värdet är korrekt beträffande om strängen är null
                // eller tom för i så fall...
                if (String.IsNullOrWhiteSpace(value))
                {
                    // ...är det ett fel varför nyckel Username (namnet på egenskapen)
                    // mappas mot ett felmeddelande.
                    this.ValidationErrors.Add("Username", Strings.Member_Username_Required);
                }
                else if (value.Length > 30)
                {
                    // Om strängen innehåller fler än 30 tecken kan inte det fullständiga 
                    // datat inte sparas i databastabellen vilket är att betrakta som ett fel.
                    this.ValidationErrors.Add("Username", Strings.Member_Username_MaxLength);
                }

                // Tilldelar fältet värdet, oavsett om det är ett korrekt värde 
                // enligt affärsreglerna eller inte.
                this._username = value != null ? value.Trim() : null;
            }
        }

        public string Mail
        {
            get { return this._mail; }
            set
            {
                // Antar att värdet är korrekt.
                this.ValidationErrors.Remove("Mail");

                // Undersöker om värdet är korrekt beträffande om strängen är null
                // eller tom för i så fall...
                if (String.IsNullOrWhiteSpace(value))
                {
                    // ...är det ett fel varför nyckel Mail (namnet på egenskapen)
                    // mappas mot ett felmeddelande.
                    this.ValidationErrors.Add("Mail", Strings.Member_Mail_Required);
                }
                else if (!Regex.IsMatch(value, Strings.Regular_Expression_Mail))
                {
                    // Om strängen inte är en emailadress
                    this.ValidationErrors.Add("Mail", Strings.Member_Mail_MaxLength);
                }

                // Tilldelar fältet värdet, oavsett om det är ett korrekt värde 
                // enligt affärsreglerna eller inte.
                this._mail = value != null ? value.Trim() : null;
            }
        }

        //@TODO: Kryptera lösenord
        public string Password
        {
            get { return this._password; }
            set
            {
                // Antar att värdet är korrekt.
                this.ValidationErrors.Remove("Password");

                // Undersöker om värdet är korrekt beträffande om strängen är null
                // eller tom för i så fall...
                if (String.IsNullOrWhiteSpace(value))
                {
                    // ...är det ett fel varför nyckel Password (namnet på egenskapen)
                    // mappas mot ett felmeddelande.
                    this.ValidationErrors.Add("Password", Strings.Member_Password_Required);
                }
                else if (value.Length > 20)
                {
                    // Om strängen innehåller fler än 30 tecken kan inte det fullständiga 
                    // datat inte sparas i databastabellen vilket är att betrakta som ett fel.
                    this.ValidationErrors.Add("Password", Strings.Member_Password_MaxLength);
                }

                // Tilldelar fältet värdet, oavsett om det är ett korrekt värde 
                // enligt affärsreglerna eller inte.
                this._password = value != null ? value.Trim() : null;
                
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
