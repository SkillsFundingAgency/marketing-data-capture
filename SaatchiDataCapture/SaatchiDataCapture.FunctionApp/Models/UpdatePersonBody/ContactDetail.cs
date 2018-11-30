namespace SaatchiDataCapture.FunctionApp.Models.UpdatePersonBody
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents contact detail.
    /// </summary>
    public class ContactDetail : ModelsBase
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required]
        [MaxLength(256)]
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

        /// <summary>
        /// Gets or sets a value indicating whether or not to update
        /// <see cref="EmailVerificationCompletion" />.
        /// Note: This is required, as valid values for
        /// <see cref="EmailVerificationCompletion" /> are either null (to
        /// indicate that the email address has not been verified) or a
        /// <see cref="DateTime" /> (when the email address was verified).
        /// If <see cref="EmailVerificationCompletion" /> is simply not
        /// specified, but already has a <see cref="DateTime" /> value in the
        /// database, then this value would be overwritten with null.
        /// Therefore, this switch should be specified to force an update of
        /// the underlying data.
        /// Not set to nullable, as if not specified, this property will be
        /// set to false, which is a good default behaviour to go by!
        /// </summary>
        public bool UpdateEmailVerificationCompletion
        {
            get;
            set;
        }
    }
}