namespace MarketingDataCapture.FunctionApp.Infrastructure
{
    using System;
    using MarketingDataCapture.Logic.Definitions;
    using Microsoft.Azure.WebJobs.Host;

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
        public void Debug(string message)
        {
            this.traceWriter.Verbose(message);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            this.traceWriter.Info(message);
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
            this.traceWriter.Warning(message);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception)
        {
            this.traceWriter.Error(message, exception);
        }
    }
}
