namespace SaatchiDataCapture.FunctionApp.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;
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
        /// <typeparam name="TRequestBody">
        /// A type deriving from <see cref="Models.ModelsBase" />.
        /// </typeparam>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="traceWriter">
        /// An instance of <see cref="TraceWriter" />.
        /// </param>
        /// <param name="serviceInvocationLogic">
        /// The service invocation logic, accepting an instance of type
        /// <see cref="IPersonManager" /> and the deserialised
        /// <typeparamref name="TRequestBody" /> instance.
        /// Returns a <see cref="HttpStatusCode" /> depending on expected
        /// outcomes.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IActionResult" />.
        /// </returns>
        public static IActionResult Execute<TRequestBody>(
            HttpRequest httpRequest,
            TraceWriter traceWriter,
            Func<IPersonManager, TRequestBody, HttpStatusCode> serviceInvocationLogic)
            where TRequestBody : Models.ModelsBase
        {
            IActionResult toReturn = null;

            HttpStatusCode httpStatusCode;
            try
            {
                httpStatusCode = InvokePersonManager<TRequestBody>(
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

        private static HttpStatusCode InvokePersonManager<TRequestBody>(
            HttpRequest httpRequest,
            TraceWriter traceWriter,
            Func<IPersonManager, TRequestBody, HttpStatusCode> serviceInvocationLogic)
            where TRequestBody : Models.ModelsBase
        {
            HttpStatusCode toReturn;

            // Get our entry point in the logic layer and...
            IPersonManager personManager =
                GetIPersonManagerInstance(traceWriter);

            TRequestBody requestBody = ParseRequestBody<TRequestBody>(
                httpRequest,
                traceWriter);

            if (requestBody != null && ValidateModel(traceWriter, requestBody))
            {
                toReturn = serviceInvocationLogic(personManager, requestBody);
            }
            else
            {
                // If person == null, then the JSON couldn't be parsed
                // correctly. Return a BadRequest.
                toReturn = HttpStatusCode.BadRequest;
            }

            return toReturn;
        }

        private static bool ValidateModel<TRequestBody>(
            TraceWriter traceWriter,
            TRequestBody requestBody)
            where TRequestBody : Models.ModelsBase
        {
            bool toReturn = false;

            ValidationContext validationContext = new ValidationContext(
                requestBody,
                null,
                null);

            List<ValidationResult> validationResults =
                new List<ValidationResult>();

            traceWriter.Info($"Performing validation of {requestBody}...");

            // TODO: We need to validate the models within this as well.
            toReturn = Validator.TryValidateObject(
                requestBody,
                validationContext,
                validationResults);

            if (toReturn)
            {
                traceWriter.Info($"{requestBody} passed validation!");
            }
            else
            {
                string[] validationList = validationResults
                    .Select(ValidationResultToString)
                    .ToArray();

                string validationListConcat =
                    string.Join(", ", validationList);

                string validationFailuresDesc =
                    $"{validationResults.Count} validation error(s) were " +
                    $"highlighted. These are: {validationListConcat}.";

                traceWriter.Warning(
                    $"{requestBody} failed validation. " +
                    $"{validationFailuresDesc}.");
            }

            return toReturn;
        }

        private static string ValidationResultToString(
            ValidationResult validationResult)
        {
            string toReturn = null;

            string[] affectedFields = validationResult.MemberNames
                .ToArray();

            string affectedFieldsDesc = string.Join(", ", affectedFields);

            toReturn =
                $"{validationResult.ErrorMessage} ({affectedFieldsDesc})";

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

        private static TRequestBody ParseRequestBody<TRequestBody>(
            HttpRequest httpRequest,
            TraceWriter traceWriter)
            where TRequestBody : Models.ModelsBase
        {
            TRequestBody toReturn = null;

            Type requestBodyType = typeof(TRequestBody);

            traceWriter.Info("Reading the request body...");

            string requestBody = null;
            using (StreamReader streamReader = new StreamReader(httpRequest.Body))
            {
                requestBody = streamReader.ReadToEnd();
            }

            traceWriter.Info(
                $"Request body: \"{requestBody}\". Parsing body into " +
                $"{requestBodyType.Name} instance...");

            try
            {
                toReturn = JsonConvert.DeserializeObject<TRequestBody>(
                    requestBody);

                traceWriter.Info($"Parsed: {toReturn}.");
            }
            catch (JsonReaderException)
            {
                // Do nothing - this method will return null if the JSON is
                // badly formed.
            }

            return toReturn;
        }
    }
}