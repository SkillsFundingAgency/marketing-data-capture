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
                    Consent = new Consent()
                    {
                        GdprConsentDeclared = updatePerson.Consent.GdprConsentDeclared,
                        GdprConsentGiven = updatePerson.Consent.GdprConsentGiven,
                    },
                    ContactDetail = new ContactDetail()
                    {
                        EmailAddress = updatePerson.ContactDetail.EmailAddress,
                        EmailVerificationCompletion = updatePerson.ContactDetail.EmailVerificationCompletion,
                    },
                    Cookie = new SaatchiDataCapture.Models.Cookie()
                    {
                        Captured = updatePerson.Cookie.Captured,
                        CookieIdentifier = updatePerson.Cookie.CookieIdentifier,
                    },
                    Route = new Route()
                    {
                        Captured = updatePerson.Route.Captured,
                        RouteIdentifier = updatePerson.Route.RouteIdentifier,
                    },
                    FirstName = updatePerson.FirstName,
                    LastName = updatePerson.LastName,
                };

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