﻿namespace SaatchiDataCapture.FunctionApp
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using SaatchiDataCapture.FunctionApp.Infrastructure;
    using SaatchiDataCapture.Logic;
    using SaatchiDataCapture.Logic.Definitions;
    using SaatchiDataCapture.Models;

    /// <summary>
    /// Entry class for the <c>update-person</c> function.
    /// </summary>
    public static class UpdatePerson
    {
        /// <summary>
        /// Entry method for the <c>update-person</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpContext" />.
        /// </param>
        /// <param name="traceWriter">
        /// An instance of <see cref="TraceWriter" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName("update-person")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)]
            HttpRequest httpRequest,
            TraceWriter traceWriter)
        {
            IActionResult toReturn = FunctionLogicHarness.Execute<Models.UpdatePersonBody.Person>(
                httpRequest,
                traceWriter,
                (person) =>
                {
                    bool passedValidation = PerformPersonValidation(
                        traceWriter,
                        person);

                    return passedValidation;
                },
                (personManager, person) =>
                {
                    HttpStatusCode httpStatusCode = PerformUpdatePerson(
                        traceWriter,
                        personManager,
                        person);

                    return httpStatusCode;
                });

            return toReturn;
        }

        private static bool PerformPersonValidation(
            TraceWriter traceWriter,
            Models.UpdatePersonBody.Person person)
        {
            bool passedValidation = true;

            // The top level is validated for free.
            // If the top level passed, now validate the sub-properties.
            if (passedValidation && (person.Consent != null))
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.Consent);
            }

            // ContactDetail is required, whilst the others are optional.
            // We need ContactDetail because it contains the key we use to
            // look up a person.
            if (passedValidation)
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.ContactDetail);
            }

            if (passedValidation && (person.Cookie != null))
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.Cookie);
            }

            if (passedValidation && (person.Cookie != null))
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.Route);
            }

            return passedValidation;
        }

        private static HttpStatusCode PerformUpdatePerson(
            TraceWriter traceWriter,
            IPersonManager personManager,
            Models.UpdatePersonBody.Person updatePerson)
        {
            HttpStatusCode toReturn;

            traceWriter.Info(
                $"Invoking " +
                $"{nameof(IPersonManager)}.{nameof(IPersonManager.Update)}...");

            try
            {
                // Map to the global person class.
                Person person = new Person()
                {
                    // ContactDetail is not optional...
                    ContactDetail = new ContactDetail()
                    {
                        // And neither is email address, as this is how we
                        // look up the person.
                        EmailAddress = updatePerson.ContactDetail.EmailAddress,
                    },
                    FirstName = updatePerson.FirstName,
                    LastName = updatePerson.LastName,
                };

                if (updatePerson.ContactDetail.UpdateEmailVerificationCompletion)
                {
                    // Note: Why the above?
                    // EmailVerificationCompletion is valid to be null
                    // (indicating it's still not verified) or to have a
                    // DateTime describing when it was verified.
                    // For more detail, see the docs on
                    // UpdatePersonBody.ContactDetail.EmailVerificationCompletion.
                    person.ContactDetail.EmailVerificationCompletion =
                        updatePerson.ContactDetail.EmailVerificationCompletion;
                }

                // Consent is optional. However, if declared, then...
                if (updatePerson.Consent != null)
                {
                    person.Consent = new Consent()
                    {
                        // It needs the GdprConsentDeclared property specified.
                        GdprConsentDeclared = updatePerson.Consent.GdprConsentDeclared.Value,

                        // Whereas valid states for this property is either
                        // true, false or null.
                        GdprConsentGiven = updatePerson.Consent.GdprConsentGiven,
                    };
                }

                // Cookie is also optional. However if declared, then...
                if (updatePerson.Cookie != null)
                {
                    person.Cookie = new SaatchiDataCapture.Models.Cookie()
                    {
                        // Needs the captured date.
                        Captured = updatePerson.Cookie.Captured.Value,

                        // And the cookie identifier (doesn't make sense for
                        // it to be null).
                        CookieIdentifier = updatePerson.Cookie.CookieIdentifier,
                    };
                }

                // Route is also optional. However if declared...
                if (updatePerson.Route != null)
                {
                    person.Route = new Route()
                    {
                        // Then the captured date needs to be specified...
                        Captured = updatePerson.Route.Captured.Value,

                        // As does the RouteIdentifier.
                        RouteIdentifier = updatePerson.Route.RouteIdentifier,
                    };
                }

                personManager.Update(person);

                traceWriter.Info(
                    $"{nameof(IPersonManager)}.{nameof(IPersonManager.Update)} " +
                    $"invoked with success.");

                // Return Created.
                toReturn = HttpStatusCode.NoContent;
            }
            catch (PersonRecordDoesNotExistException)
            {
                // Return conflicted.
                toReturn = HttpStatusCode.NotFound;
            }

            return toReturn;
        }
    }
}