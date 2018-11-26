namespace SaatchiDataCapture.Logic
{
    using Meridian.InterSproc;
    using SaatchiDataCapture.Data.Definitions;
    using SaatchiDataCapture.Logic.Definitions;
    using SaatchiDataCapture.Models;
    using System;

    public class PersonManager : IPersonManager
    {
        private readonly IDataCaptureDatabaseContract dataCaptureDatabaseContract;
        private readonly ILoggerProvider loggerProvider;

        public PersonManager(
            ILoggerProvider loggerProvider,
            IPersonManagerSettingsProvider personManagerSettingsProvider)
        {
            this.loggerProvider = loggerProvider;

            string dataCaptureDatabaseConnectionString =
                personManagerSettingsProvider.DataCaptureDatabaseConnectionString;

            this.dataCaptureDatabaseContract =
                SprocStubFactory.Create<IDataCaptureDatabaseContract>(
                    dataCaptureDatabaseConnectionString);
        }

        public void Create(Person person)
        {
            this.loggerProvider.Info(
                $"Invoking " +
                $"{nameof(IDataCaptureDatabaseContract)}.{nameof(IDataCaptureDatabaseContract.Create_Person)}...");

            Data.Models.CreatedEntityReference createdEntityReference =
                this.dataCaptureDatabaseContract.Create_Person(
                    DateTime.Now,
                    DateTime.Now,
                    "Test",
                    "Test");

            this.loggerProvider.Info(
                $"Created: {createdEntityReference}.");
        }
    }
}