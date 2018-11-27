namespace SaatchiDataCapture.Logic
{
    using System;
    using SaatchiDataCapture.Data.Definitions;
    using SaatchiDataCapture.Data.Models;
    using SaatchiDataCapture.Logic.Definitions;
    using SaatchiDataCapture.Models;

    /// <summary>
    /// Implements <see cref="IPersonManager" />.
    /// </summary>
    public class PersonManager : IPersonManager
    {
        private readonly IDataCaptureDatabaseAdapter dataCaptureDatabaseAdapter;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="PersonManager" />
        /// class.
        /// </summary>
        /// <param name="dataCaptureDatabaseAdapter">
        /// An instance of type <see cref="IDataCaptureDatabaseAdapter" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public PersonManager(
            IDataCaptureDatabaseAdapter dataCaptureDatabaseAdapter,
            ILoggerProvider loggerProvider)
        {
            this.dataCaptureDatabaseAdapter = dataCaptureDatabaseAdapter;
            this.loggerProvider = loggerProvider;
        }

        /// <inheritdoc />
        public void Create(Person person)
        {
            // TODO: Validate the Person instance.
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseContract)}.{nameof(IDataCaptureDatabaseContract.CreatePerson)}...");

            CreatedEntityReference createdEntityReference =
                this.dataCaptureDatabaseAdapter.CreatePerson(
                    DateTime.UtcNow,
                    person.Enrolled,
                    person.FirstName,
                    person.LastName);

            this.loggerProvider.Info(
                $"Created: {createdEntityReference}.");
        }
    }
}