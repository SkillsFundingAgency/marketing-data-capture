namespace SaatchiDataCapture.FunctionApp.Models.CreatePerson
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a person.
    /// </summary>
    public class Person : ModelsBase
    {
        /// <summary>
        /// Gets or sets an instance of <see cref="CreatePerson.Consent" />.
        /// </summary>
        [Required]
        public Consent Consent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an instance of <see cref="CreatePerson.Cookie" />.
        /// </summary>
        [Required]
        public Cookie Cookie
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an instance of <see cref="CreatePerson.Route" />.
        /// </summary>
        [Required]
        public Route Route
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an instance of
        /// <see cref="CreatePerson.ContactDetail" />.
        /// </summary>
        [Required]
        public ContactDetail ContactDetail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="DateTime" /> in which the person was
        /// enrolled.
        /// </summary>
        [Required]
        public DateTime Enrolled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required]
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required]
        public string LastName
        {
            get;
            set;
        }
    }
}
