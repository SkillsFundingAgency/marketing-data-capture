﻿namespace MarketingDataCapture.Logic
{
    using System;
    using MarketingDataCapture.Data.Definitions;
    using MarketingDataCapture.Data.Models;
    using MarketingDataCapture.Logic.Definitions;
    using MarketingDataCapture.Models;

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
            string emailAddress = person.ContactDetail.EmailAddress;

            // As this is a create request, we can only create the record
            // if the email address does not exist currently.
            // Check for the Person instance first off.
            ReadPersonResult readPersonResult =
                this.GetContactDetailByEmail(emailAddress);

            if (readPersonResult == null)
            {
                this.loggerProvider.Info(
                    $"\"{emailAddress}\" does not exist. Going ahead " +
                    $"with insert of {person}.");

                this.InsertRecordsIntoDatabase(person);

                this.loggerProvider.Info(
                    $"{person} was inserted into the database " +
                    $"successfully.");
            }
            else
            {
                throw new PersonRecordExistsAlreadyException(emailAddress);
            }
        }

        /// <inheritdoc />
        public void Update(Person person, bool updateEmailAddressVerification)
        {
            // As this is an update request, we can only update the record
            // if the email address exists currently.
            // Check for the Person instance first off.
            string emailAddress = person.ContactDetail.EmailAddress;

            ReadPersonResult readPersonResult =
                this.GetContactDetailByEmail(emailAddress);

            if (readPersonResult != null)
            {
                this.loggerProvider.Info(
                    $"\"{emailAddress}\" exists. Going ahead with update " +
                    $"of {person}.");

                this.UpdatePersonInDatabase(
                    person,
                    readPersonResult,
                    updateEmailAddressVerification);

                this.loggerProvider.Info(
                    $"{person} was updated successfully.");
            }
            else
            {
                throw new PersonRecordDoesNotExistException(emailAddress);
            }
        }

        private ReadPersonResult GetContactDetailByEmail(
            string emailAddress)
        {
            ReadPersonResult toReturn = null;

            this.loggerProvider.Debug(
                $"Checking for existing {nameof(ContactDetail)} record " +
                $"using email address \"{emailAddress}\"...");

            toReturn = this.dataCaptureDatabaseAdapter.ReadPerson(
                emailAddress);

            return toReturn;
        }

        private void InsertRecordsIntoDatabase(Person person)
        {
            // 1) Person
            this.loggerProvider.Debug(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreatePerson)}...");

            CreatePersonResult createPersonResult =
                this.dataCaptureDatabaseAdapter.CreatePerson(
                    DateTime.UtcNow,
                    person.Enrolled.Value,
                    person.FirstName,
                    person.LastName);

            this.loggerProvider.Info(
                $"Created: {createPersonResult}.");

            long personId = createPersonResult.Id;

            // Now we have an id for Person, insert into the satellite tables.
            // 2) ContactDetail
            this.loggerProvider.Debug(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateContactDetail)}...");

            CreateContactDetailResult createContactDetailResult =
                this.dataCaptureDatabaseAdapter.CreateContactDetail(
                    personId,
                    DateTime.UtcNow,
                    person.ContactDetail.Captured.Value,
                    person.ContactDetail.EmailAddress,
                    person.ContactDetail.EmailVerificationCompletion);

            this.loggerProvider.Info($"Created: {createContactDetailResult}.");

            this.InsertIntoOneToManyTables(personId, person);
        }

        private void InsertIntoOneToManyTables(
            long personId,
            Person person)
        {
            // Why the null checks?
            // When updating, the sattelite classes are optional (bar
            // ContactDetail).
            // Since validation is the responsibility of a level above,
            // we don't have to worry about this.
            if (person.Consent != null)
            {
                // 3) Consent
                this.loggerProvider.Debug(
                    $"Invoking " +
                    $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateConsent)}...");

                CreateConsentResult createConsentResult =
                    this.dataCaptureDatabaseAdapter.CreateConsent(
                        personId,
                        DateTime.UtcNow,
                        person.Consent.GdprConsentDeclared,
                        person.Consent.GdprConsentGiven);

                this.loggerProvider.Info($"Created: {createConsentResult}.");
            }

            if (person.Cookie != null)
            {
                // 4) Cookie
                this.loggerProvider.Debug(
                    $"Invoking " +
                    $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateCookie)}...");

                CreateCookieResult createCookieResult =
                    this.dataCaptureDatabaseAdapter.CreateCookie(
                        personId,
                        DateTime.UtcNow,
                        person.Cookie.Captured,
                        person.Cookie.CookieIdentifier);

                this.loggerProvider.Info($"Created: {createCookieResult}.");
            }

            if (person.Route != null)
            {
                // 5) Route
                this.loggerProvider.Debug(
                    $"Invoking " +
                    $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.CreateRoute)}...");

                CreateRouteResult createRouteResult =
                    this.dataCaptureDatabaseAdapter.CreateRoute(
                        personId,
                        DateTime.UtcNow,
                        person.Route.Captured,
                        person.Route.RouteIdentifier);

                this.loggerProvider.Info($"Created: {createRouteResult}.");
            }
        }

        private void UpdatePersonInDatabase(
            Person person,
            ReadPersonResult readContactDetailResult,
            bool updateEmailAddressVerification)
        {
            // First, insert the one-to-many records, as required.
            long personId = readContactDetailResult.Id;

            this.InsertIntoOneToManyTables(personId, person);

            // Then update the Person and ContactDetail tables.
            this.loggerProvider.Debug(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.UpdatePerson)} " +
                $"with id {personId}...");

            this.dataCaptureDatabaseAdapter.UpdatePerson(
                personId,
                person.FirstName,
                person.LastName);

            this.loggerProvider.Info(
                $"Updated {nameof(Person)} id {personId}.");

            if (updateEmailAddressVerification)
            {
                long contactDetailId = readContactDetailResult.ContactDetail_Id;

                this.loggerProvider.Debug(
                    $"Invoking " +
                    $"{nameof(IDataCaptureDatabaseAdapter)}.{nameof(IDataCaptureDatabaseAdapter.UpdateContactDetail)} " +
                    $"with id {contactDetailId}...");

                this.dataCaptureDatabaseAdapter.UpdateContactDetail(
                    contactDetailId,
                    person.ContactDetail.EmailVerificationCompletion);

                this.loggerProvider.Info(
                    $"Updated {nameof(ContactDetail)} id {contactDetailId}.");
            }
        }
    }
}