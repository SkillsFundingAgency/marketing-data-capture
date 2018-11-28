namespace SaatchiDataCapture.Data.Definitions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using SaatchiDataCapture.Data.Models;

    /// <summary>
    /// Describes the operations of the data capture database adapter.
    /// </summary>
    public interface IDataCaptureDatabaseAdapter
    {
        /// <summary>
        /// Executes the <c>Create_Consent</c> stored procedure.
        /// </summary>
        /// <param name="personId">
        /// Provides the <c>@Person_Id</c> parameter.
        /// </param>
        /// <param name="created">
        /// Provides the <c>@Created</c> parameter.
        /// </param>
        /// <param name="gdprConsentDeclared">
        /// Provides the <c>@GDPRConsentDeclared</c> parameter.
        /// </param>
        /// <param name="gdprConsentGiven">
        /// Provides the <c>@GDPRConsentGiven</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of <see cref="CreateConsentResult" />.
        /// </returns>
        CreateConsentResult CreateConsent(
            long personId,
            DateTime created,
            DateTime gdprConsentDeclared,
            bool? gdprConsentGiven);

        /// <summary>
        /// Executes the <c>Create_ContactDetail</c> stored procedure.
        /// </summary>
        /// <param name="personId">
        /// Provides the <c>@Person_Id</c> parameter.
        /// </param>
        /// <param name="created">
        /// Provides the <c>@Created</c> parameter.
        /// </param>
        /// <param name="captured">
        /// Provides the <c>@Captured</c> parameter.
        /// </param>
        /// <param name="emailAddress">
        /// Provides the <c>@EmailAddress</c> parameter.
        /// </param>
        /// <param name="emailVerificationCompletion">
        /// Provides the <c>@EmailVerificationCompletion</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of <see cref="CreateContactDetailResult" />.
        /// </returns>
        CreateContactDetailResult CreateContactDetail(
            long personId,
            DateTime created,
            DateTime captured,
            string emailAddress,
            DateTime? emailVerificationCompletion);

        /// <summary>
        /// Executes the <c>Create_Cookie</c> stored procedure.
        /// </summary>
        /// <param name="personId">
        /// Provides the <c>@Person_Id</c> parameter.
        /// </param>
        /// <param name="created">
        /// Provides the <c>@Created</c> parameter.
        /// </param>
        /// <param name="captured">
        /// Provides the <c>@Captured</c> parameter.
        /// </param>
        /// <param name="cookieIdentifier">
        /// Provides the <c>@CookieIdentifier</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of <see cref="CreateCookieResult" />.
        /// </returns>
        CreateCookieResult CreateCookie(
            long personId,
            DateTime created,
            DateTime captured,
            string cookieIdentifier);

        /// <summary>
        /// Executes the <c>Create_Person</c> stored procedure.
        /// </summary>
        /// <param name="created">
        /// Provides the <c>@Created</c> parameter.
        /// </param>
        /// <param name="enrolled">
        /// Provides the <c>@Enrolled</c> parameter.
        /// </param>
        /// <param name="firstName">
        /// Provides the <c>@FirstName</c> parameter.
        /// </param>
        /// <param name="lastName">
        /// Provides the <c>@LastName</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of <see cref="CreatePersonResult" />.
        /// </returns>
        CreatePersonResult CreatePerson(
            DateTime created,
            DateTime enrolled,
            string firstName,
            string lastName);

        /// <summary>
        /// Executes the <c>Create_Route</c> stored procedure.
        /// </summary>
        /// <param name="personId">
        /// Provides the <c>@Person_Id</c> parameter.
        /// </param>
        /// <param name="created">
        /// Provides the <c>@Created</c> parameter.
        /// </param>
        /// <param name="captured">
        /// Provides the <c>@Captured</c> parameter.
        /// </param>
        /// <param name="routeIdentifier">
        /// Provides the <c>@RouteIdentifier</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of <see cref="CreateRouteResult" />.
        /// </returns>
        CreateRouteResult CreateRoute(
            long personId,
            DateTime created,
            DateTime captured,
            string routeIdentifier);

        /// <summary>
        /// Executes the <c>Read_ContactDetail</c> stored procedure.
        /// </summary>
        /// <param name="emailAddress">
        /// Provides the <c>@EmailAddress</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="ReadContactDetailResult" />.
        /// </returns>
        ReadContactDetailResult ReadContactDetail(
            string emailAddress);
    }
}