namespace SaatchiDataCapture.Models
{
    using System;

    /// <summary>
    /// Represents contact detail.
    /// </summary>
    public class ContactDetail
    {
        /// <summary>
        /// Gets or sets when the detail was captured.
        /// </summary>
        public DateTime Captured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
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