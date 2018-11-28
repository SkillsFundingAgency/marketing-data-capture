namespace SaatchiDataCapture.Logic.Definitions
{
    using System;

    /// <summary>
    /// Describes the operations of the logger provider.
    /// </summary>
    public interface ILoggerProvider
    {
        /// <summary>
        /// Logs a <paramref name="message" /> with info-level importance.
        /// </summary>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void Info(string message);
    }
}