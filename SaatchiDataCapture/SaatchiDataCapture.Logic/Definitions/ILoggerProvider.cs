// TODO: Log other types of error message? e.g. Debug?
namespace SaatchiDataCapture.Logic.Definitions
{
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