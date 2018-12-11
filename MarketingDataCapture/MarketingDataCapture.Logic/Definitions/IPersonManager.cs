namespace SaatchiDataCapture.Logic.Definitions
{
    using SaatchiDataCapture.Models;

    /// <summary>
    /// Describes the operations of the <see cref="Person" /> manager.
    /// </summary>
    public interface IPersonManager
    {
        /// <summary>
        /// Creates a <see cref="Person" />.
        /// </summary>
        /// <param name="person">
        /// An instance of <see cref="Person" />.
        /// </param>
        void Create(Person person);

        /// <summary>
        /// Updates a <see cref="Person" />.
        /// </summary>
        /// <param name="person">
        /// An instance of <see cref="Person" />.
        /// </param>
        /// <param name="updateEmailAddressVerification">
        /// A flag indicating whether or not to update
        /// <see cref="ContactDetail.EmailVerificationCompletion" />.
        /// <see cref="ContactDetail.EmailVerificationCompletion" /> is valid
        /// to be null (indicating it's still not verified) or to have a
        /// DateTime describing when it was verified.
        /// </param>
        void Update(Person person, bool updateEmailAddressVerification);
    }
}