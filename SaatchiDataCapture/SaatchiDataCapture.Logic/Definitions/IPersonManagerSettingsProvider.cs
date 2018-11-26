namespace SaatchiDataCapture.Logic.Definitions
{
    /// <summary>
    /// Describes the operations of the <see cref="IPersonManager" /> settings
    /// provider.
    /// </summary>
    public interface IPersonManagerSettingsProvider
    {
        /// <summary>
        /// Gets the data capture database's connection string.
        /// </summary>
        string DataCaptureDatabaseConnectionString
        {
            get;
        }
    }
}