namespace MarketingDataCapture.FunctionApp.Models.UpdatePersonBody
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a person.
    /// </summary>
    public class Person : ModelsBase
    {
        /// <summary>
        /// Gets or sets an instance of <see cref="UpdatePersonBody.Consent" />.
        /// </summary>
        public Consent Consent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an instance of <see cref="UpdatePersonBody.Cookie" />.
        /// </summary>
        public Cookie Cookie
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an instance of <see cref="UpdatePersonBody.Route" />.
        /// </summary>
        public Route Route
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an instance of
        /// <see cref="UpdatePersonBody.ContactDetail" />.
        /// </summary>
        [Required]
        public ContactDetail ContactDetail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [MaxLength(256)]
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [MaxLength(256)]
        public string LastName
        {
            get;
            set;
        }
    }
}
