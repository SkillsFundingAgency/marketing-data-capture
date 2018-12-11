namespace MarketingDataCapture.FunctionApp.Infrastructure
{
    using System;
    using MarketingDataCapture.Data.Definitions;

    /// <summary>
    /// Implements <see cref="IDataCaptureDatabaseAdapterSettingsProvider" />.
    /// </summary>
    public class DataCaptureDatabaseAdapterSettingsProvider
        : IDataCaptureDatabaseAdapterSettingsProvider
    {
        /// <inheritdoc />
        public string DataCaptureDatabaseConnectionString
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                    "DataCaptureDatabaseConnectionString");

                return toReturn;
            }
        }
    }
}