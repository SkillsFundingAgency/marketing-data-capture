namespace SaatchiDataCapture.Data.Definitions
{
    using System;
    using Meridian.InterSproc;
    using SaatchiDataCapture.Data.Models;

    /// <summary>
    /// Describes the stored procedures exposed by the data capture database.
    /// </summary>
    public interface IDataCaptureDatabaseContract
    {
        /// <summary>
        /// Executes the <c>Create_Person</c> stored procedure.
        /// </summary>
        /// <param name="created">
        /// Provides the <c>@Created</c> parameter.
        /// </param>
        /// <param name="enrolled">
        /// Provides the <c>@Enrolled</c> parameter.
        /// </param>
        /// <param name="firstName">
        /// Provides the <c>@FirstName</c> parameter.
        /// </param>
        /// <param name="lastName">
        /// Provides the <c>@LastName</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="CreatePersonResult" />.
        /// </returns>
        [InterSprocContractMethod(Name = "Create_Person")]
        CreatePersonResult CreatePerson(
            DateTime created,
            DateTime enrolled,
            string firstName,
            string lastName);

        /// <summary>
        /// Executes the <c>Read_ContactDetail</c> stored procedure.
        /// </summary>
        /// <param name="emailAddress">
        /// Provides the <c>@EmailAddress</c> parameter.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="ReadContactDetailResult" />.
        /// </returns>
        [InterSprocContractMethod(Name = "Read_ContactDetail")]
        ReadContactDetailResult ReadContactDetail(string emailAddress);
    }
}
