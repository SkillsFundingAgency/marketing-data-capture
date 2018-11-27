namespace SaatchiDataCapture.Data.Definitions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Meridian.InterSproc;
    using SaatchiDataCapture.Data.Models;

    /// <summary>
    /// Describes the stored procedures exposed by the data capture database.
    /// </summary>
    public interface IDataCaptureDatabaseContract
    {
        /// <summary>
        /// Executes the <c>Create_Consent</c> stored procedure.
        /// </summary>
        /// <param name="person_Id">
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
        [InterSprocContractMethod(Name = "Create_Consent")]
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1707",
            Justification = "Stored procedure parameters need to match contract arguments.")]
        CreateConsentResult CreateConsent(
            long person_Id,
            DateTime created,
            DateTime gdprConsentDeclared,
            bool? gdprConsentGiven);

        /// <summary>
        /// Executes the <c>Create_ContactDetail</c> stored procedure.
        /// </summary>
        /// <param name="person_Id">
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
        [InterSprocContractMethod(Name = "Create_ContactDetail")]
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1707",
            Justification = "Stored procedure parameters need to match contract arguments.")]
        CreateContactDetailResult CreateContactDetail(
            long person_Id,
            DateTime created,
            DateTime captured,
            string emailAddress,
            DateTime? emailVerificationCompletion);

        /// <summary>
        /// Executes the <c>Create_Cookie</c> stored procedure.
        /// </summary>
        /// <param name="person_Id">
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
        [InterSprocContractMethod(Name = "Create_Cookie")]
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1707",
            Justification = "Stored procedure parameters need to match contract arguments.")]
        CreateCookieResult CreateCookie(
            long person_Id,
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
        [InterSprocContractMethod(Name = "Create_Person")]
        CreatePersonResult CreatePerson(
            DateTime created,
            DateTime enrolled,
            string firstName,
            string lastName);

        /// <summary>
        /// Executes the <c>Create_Route</c> stored procedure.
        /// </summary>
        /// <param name="person_Id">
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
        [InterSprocContractMethod(Name = "Create_Route")]
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1707",
            Justification = "Stored procedure parameters need to match contract arguments.")]
        CreateRouteResult CreateRoute(
            long person_Id,
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
        [InterSprocContractMethod(Name = "Read_ContactDetail")]
        ReadContactDetailResult ReadContactDetail(string emailAddress);
    }
}
