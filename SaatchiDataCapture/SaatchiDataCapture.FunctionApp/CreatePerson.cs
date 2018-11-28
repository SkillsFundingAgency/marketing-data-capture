namespace SaatchiDataCapture.FunctionApp
{
    using System;
    using System.IO;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;
    using SaatchiDataCapture.FunctionApp.Infrastructure;
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

            HttpStatusCode httpStatusCode;
            try
            {
                httpStatusCode = Execute(httpRequest, traceWriter);
            }
            catch (Exception exception)
            {
                traceWriter.Error(
                    "An unhandled exception was thrown.",
                    exception);

                httpStatusCode = HttpStatusCode.InternalServerError;
            }

            toReturn = new StatusCodeResult((int)httpStatusCode);

            traceWriter.Info($"Returning {httpStatusCode}.");

            return toReturn;
        }

        private static HttpStatusCode Execute(
            HttpRequest httpRequest,
            TraceWriter traceWriter)
        {
            HttpStatusCode toReturn;

            // Get our entry point in the logic layer and...
            IPersonManager personManager =
                GetIPersonManagerInstance(traceWriter);

            Person person = ParseRequestBody(httpRequest, traceWriter);

            traceWriter.Info(
                $"Invoking " +
                $"{nameof(IPersonManager)}.{nameof(IPersonManager.Create)}...");

            try
            {
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

        private static string GetAssemblyDirectory(TraceWriter traceWriter)
        {
            string toReturn = null;

            // For some strange reason, when deployed on Azure, the below
            // will be a path to a directory, not the location of the file.
            // Locally, this translates to a file.
            // Thus the craziness below.
            string assemblyLocation = typeof(CreatePerson).Assembly.Location;

            FileAttributes attr = File.GetAttributes(assemblyLocation);

            DirectoryInfo executionDirectory = null;
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                executionDirectory = new DirectoryInfo(assemblyLocation);
            }
            else
            {
                FileInfo fileInfo = new FileInfo(assemblyLocation);

                executionDirectory = fileInfo.Directory;
            }

            toReturn = executionDirectory.FullName;

            traceWriter.Info($"Execution location: {toReturn}.");

            return toReturn;
        }

        private static IPersonManager GetIPersonManagerInstance(
            TraceWriter traceWriter)
        {
            IPersonManager toReturn = null;

            string path = GetAssemblyDirectory(traceWriter);

            traceWriter.Info(
                $"Pulling back an instance of {nameof(IPersonManager)}...");

            Registry registry = new Registry(path);
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