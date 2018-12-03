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
    using Newtonsoft.Json;
    using SaatchiDataCapture.Logic.Definitions;
    using StructureMap;

    /// <summary>
    /// Provides common error handling and provisions for dependency injection
    /// for all functions.
    /// </summary>
    public static class FunctionLogicHarness
    {
        /// <summary>
        /// Performs 6 common actions for all function entry points:
        /// 1) Initialises and creates an instance of
        ///    <see cref="IPersonManager" /> via the container, providing an
        ///    entry point to the logic library.
        /// 2) Deserialises the request body into a
        ///    <typeparamref name="TRequestBody" /> instance.
        /// 3) Validates the root of the deserialised
        ///    <typeparamref name="TRequestBody" /> instance.
        /// 4) Validates the rest of the <typeparamref name="TRequestBody" />
        ///    instance via <paramref name="validateModelLogic" />.
        /// 5) Wraps <paramref name="serviceInvocationLogic" /> in a try catch,
        ///    providing global error handling across all functions. In every
        ///    instance, if an unhandled/unexpected exception is thrown, then
        ///    a 500 will be returned, and the exception logged via the
        ///    provided <paramref name="loggerProvider" />.
        /// 6) Provides the <see cref="IActionResult" />.
        /// </summary>
        /// <typeparam name="TRequestBody">
        /// A type deriving from <see cref="Models.ModelsBase" />.
        /// </typeparam>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        /// <param name="validateModelLogic">
        /// Additional validation logic beyond the immediate route of the
        /// provided instance of <typeparamref name="TRequestBody" />.
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
            ILoggerProvider loggerProvider,
            Func<ILoggerProvider, TRequestBody, bool> validateModelLogic,
            Func<ILoggerProvider, IPersonManager, TRequestBody, HttpStatusCode> serviceInvocationLogic)
            where TRequestBody : Models.ModelsBase
        {
            IActionResult toReturn = null;

            HttpStatusCode httpStatusCode;
            try
            {
                httpStatusCode = InvokePersonManager(
                    httpRequest,
                    loggerProvider,
                    validateModelLogic,
                    serviceInvocationLogic);
            }
            catch (Exception exception)
            {
                loggerProvider.Error(
                    "An unhandled exception was thrown.",
                    exception);

                httpStatusCode = HttpStatusCode.InternalServerError;
            }

            toReturn = new StatusCodeResult((int)httpStatusCode);

            loggerProvider.Info($"Returning {httpStatusCode}.");

            return toReturn;
        }

        /// <summary>
        /// Validates an instance of type <typeparamref name="TRequestBody" />,
        /// according to it's attributes.
        /// </summary>
        /// <typeparam name="TRequestBody">
        /// A type deriving from <see cref="Models.ModelsBase" />.
        /// </typeparam>
        /// <param name="loggerProvider">
        /// An instance of <see cref="ILoggerProvider" />.
        /// </param>
        /// <param name="requestBody">
        /// An instance of type <typeparamref name="TRequestBody" />.
        /// </param>
        /// <returns>
        /// True if the instance passed validation, otherwise false.
        /// </returns>
        public static bool ValidateModel<TRequestBody>(
            ILoggerProvider loggerProvider,
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

            loggerProvider.Debug($"Performing validation of {requestBody}...");

            toReturn = Validator.TryValidateObject(
                requestBody,
                validationContext,
                validationResults,
                true);

            if (toReturn)
            {
                loggerProvider.Info($"{requestBody} passed validation!");
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
                    $"highlighted. These are: {validationListConcat}";

                loggerProvider.Warning(
                    $"{requestBody} failed validation. " +
                    $"{validationFailuresDesc}.");
            }

            return toReturn;
        }

        private static string GetAssemblyDirectory(
            ILoggerProvider loggerProvider)
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

            loggerProvider.Debug($"Execution location: {toReturn}.");

            return toReturn;
        }

        private static IPersonManager GetIPersonManagerInstance(
            ILoggerProvider loggerProvider)
        {
            IPersonManager toReturn = null;

            string path = GetAssemblyDirectory(loggerProvider);

            loggerProvider.Debug(
                $"Pulling back an instance of {nameof(IPersonManager)}...");

            Registry registry = new Registry(path);
            Container container = new Container(registry);

            IPersonManagerFactory personManagerFactory =
                container.GetInstance<IPersonManagerFactory>();

            toReturn = personManagerFactory.Create(loggerProvider);

            loggerProvider.Info(
                $"Instance of {nameof(IPersonManager)} pulled back.");

            return toReturn;
        }

        private static HttpStatusCode InvokePersonManager<TRequestBody>(
            HttpRequest httpRequest,
            ILoggerProvider loggerProvider,
            Func<ILoggerProvider, TRequestBody, bool> validateModelLogic,
            Func<ILoggerProvider, IPersonManager, TRequestBody, HttpStatusCode> serviceInvocationLogic)
            where TRequestBody : Models.ModelsBase
        {
            HttpStatusCode toReturn;

            // Get our entry point in the logic layer and...
            IPersonManager personManager =
                GetIPersonManagerInstance(loggerProvider);

            TRequestBody requestBody = ParseRequestBody<TRequestBody>(
                httpRequest,
                loggerProvider);

            // First, if the requestBody is not null then...
            // Validate the top level of the model then...
            // Validate the rest via the function...
            // If all that passes, then continue.
            bool passedValidation =
                requestBody != null
                    &&
                ValidateModel(loggerProvider, requestBody)
                    &&
                validateModelLogic(loggerProvider, requestBody);

            if (passedValidation)
            {
                toReturn = serviceInvocationLogic(
                    loggerProvider,
                    personManager,
                    requestBody);
            }
            else
            {
                // If person == null, then the JSON couldn't be parsed
                // correctly. Return a BadRequest.
                toReturn = HttpStatusCode.BadRequest;
            }

            return toReturn;
        }

        private static TRequestBody ParseRequestBody<TRequestBody>(
            HttpRequest httpRequest,
            ILoggerProvider loggerProvider)
            where TRequestBody : Models.ModelsBase
        {
            TRequestBody toReturn = null;

            Type requestBodyType = typeof(TRequestBody);

            loggerProvider.Debug("Reading the request body...");

            string requestBody = null;
            using (StreamReader streamReader = new StreamReader(httpRequest.Body))
            {
                requestBody = streamReader.ReadToEnd();
            }

            loggerProvider.Debug(
                $"Request body: \"{requestBody}\". Parsing body into " +
                $"{requestBodyType.Name} instance...");

            try
            {
                toReturn = JsonConvert.DeserializeObject<TRequestBody>(
                    requestBody);

                loggerProvider.Info($"Parsed: {toReturn}.");
            }
            catch (JsonReaderException jsonReaderException)
            {
                loggerProvider.Warning(
                    $"A {nameof(JsonReaderException)} was thrown when " +
                    $"deserialising the request body \"{requestBody}\". The " +
                    $"exception message: \"{jsonReaderException.Message}\".");
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
    }
}