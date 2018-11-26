namespace SaatchiDataCapture.Logic
{
    using System;
    using Meridian.InterSproc;
    using SaatchiDataCapture.Data.Definitions;
    using SaatchiDataCapture.Data.Models;
    using SaatchiDataCapture.Logic.Definitions;
    using SaatchiDataCapture.Models;

    /// <summary>
    /// Implements <see cref="IPersonManager" />.
    /// </summary>
    public class PersonManager : IPersonManager
    {
        private readonly IDataCaptureDatabaseContract dataCaptureDatabaseContract;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="PersonManager" />
        /// class.
        /// </summary>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        /// <param name="personManagerSettingsProvider">
        /// An instance of type <see cref="IPersonManagerSettingsProvider" />.
        /// </param>
        public PersonManager(
            ILoggerProvider loggerProvider,
            IPersonManagerSettingsProvider personManagerSettingsProvider)
        {
            this.loggerProvider = loggerProvider;

            string dataCaptureDatabaseConnectionString =
                personManagerSettingsProvider.DataCaptureDatabaseConnectionString;

            this.dataCaptureDatabaseContract =
                SprocStubFactory.Create<IDataCaptureDatabaseContract>(
                    dataCaptureDatabaseConnectionString);
        }

        /// <inheritdoc />
        public void Create(Person person)
        {
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseContract)}.{nameof(IDataCaptureDatabaseContract.CreatePerson)}...");

            CreatedEntityReference createdEntityReference =
                this.dataCaptureDatabaseContract.CreatePerson(
                    DateTime.UtcNow,
                    person.Enrolled,
                    person.FirstName,
                    person.LastName);

            this.loggerProvider.Info(
                $"Created: {createdEntityReference}.");
        }
    }
}