namespace SaatchiDataCapture.FunctionApp
{
    using Microsoft.Azure.WebJobs.Host;
    using SaatchiDataCapture.Logic.Definitions;

    public class LoggerProvider : ILoggerProvider
    {
        private readonly TraceWriter traceWriter;

        public LoggerProvider(TraceWriter traceWriter)
        {
            this.traceWriter = traceWriter;
        }

        public void Info(string message)
        {
            this.traceWriter.Info(message);
        }
    }
}
