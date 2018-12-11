namespace MarketingDataCapture.FunctionApp.Functions
{
    using System.Net;
    using MarketingDataCapture.FunctionApp.Infrastructure;
    using MarketingDataCapture.Logic;
    using MarketingDataCapture.Logic.Definitions;
    using MarketingDataCapture.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;

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
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)]
            HttpRequest httpRequest,
            TraceWriter traceWriter)
        {
            IActionResult toReturn = null;

            LoggerProvider loggerProvider = new LoggerProvider(traceWriter);

            toReturn = FunctionLogicHarness.Execute<Models.UpdatePersonBody.Person>(
                httpRequest,
                new LoggerProvider(traceWriter),
                PerformPersonValidation,
                PerformUpdatePerson);

            return toReturn;
        }

        private static bool PerformPersonValidation(
            ILoggerProvider loggerProvider,
            Models.UpdatePersonBody.Person person)
        {
            bool passedValidation = true;

            // The top level is validated for free.
            // If the top level passed, now validate the sub-properties.
            if (passedValidation && (person.Consent != null))
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    loggerProvider,
                    person.Consent);
            }

            // ContactDetail is required, whilst the others are optional.
            // We need ContactDetail because it contains the key we use to
            // look up a person.
            if (passedValidation)
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    loggerProvider,
                    person.ContactDetail);
            }

            if (passedValidation && (person.Cookie != null))
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    loggerProvider,
                    person.Cookie);
            }

            if (passedValidation && (person.Cookie != null))
            {
                passedValidation = FunctionLogicHarness.ValidateModel(
                    loggerProvider,
                    person.Route);
            }

            return passedValidation;
        }

        private static HttpStatusCode PerformUpdatePerson(
            ILoggerProvider loggerProvider,
            IPersonManager personManager,
            Models.UpdatePersonBody.Person updatePerson)
        {
            HttpStatusCode toReturn;

            loggerProvider.Debug(
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

                        // This will only be updated, depending on
                        // UpdatePersonBody.ContactDetail.EmailVerificationCompletion.
                        EmailVerificationCompletion = updatePerson.ContactDetail.EmailVerificationCompletion,
                    },
                    FirstName = updatePerson.FirstName,
                    LastName = updatePerson.LastName,
                };

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
                    person.Cookie = new MarketingDataCapture.Models.Cookie()
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

                personManager.Update(
                    person,
                    updatePerson.ContactDetail.UpdateEmailVerificationCompletion);

                loggerProvider.Info(
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