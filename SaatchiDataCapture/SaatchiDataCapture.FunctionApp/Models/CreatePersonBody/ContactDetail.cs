namespace SaatchiDataCapture.FunctionApp.Models.CreatePersonBody
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents contact detail.
    /// </summary>
    public class ContactDetail : ModelsBase
    {
        /// <summary>
        /// Gets or sets when the detail was captured.
        /// </summary>
        [Required]
        public DateTime Captured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required]
        public string EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets when the email address was verified.
        /// If null, then the email address has not been verified.
        /// </summary>
        public DateTime? EmailVerificationCompletion
        {
            get;
            set;
        }
    }
}