using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

    public abstract class BusinessObjectBase : IDataErrorInfo
    {
        #region Fält

        // Fält som används för hantering av validering.
        private Dictionary<string, string> _validationErrors;
        protected string CommonErrorMessage;

        #endregion

        #region Konstruktorer

        public BusinessObjectBase()
            : this("The objects status is invalid")
        {
            //Empty.
        }

        public BusinessObjectBase(string commonErrorMessage)
        {
            this.CommonErrorMessage = commonErrorMessage;
        }

        #endregion

        #region Egenskaper - validering

        protected Dictionary<string, string> ValidationErrors
        {
            get
            {
                return this._validationErrors ?? (this._validationErrors = new Dictionary<string, string>());
            }
        }

        public bool IsValid
        {
            get { return !this.ValidationErrors.Any(); }
        }

        public string Error
        {
            get
            {
                if (!this.IsValid)
                {
                    return this.CommonErrorMessage;
                }

                return null;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                string error;
                if (this.ValidationErrors.TryGetValue(propertyName, out error))
                {
                    return error;
                }

                return null;
            }
        }

        #endregion
    }