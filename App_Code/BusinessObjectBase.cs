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
            : this("Objektets status är ogiltigt.")
        {
            // Tom.
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
                // Bara då _validationErrors refererar till null skapas ett nytt objekt
                return this._validationErrors ?? (this._validationErrors = new Dictionary<string, string>());
            }
        }

        public bool IsValid
        {
            // Finns det inga fel är Dictionary-objektet tomt.
            get { return !this.ValidationErrors.Any(); }
        }

        public string Error
        {
            get
            {
                // Kontrollerar om det finns några fel och...
                if (!this.IsValid)
                {
                    // ...i så fall returneras en allmän felbeskrivning.
                    return this.CommonErrorMessage;
                }
                // Finns det inga fel så returneras ingen sträng alls!
                return null;
            }
        }

        // Med hjälp av en referens till ett objekt är det fullt möjligt att
        // "fråga" objektet om en specifik egenskap orsakt ett fel, och i så fall
        // vilket.
        public string this[string propertyName]
        {
            get
            {
                // Försöker hitta ett fel för angiven egenskap och...
                string error;
                if (this.ValidationErrors.TryGetValue(propertyName, out error))
                {
                    // ...finns det ett fel så returneras felmeddelandet...
                    return error;
                }
                // ...annars returneras ingen sträng.
                return null;
            }
        }

        #endregion
    }