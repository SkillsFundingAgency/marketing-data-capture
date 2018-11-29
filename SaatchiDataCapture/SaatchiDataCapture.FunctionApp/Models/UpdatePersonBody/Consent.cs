namespace SaatchiDataCapture.FunctionApp.Models.UpdatePersonBody
{
    using System;

    /// <summary>
    /// Represents consent.
    /// </summary>
    public class Consent : ModelsBase
    {
        /// <summary>
        /// Gets or sets when the consent (or lack of) was declared.
        /// </summary>
        public DateTime GdprConsentDeclared
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not consent was given (or indeed, neither
        /// given or denied if null).
        /// </summary>
        public bool? GdprConsentGiven
        {
            get;
            set;
        }
    }
}