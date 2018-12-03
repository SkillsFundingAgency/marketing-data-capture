namespace SaatchiDataCapture.FunctionApp
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
    /// Entry class for the <c>create-person</c> function.
    /// </summary>
    public static class CreatePerson
    {
        /// <summary>
        /// Entry method for the <c>create-person</c> function.
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
        [FunctionName("create-person")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)]
            HttpRequest httpRequest,
            TraceWriter traceWriter)
        {
            IActionResult toReturn = FunctionLogicHarness.Execute<Models.CreatePersonBody.Person>(
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
                    HttpStatusCode httpStatusCode = PerformCreatePerson(
                        traceWriter,
                        personManager,
                        person);

                    return httpStatusCode;
                });

            return toReturn;
        }

        private static bool PerformPersonValidation(
            TraceWriter traceWriter,
            Models.CreatePersonBody.Person person)
        {
            bool passedValidation = true;

            // The top level is validated for free.
            // If the top level passed, now validate the sub-properties.
            if (passedValidation)
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.Consent);
            }

            if (passedValidation)
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.ContactDetail);
            }

            if (passedValidation)
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.Cookie);
            }

            if (passedValidation)
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    traceWriter,
                    person.Route);
            }

            return passedValidation;
        }

        private static HttpStatusCode PerformCreatePerson(
            TraceWriter traceWriter,
            IPersonManager personManager,
            Models.CreatePersonBody.Person createPerson)
        {
            HttpStatusCode toReturn;

            traceWriter.Info(
                $"Invoking " +
                $"{nameof(IPersonManager)}.{nameof(IPersonManager.Create)}...");

            try
            {
                // Map to the global person class.
                Person person = new Person()
                {
                    Consent = new Consent()
                    {
                        GdprConsentDeclared = createPerson.Consent.GdprConsentDeclared.Value,
                        GdprConsentGiven = createPerson.Consent.GdprConsentGiven,
                    },
                    ContactDetail = new ContactDetail()
                    {
                        Captured = createPerson.ContactDetail.Captured,
                        EmailAddress = createPerson.ContactDetail.EmailAddress,
                        EmailVerificationCompletion = createPerson.ContactDetail.EmailVerificationCompletion,
                    },
                    Cookie = new SaatchiDataCapture.Models.Cookie()
                    {
                        Captured = createPerson.Cookie.Captured.Value,
                        CookieIdentifier = createPerson.Cookie.CookieIdentifier,
                    },
                    Route = new Route()
                    {
                        Captured = createPerson.Route.Captured.Value,
                        RouteIdentifier = createPerson.Route.RouteIdentifier,
                    },
                    Enrolled = createPerson.Enrolled,
                    FirstName = createPerson.FirstName,
                    LastName = createPerson.LastName,
                };

                personManager.Create(person);

                traceWriter.Info(
                    $"{nameof(IPersonManager)}.{nameof(IPersonManager.Create)} " +
                    $"invoked with success.");

                // Return Created.
                toReturn = HttpStatusCode.Created;
            }
            catch (PersonRecordExistsAlreadyException)
            {
                // Return conflicted.
                toReturn = HttpStatusCode.Conflict;
            }

            return toReturn;
        }
    }
}