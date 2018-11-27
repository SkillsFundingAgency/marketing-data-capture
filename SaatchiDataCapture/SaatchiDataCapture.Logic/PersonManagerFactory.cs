namespace SaatchiDataCapture.Logic
{
    using SaatchiDataCapture.Data.Definitions;
    using SaatchiDataCapture.Logic.Definitions;

    /// <summary>
    /// Implements <see cref="IPersonManagerFactory" />.
    /// </summary>
    public class PersonManagerFactory : IPersonManagerFactory
    {
        private readonly IDataCaptureDatabaseAdapter dataCaptureDatabaseAdapter;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="PersonManagerFactory" /> class.
        /// </summary>
        /// <param name="dataCaptureDatabaseAdapter">
        /// An instance of type <see cref="IDataCaptureDatabaseAdapter" />.
        /// </param>
        public PersonManagerFactory(
            IDataCaptureDatabaseAdapter dataCaptureDatabaseAdapter)
        {
            this.dataCaptureDatabaseAdapter = dataCaptureDatabaseAdapter;
        }

        /// <inheritdoc />
        public IPersonManager Create(ILoggerProvider loggerProvider)
        {
            PersonManager toReturn = new PersonManager(
                this.dataCaptureDatabaseAdapter,
                loggerProvider);

            return toReturn;
        }
    }
}
