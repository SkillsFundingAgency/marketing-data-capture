namespace SaatchiDataCapture.FunctionApp.Models.UpdatePerson
{
    using System;

    /// <summary>
    /// Represents a route.
    /// </summary>
    public class Route : ModelsBase
    {
        /// <summary>
        /// Gets or sets when the route was captured.
        /// </summary>
        public DateTime Captured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the route's identifier.
        /// </summary>
        public string RouteIdentifier
        {
            get;
            set;
        }
    }
}