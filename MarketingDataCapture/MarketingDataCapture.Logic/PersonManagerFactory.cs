﻿namespace MarketingDataCapture.Logic
{
    using System.Diagnostics.CodeAnalysis;
    using MarketingDataCapture.Data.Definitions;
    using MarketingDataCapture.Logic.Definitions;

    /// <summary>
    /// Implements <see cref="IPersonManagerFactory" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PersonManagerFactory : IPersonManagerFactory
    {
        private readonly IDataCaptureDatabaseAdapter dataCaptureDatabaseAdapter;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="PersonManagerFactory" /> class.
        /// </summary>
        /// <param name="dataCaptureDatabaseAdapter">
        /// An instance of type <see cref="IDataCaptureDatabaseAdapter" />.
        /// </param>
        public PersonManagerFactory(
            IDataCaptureDatabaseAdapter dataCaptureDatabaseAdapter)
        {
            this.dataCaptureDatabaseAdapter = dataCaptureDatabaseAdapter;
        }

        /// <inheritdoc />
        public IPersonManager Create(ILoggerProvider loggerProvider)
        {
            PersonManager toReturn = new PersonManager(
                this.dataCaptureDatabaseAdapter,
                loggerProvider);

            return toReturn;
        }
    }
}
