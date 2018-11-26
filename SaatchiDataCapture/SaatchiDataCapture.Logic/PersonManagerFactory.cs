namespace SaatchiDataCapture.Logic
{
    using SaatchiDataCapture.Logic.Definitions;

    public class PersonManagerFactory : IPersonManagerFactory
    {
        private readonly IPersonManagerSettingsProvider personManagerSettingsProvider;

        public PersonManagerFactory(
            IPersonManagerSettingsProvider personManagerSettingsProvider)
        {
            this.personManagerSettingsProvider = personManagerSettingsProvider;
        }

        public IPersonManager Create(ILoggerProvider loggerProvider)
        {
            PersonManager toReturn = new PersonManager(
                loggerProvider,
                this.personManagerSettingsProvider);

            return toReturn;
        }
    }
}
