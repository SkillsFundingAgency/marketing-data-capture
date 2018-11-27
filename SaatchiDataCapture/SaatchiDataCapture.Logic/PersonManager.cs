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

                this.loggerProvider.Info(
                    $"Checking for existing ContactDetail record using " +
                    $"email address \"{emailAddress}\"...");

                ReadContactDetailResult readContactDetailResult =
                    this.dataCaptureDatabaseAdapter.ReadContactDetail(
                        emailAddress);

                if (readContactDetailResult == null)
                {
                    this.loggerProvider.Info(
                        $"\"{emailAddress}\" does not exist. Going ahead " +
                        $"with insert of {person}.");

                    this.InsertPersonIntoDatabase(person);

                    this.loggerProvider.Info(
                        $"{person} was inserted into the database " +
                        $"successfully.");
                }
                else
                {
                    throw new PersonRecordExistsAlreadyException(emailAddress);
                }
            }
        }

        private void InsertPersonIntoDatabase(Person person)
        {
            // 1) Person
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreatePerson)}...");

            CreatePersonResult createPersonResult =
                this.dataCaptureDatabaseAdapter.CreatePerson(
                    DateTime.UtcNow,
                    person.Enrolled,
                    person.FirstName,
                    person.LastName);

            this.loggerProvider.Info(
                $"Created: {createPersonResult}.");

            long personId = createPersonResult.Id;

            // Now we have an id for Person, insert into the satellite tables.
            // 2) Consent
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateConsent)}...");

            CreateConsentResult createConsentResult =
                this.dataCaptureDatabaseAdapter.CreateConsent(
                    personId,
                    DateTime.UtcNow,
                    person.Consent.GdprConsentDeclared,
                    person.Consent.GdprConsentGiven);

            this.loggerProvider.Info($"Created: {createConsentResult}.");

            // 3) Cookie
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateCookie)}...");

            CreateCookieResult createCookieResult =
                this.dataCaptureDatabaseAdapter.CreateCookie(
                    personId,
                    DateTime.UtcNow,
                    person.Cookie.Captured,
                    person.Cookie.CookieIdentifier);

            this.loggerProvider.Info($"Created: {createCookieResult}.");

            // 4) Route
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateRoute)}...");

            CreateRouteResult createRouteResult =
                this.dataCaptureDatabaseAdapter.CreateRoute(
                    personId,
                    DateTime.UtcNow,
                    person.Route.Captured,
                    person.Route.RouteIdentifier);

            this.loggerProvider.Info($"Created: {createCookieResult}.");

            // 5) ContactDetail
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateContactDetail)}...");

            CreateContactDetailResult createContactDetailResult =
                this.dataCaptureDatabaseAdapter.CreateContactDetail(
                    personId,
                    DateTime.UtcNow,
                    person.ContactDetail.Captured,
                    person.ContactDetail.EmailAddress,
                    person.ContactDetail.EmailVerificationCompletion);

            this.loggerProvider.Info($"Created: {createContactDetailResult}.");
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