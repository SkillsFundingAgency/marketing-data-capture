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
            bool personIsValid = this.ModelIsValid(person);

            if (personIsValid)
            {
                // As this is a create request, we can only create the record
                // if the email address does not exist currently.
                // Check for the ContactDetail instance first off.
                string emailAddress = person.ContactDetail.EmailAddress;

                ReadContactDetailResult readContactDetailResult =
                    this.dataCaptureDatabaseAdapter.ReadContactDetail(
                        emailAddress);

                if (readContactDetailResult == null)
                {
                    this.InsertPersonIntoDatabase(person);
                }
                else
                {
                    // TODO: Throw a custom exception to translate above.
                    //       Or return a model?
                    //       i.e. CreateResult?
                }
            }
        }

        private void InsertPersonIntoDatabase(Person person)
        {
            // 1) Person
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseContract)}.{nameof(IDataCaptureDatabaseContract.CreatePerson)}...");

            CreatePersonResult createPersonResult =
                this.dataCaptureDatabaseAdapter.CreatePerson(
                    DateTime.UtcNow,
                    person.Enrolled,
                    person.FirstName,
                    person.LastName);

            this.loggerProvider.Info(
                $"Created: {createPersonResult}.");

            // Now we have an id for Person, insert into the satellite tables.
            // 2) Consent
            // TODO...
        }

        private bool ModelIsValid(Person person)
        {
            bool toReturn = true;

            this.loggerProvider.Info($"Checking the validity of {person}...");

            // TODO: Compelte me.
            return toReturn;
        }
    }
}