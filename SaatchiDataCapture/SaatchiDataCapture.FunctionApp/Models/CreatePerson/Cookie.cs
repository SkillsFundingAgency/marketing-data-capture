namespace SaatchiDataCapture.FunctionApp.Models.CreatePerson
{
    using System;

    /// <summary>
    /// Represents a cookie.
    /// </summary>
    public class Cookie : ModelsBase
    {
        /// <summary>
        /// Gets or sets when the cookie was captured.
        /// </summary>
        public DateTime Captured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cookie's identifier.
        /// </summary>
        public string CookieIdentifier
        {
            get;
            set;
        }
    }
}