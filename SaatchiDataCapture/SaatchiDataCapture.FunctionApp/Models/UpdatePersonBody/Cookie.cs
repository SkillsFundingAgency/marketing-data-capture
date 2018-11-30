namespace SaatchiDataCapture.FunctionApp.Models.UpdatePersonBody
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a cookie.
    /// </summary>
    public class Cookie : ModelsBase
    {
        /// <summary>
        /// Gets or sets when the cookie was captured.
        /// </summary>
        [Required]
        public DateTime? Captured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cookie's identifier.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string CookieIdentifier
        {
            get;
            set;
        }
    }
}