namespace SaatchiDataCapture.FunctionApp
{
    using System;
    using SaatchiDataCapture.Logic.Definitions;

    public class PersonManagerSettingsProvider : IPersonManagerSettingsProvider
    {
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