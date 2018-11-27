namespace SaatchiDataCapture.FunctionApp
{
    using System.IO;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;
    using SaatchiDataCapture.Logic;
    using SaatchiDataCapture.Logic.Definitions;
    using SaatchiDataCapture.Models;
    using StructureMap;

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
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)]
            HttpRequest httpRequest,
            TraceWriter traceWriter)
        {
            IActionResult toReturn = null;

            // Get our entry point in the logic layer and...
            IPersonManager personManager =
                GetIPersonManagerInstance(traceWriter);

            Person person = ParseRequestBody(httpRequest, traceWriter);

            traceWriter.Info(
                $"Invoking " +
                $"{nameof(IPersonManager)}.{nameof(IPersonManager.Create)}...");

            HttpStatusCode httpStatusCode;
            try
            {
                personManager.Create(person);

                traceWriter.Info(
                    $"{nameof(IPersonManager)}.{nameof(IPersonManager.Create)} " +
                    $"invoked with success.");

                // Return Created.
                httpStatusCode = HttpStatusCode.Created;
            }
            catch (PersonRecordExistsAlreadyException)
            {
                // Return conflicted.
                httpStatusCode = HttpStatusCode.Conflict;
            }

            traceWriter.Info($"Returning {httpStatusCode}.");

            toReturn = new StatusCodeResult((int)httpStatusCode);

            return toReturn;
        }

        private static Person ParseRequestBody(
            HttpRequest httpRequest,
            TraceWriter traceWriter)
        {
            Person toReturn = null;

            traceWriter.Info("Reading the request body...");

            string requestBody = null;
            using (StreamReader streamReader = new StreamReader(httpRequest.Body))
            {
                requestBody = streamReader.ReadToEnd();
            }

            traceWriter.Info(
                $"Request body: \"{requestBody}\". Parsing body into " +
                $"{nameof(Person)} instance...");

            toReturn = JsonConvert.DeserializeObject<Person>(requestBody);

            traceWriter.Info($"Parsed: {toReturn}.");

            return toReturn;
        }

        private static IPersonManager GetIPersonManagerInstance(
            TraceWriter traceWriter)
        {
            IPersonManager toReturn = null;

            traceWriter.Info(
                $"Pulling back an instance of {nameof(IPersonManager)}...");

            Registry registry = new Registry();
            Container container = new Container(registry);

            IPersonManagerFactory personManagerFactory =
                container.GetInstance<IPersonManagerFactory>();

            LoggerProvider loggerProvider = new LoggerProvider(traceWriter);

            toReturn = personManagerFactory.Create(loggerProvider);

            traceWriter.Info(
                $"Instance of {nameof(IPersonManager)} pulled back.");

            return toReturn;
        }
    }
}