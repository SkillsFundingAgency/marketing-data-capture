namespace SaatchiDataCapture.Logic
{
    using SaatchiDataCapture.Logic.Definitions;

    /// <summary>
    /// Implements <see cref="IPersonManagerFactory" />.
    /// </summary>
    public class PersonManagerFactory : IPersonManagerFactory
    {
        private readonly IPersonManagerSettingsProvider personManagerSettingsProvider;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="PersonManagerFactory" /> class.
        /// </summary>
        /// <param name="personManagerSettingsProvider">
        /// An instance of type <see cref="IPersonManagerSettingsProvider" />.
        /// </param>
        public PersonManagerFactory(
            IPersonManagerSettingsProvider personManagerSettingsProvider)
        {
            this.personManagerSettingsProvider = personManagerSettingsProvider;
        }

        /// <inheritdoc />
        public IPersonManager Create(ILoggerProvider loggerProvider)
        {
            PersonManager toReturn = new PersonManager(
                loggerProvider,
                this.personManagerSettingsProvider);

            return toReturn;
        }
    }
}
