using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

public class Post : BusinessObjectBase, ICloneable
{
    #region Fält

    private string _value;
    private string _author;

    #endregion

    #region Konstruktorer

    public Post()
    {   this.MemberId = 0;
        this.PostId = 0;
        this.Value = null;
        this.Author = null;
    }

    #endregion

    #region Egenskaper 

    public int MemberId { get; set; }
    public int PostId { get; set; }
    public string Author { get; set; }

    public string Value
    {
        get { return this._value; }
        set
        {
            base.ValidationErrors.Remove("Value");

            if (String.IsNullOrEmpty(value))
            {
                base.ValidationErrors.Add("Value", "You need to enter a value");
            }
            else if (value.Length > 500)
            {
                base.ValidationErrors.Add("Value", "Your post is too long");
            }
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

