namespace SaatchiDataCapture.FunctionApp.Infrastructure
{
    using Microsoft.Azure.WebJobs.Host;
    using SaatchiDataCapture.Logic.Definitions;

    /// <summary>
    /// Implements <see cref="ILoggerProvider" />.
    /// </summary>
    public class LoggerProvider : ILoggerProvider
    {
        private readonly TraceWriter traceWriter;

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggerProvider" />
        /// class.
        /// </summary>
        /// <param name="traceWriter">
        /// An instance of <see cref="TraceWriter" />.
        /// </param>
        public LoggerProvider(TraceWriter traceWriter)
        {
            this.traceWriter = traceWriter;
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            this.traceWriter.Info(message);
        }
    }
}
