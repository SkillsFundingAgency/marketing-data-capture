namespace SaatchiDataCapture.Data.Definitions
{
    using SaatchiDataCapture.Data.Models;
    using System;

    public interface IDataCaptureDatabaseContract
    {
        CreatedEntityReference Create_Person(
            DateTime created,
            DateTime enrolled,
            string firstName,
            string lastName);
    }
}
