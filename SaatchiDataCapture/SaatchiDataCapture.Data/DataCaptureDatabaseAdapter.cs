namespace SaatchiDataCapture.Data
{
    using System;
    using Meridian.InterSproc;
    using SaatchiDataCapture.Data.Definitions;
    using SaatchiDataCapture.Data.Models;

    /// <summary>
    /// Implements <see cref="IDataCaptureDatabaseAdapter" />.
    /// </summary>
    public class DataCaptureDatabaseAdapter : IDataCaptureDatabaseAdapter
    {
        private readonly IDataCaptureDatabaseContract dataCaptureDatabaseContract;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="DataCaptureDatabaseAdapter" /> class.
        /// </summary>
        /// <param name="dataCaptureDatabaseAdapterSettingsProvider">
        /// An instance of type
        /// <see cref="IDataCaptureDatabaseAdapterSettingsProvider" />.
        /// </param>
        public DataCaptureDatabaseAdapter(
            IDataCaptureDatabaseAdapterSettingsProvider dataCaptureDatabaseAdapterSettingsProvider)
        {
            string dataCaptureDatabaseConnectionString =
                dataCaptureDatabaseAdapterSettingsProvider.DataCaptureDatabaseConnectionString;

            this.dataCaptureDatabaseContract =
                SprocStubFactory.Create<IDataCaptureDatabaseContract>(
                    dataCaptureDatabaseConnectionString);
        }

        /// <inheritdoc />
        public CreateConsentResult CreateConsent(
            long person_Id,
            DateTime created,
            DateTime gdprConsentDeclared,
            bool? gdprConsentGiven)
        {
            CreateConsentResult toReturn =
                this.dataCaptureDatabaseContract.CreateConsent(
                    person_Id,
                    created,
                    gdprConsentDeclared,
                    gdprConsentGiven);

            return toReturn;
        }

        /// <inheritdoc />
        public CreateContactDetailResult CreateContactDetail(
            long person_Id,
            DateTime created,
            DateTime captured,
            string emailAddress,
            DateTime? emailVerificationCompletion)
        {
            CreateContactDetailResult toReturn =
                this.dataCaptureDatabaseContract.CreateContactDetail(
                    person_Id,
                    created,
                    captured,
                    emailAddress,
                    emailVerificationCompletion);

            return toReturn;
        }

        /// <inheritdoc />
        public CreateCookieResult CreateCookie(
            long person_Id,
            DateTime created,
            DateTime captured,
            string cookieIdentifier)
        {
            CreateCookieResult toReturn =
                this.dataCaptureDatabaseContract.CreateCookie(
                    person_Id,
                    created,
                    captured,
                    cookieIdentifier);

            return toReturn;
        }

        /// <inheritdoc />
        public CreatePersonResult CreatePerson(
            DateTime created,
            DateTime enrolled,
            string firstName,
            string lastName)
        {
            CreatePersonResult toReturn =
                this.dataCaptureDatabaseContract.CreatePerson(
                    created,
                    enrolled,
                    firstName,
                    lastName);

            return toReturn;
        }

        /// <inheritdoc />
        public CreateRouteResult CreateRoute(
            long person_Id,
            DateTime created,
            DateTime captured,
            string routeIdentifier)
        {
            CreateRouteResult toReturn =
                this.dataCaptureDatabaseContract.CreateRoute(
                    person_Id,
                    created,
                    captured,
                    routeIdentifier);

            return toReturn;
        }

        /// <inheritdoc />
        public ReadContactDetailResult ReadContactDetail(
            string emailAddress)
        {
            ReadContactDetailResult toReturn =
                this.dataCaptureDatabaseContract.ReadContactDetail(
                    emailAddress);

            return toReturn;
        }
    }
}