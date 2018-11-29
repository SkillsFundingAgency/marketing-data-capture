namespace SaatchiDataCapture.FunctionApp.Infrastructure
{
    using System;
    using System.IO;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;
    using SaatchiDataCapture.FunctionApp.Infrastructure;
    using SaatchiDataCapture.Logic.Definitions;
    using SaatchiDataCapture.Models;
    using StructureMap;

    /// <summary>
    /// Provides common error handling and provisions for dependency injection
    /// for all functions.
    /// </summary>
    public static class FunctionLogicHarness
    {
        /// <summary>
        /// Performs 3 common actions for all function entry points:
        /// 1) Initialises and creates an instance of
        ///    <see cref="IPersonManager" /> via the container, providing an
        ///    entry point to the logic library.
        /// 2) Deserialises the request body into a <see cref="Person" />
        ///    instance.
        /// 3) Wraps <paramref name="serviceInvocationLogic" /> in a try catch,
        ///    providing global error handling across all functions. In every
        ///    instance, if an unhandled/unexpected exception is thrown, then
        ///    a 500 will be returned, and the exception logged via the
        ///    provided <paramref name="traceWriter" />.
        /// 4) Provides the <see cref="IActionResult" />.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="traceWriter">
        /// An instance of <see cref="TraceWriter" />.
        /// </param>
        /// <param name="serviceInvocationLogic">
        /// The service invocation logic, accepting an instance of type
        /// <see cref="IPersonManager" /> and the deserialised
        /// <see cref="Person" /> instance.
        /// Returns a <see cref="HttpStatusCode" /> depending on expected
        /// outcomes.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IActionResult" />.
        /// </returns>
        public static IActionResult Execute(
            HttpRequest httpRequest,
            TraceWriter traceWriter,
            Func<IPersonManager, Person, HttpStatusCode> serviceInvocationLogic)
        {
            IActionResult toReturn = null;

            HttpStatusCode httpStatusCode;
            try
            {
                httpStatusCode = InvokePersonManager(
                    httpRequest,
                    traceWriter,
                    serviceInvocationLogic);
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

        private static HttpStatusCode InvokePersonManager(
            HttpRequest httpRequest,
            TraceWriter traceWriter,
            Func<IPersonManager, Person, HttpStatusCode> serviceInvocationLogic)
        {
            HttpStatusCode toReturn;

            // Get our entry point in the logic layer and...
            IPersonManager personManager =
                GetIPersonManagerInstance(traceWriter);

            Person person = ParseRequestBody(httpRequest, traceWriter);

            toReturn = serviceInvocationLogic(personManager, person);

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
    }
}