namespace MarketingDataCapture.Data.Definitions
{
    /// <summary>
    /// Describes the operations of the
    /// <see cref="IDataCaptureDatabaseAdapter" /> settings provider.
    /// </summary>
    public interface IDataCaptureDatabaseAdapterSettingsProvider
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