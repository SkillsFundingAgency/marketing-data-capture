namespace MarketingDataCapture.Data.Models
{
    using System;
    using MarketingDataCapture.Data.Definitions;

    /// <summary>
    /// Represents the return type for the
    /// <see cref="IDataCaptureDatabaseAdapter.CreateContactDetail(long, DateTime, DateTime, string, DateTime?)" />
    /// method.
    /// </summary>
    public class CreateContactDetailResult : ModelsBase
    {
        // Nothing - just inherits from the base.
    }
}