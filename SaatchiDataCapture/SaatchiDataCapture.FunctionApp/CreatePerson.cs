namespace SaatchiDataCapture.FunctionApp
{
    using System.IO;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;
    using SaatchiDataCapture.Logic.Definitions;
    using StructureMap;
    using StructureMap.Graph.Scanning;

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

            traceWriter.Info(
                $"Pulling back an instance of {nameof(IPersonManager)}...");

            Registry registry = new Registry();
            Container container = new Container(registry);

            IPersonManager personManager =
                container.GetInstance<IPersonManager>();

            traceWriter.Info(
                $"Instance of {nameof(IPersonManager)} pulled back.");

            toReturn = new OkObjectResult("TODO...");

            return toReturn;
        }
    }
}