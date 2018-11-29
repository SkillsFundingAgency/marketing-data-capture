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
            IActionResult toReturn = FunctionLogicHarness.Execute(
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
            Person person)
        {
            HttpStatusCode toReturn;

            traceWriter.Info(
                $"Invoking " +
                $"{nameof(IPersonManager)}.{nameof(IPersonManager.Update)}...");

            try
            {
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