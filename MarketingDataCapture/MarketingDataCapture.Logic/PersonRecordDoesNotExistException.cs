namespace MarketingDataCapture.Logic
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using MarketingDataCapture.Models;

    /// <summary>
    /// Custom exception thrown upon attempting to update a
    /// <see cref="ContactDetail" /> instance that does not exist.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1032",
        Justification = "We will not be serialising this exception - therefore a parameterless constructor is not required.")]
    public class PersonRecordDoesNotExistException : DataException
    {
        private const string MessageTemplate =
            "Could not find a ContactDetail instance with the email address " +
            "\"{0}\".";

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="PersonRecordDoesNotExistException" /> class.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address causing the exception.
        /// </param>
        public PersonRecordDoesNotExistException(string emailAddress)
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