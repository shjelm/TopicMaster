using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Resources;

    public class Comment : BusinessObjectBase, ICloneable
        {
            #region Fält

            // Autoimplementerade egenskaper kan inte användas i och med implementeringen
            // av valideringen varför privata fält kopplade till egenskaperna behövs.
            private string _value;

            #endregion

            #region Konstruktorer

            public Comment()
            {
                // Ombesörjer att objektets alla värden valideras genom att använda
                // egenskaperna istället för fälten direkt.
                this.MemberId = 0;
                this.PostId = 0;
                //this.PostTypeId = 0;
                this.Value = null;
            }

            #endregion

            #region Egenskaper - data

            // Primärnycklens värde, främmande nyklars värden, behöver inte valideras 
            // varför det går bra att använda en autoimplementerad egenskap.
            public int MemberId { get; set; }
            public int PostId { get; set; }
            public int CommentId { get; set; }
            //public int PostTypeId { get; set; }

            public string Value
            {
                get { return this._value; }
                set
                {
                    // Antar att värdet är korrekt.
                    base.ValidationErrors.Remove("Value");

                    // Undersöker om värdet är korrekt beträffande om strängen är null
                    // eller tom för i så fall...
                    if (String.IsNullOrEmpty(value))
                    {
                        // ...är det ett fel varför nyckeln Value (namnet på egenskapen)
                        // mappas mot ett felmeddelande.
                        base.ValidationErrors.Add("Value", Strings.Post_Value_Required);
                    }
                    else if (value.Length > 50)
                    {
                        // Om strängen innehåller fler än 50 tecken kan inte det fullständiga 
                        // datat inte sparas i databastabellen vilket är att betrakta som ett fel.
                        base.ValidationErrors.Add("Value", Strings.Post_Value_MaxLength);
                    }

                    // Tilldelar fältet värdet, oavsett om det är ett korrekt värde 
                    // enligt affärsreglerna eller inte.
                    this._value = value != null ? value.Trim() : null;

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