namespace MarketingDataCapture.Data.Models
{
    using System.Diagnostics.CodeAnalysis;
    using MarketingDataCapture.Data.Definitions;

    /// <summary>
    /// Represents the return type for the
    /// <see cref="IDataCaptureDatabaseAdapter.ReadPerson(string)" /> method.
    /// </summary>
    public class ReadPersonResult : ModelsBase
    {
        /// <summary>
        /// Gets or sets the value of the <c>ContactDetail_Id</c> column.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1707",
            Justification = "This model represents the column output of a stored procedure. Underscores are used in the database, and therefore need to be reflected here.")]
        public long ContactDetail_Id
        {
            get;
            set;
        }
    }
}