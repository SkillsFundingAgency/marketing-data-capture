namespace SaatchiDataCapture.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper;
    using SaatchiDataCapture.Data.Definitions;
    using SaatchiDataCapture.Data.Models;

    /// <summary>
    /// Implements <see cref="IDataCaptureDatabaseAdapter" />.
    /// </summary>
    public class DataCaptureDatabaseAdapter : IDataCaptureDatabaseAdapter
    {
        private readonly string dataCaptureDatabaseConnectionString;

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
            this.dataCaptureDatabaseConnectionString =
                dataCaptureDatabaseAdapterSettingsProvider.DataCaptureDatabaseConnectionString;
        }

        /// <inheritdoc />
        public CreateConsentResult CreateConsent(
            long personId,
            DateTime created,
            DateTime gdprConsentDeclared,
            bool? gdprConsentGiven)
        {
            CreateConsentResult toReturn = null;

            object sprocParameters =
                new
                {
                    Person_Id = personId,
                    Created = created,
                    GDPRConsentDeclared = gdprConsentDeclared,
                    GDPRConsentGiven = gdprConsentGiven,
                };

            toReturn =
                this.ExecuteStoredProcedureSingularResult<CreateConsentResult>(
                    "Create_Consent",
                    sprocParameters);

            return toReturn;
        }

        /// <inheritdoc />
        public CreateContactDetailResult CreateContactDetail(
            long personId,
            DateTime created,
            DateTime captured,
            string emailAddress,
            DateTime? emailVerificationCompletion)
        {
            CreateContactDetailResult toReturn = null;

            object sprocParameters =
                new
                {
                    Person_Id = personId,
                    Created = created,
                    Captured = captured,
                    EmailAddress = emailAddress,
                    EmailVerificationCompletion = emailVerificationCompletion,
                };

            toReturn =
                this.ExecuteStoredProcedureSingularResult<CreateContactDetailResult>(
                    "Create_ContactDetail",
                    sprocParameters);

            return toReturn;
        }

        /// <inheritdoc />
        public CreateCookieResult CreateCookie(
            long personId,
            DateTime created,
            DateTime captured,
            string cookieIdentifier)
        {
            CreateCookieResult toReturn = null;

            object sprocParameters =
                new
                {
                    Person_Id = personId,
                    Created = created,
                    Captured = captured,
                    CookieIdentifier = cookieIdentifier,
                };

            toReturn =
                this.ExecuteStoredProcedureSingularResult<CreateCookieResult>(
                    "Create_Cookie",
                    sprocParameters);

            return toReturn;
        }

        /// <inheritdoc />
        public CreatePersonResult CreatePerson(
            DateTime created,
            DateTime enrolled,
            string firstName,
            string lastName)
        {
            CreatePersonResult toReturn = null;

            object sprocParameters =
                new
                {
                    Created = created,
                    Enrolled = enrolled,
                    FirstName = firstName,
                    LastName = lastName,
                };

            toReturn =
                this.ExecuteStoredProcedureSingularResult<CreatePersonResult>(
                    "Create_Person",
                    sprocParameters);

            return toReturn;
        }

        /// <inheritdoc />
        public CreateRouteResult CreateRoute(
            long personId,
            DateTime created,
            DateTime captured,
            string routeIdentifier)
        {
            CreateRouteResult toReturn = null;

            object sprocParameters =
                new
                {
                    Person_Id = personId,
                    Created = created,
                    Captured = captured,
                    RouteIdentifier = routeIdentifier,
                };

            toReturn =
                this.ExecuteStoredProcedureSingularResult<CreateRouteResult>(
                    "Create_Route",
                    sprocParameters);

            return toReturn;
        }

        /// <inheritdoc />
        public ReadPersonResult ReadPerson(
            string emailAddress)
        {
            ReadPersonResult toReturn = null;

            object sprocParameters =
                new
                {
                    EmailAddress = emailAddress,
                };

            toReturn =
                this.ExecuteStoredProcedureSingularResult<ReadPersonResult>(
                    "Read_Person",
                    sprocParameters);

            return toReturn;
        }

        private TResultType ExecuteStoredProcedureSingularResult<TResultType>(
            string storedProcedureName,
            object parameters)
            where TResultType : ModelsBase
        {
            TResultType toReturn = null;

            using (IDbConnection sqlConnection = new SqlConnection(this.dataCaptureDatabaseConnectionString))
            {
                sqlConnection.Open();

                IEnumerable<TResultType> results =
                    sqlConnection.Query<TResultType>(
                        storedProcedureName,
                        parameters,
                        commandType: CommandType.StoredProcedure);

                toReturn = results.SingleOrDefault();
            }

            return toReturn;
        }
    }
}