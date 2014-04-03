using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Comment: BusinessObjectBase, ICloneable
{
    #region Fält

    private string _value;

    #endregion

    #region Konstruktorer

    public Comment()
    {
        this.MemberId = 0;
        this.PostId = 0;
        this.Value = null;
    }

    #endregion

    #region Egenskaper - data

    public int MemberId { get; set; }
    public int PostId { get; set; }
    public int CommentId { get; set; }

    public string Value
    {
        get { return this._value; }
        set
        {
            base.ValidationErrors.Remove("Value");

            if (String.IsNullOrEmpty(value))
            {
                base.ValidationErrors.Add("Value","You must enter a post");
            }
            else if (value.Length > 500)
            {
                base.ValidationErrors.Add("Value", "Your post is too long. Maximum 500 characters");
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
