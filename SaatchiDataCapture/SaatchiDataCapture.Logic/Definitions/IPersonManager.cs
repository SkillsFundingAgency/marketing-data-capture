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
        void Update(Person person);
    }
}