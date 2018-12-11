namespace MarketingDataCapture.Logic.Definitions
{
    /// <summary>
    /// Describes the operations of the <see cref="IPersonManager" /> factory.
    /// </summary>
    public interface IPersonManagerFactory
    {
        /// <summary>
        /// Creates an instance of type <see cref="IPersonManager" />.
        /// </summary>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IPersonManager" />.
        /// </returns>
        IPersonManager Create(ILoggerProvider loggerProvider);
    }
}