namespace SaatchiDataCapture.Data.Models
{
    /// <summary>
    /// Abstract base class for all models in the
    /// <c>SaatchiDataCapture.Data</c> namespace.
    /// </summary>
    public abstract class ModelsBase : Common.Models.ModelsBase
    {
        /// <summary>
        /// Gets or sets the id of the inheriting entity.
        /// </summary>
        public long Id
        {
            get;
            set;
        }
    }
}