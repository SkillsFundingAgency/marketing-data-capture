namespace SaatchiDataCapture.FunctionApp
{
    using System;
    using SaatchiDataCapture.Logic.Definitions;

    /// <summary>
    /// Implements <see cref="IPersonManagerSettingsProvider" />.
    /// </summary>
    public class PersonManagerSettingsProvider : IPersonManagerSettingsProvider
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