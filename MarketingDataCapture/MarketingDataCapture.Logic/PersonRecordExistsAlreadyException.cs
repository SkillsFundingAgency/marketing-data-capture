namespace SaatchiDataCapture.Logic
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using SaatchiDataCapture.Models;

    /// <summary>
    /// Custom exception thrown upon attempting to insert a duplicate
    /// <see cref="ContactDetail" /> instance, by email address.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1032",
        Justification = "We will not be serialising this exception - therefore a parameterless constructor is not required.")]
    public class PersonRecordExistsAlreadyException : DataException
    {
        private const string MessageTemplate =
            "A ContactDetail instance already exists with the email " +
            "address \"{0}\".";

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="PersonRecordExistsAlreadyException" /> class.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address causing the exception.
        /// </param>
        public PersonRecordExistsAlreadyException(string emailAddress)
            : base(
                string.Format(
                    CultureInfo.InvariantCulture,
                    MessageTemplate,
                    emailAddress))
        {
            // Nothing - just inherits what it needs.
        }
    }
}