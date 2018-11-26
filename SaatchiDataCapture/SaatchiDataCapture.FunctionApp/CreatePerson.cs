namespace SaatchiDataCapture.FunctionApp
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;

    public static class CreatePerson
    {
        [FunctionName("create-person")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)]
            HttpRequest httpRequest,
            TraceWriter traceWriter)
        {
            IActionResult toReturn = null;

            traceWriter.Info("C# HTTP trigger function processed a request.");

            string name = httpRequest.Query["name"];

            string requestBody = new StreamReader(httpRequest.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            toReturn = name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");

            return toReturn;
        }
    }
}