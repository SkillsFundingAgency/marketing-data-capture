namespace SaatchiDataCapture.FunctionApp.Models.CreatePersonBody
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a route.
    /// </summary>
    public class Route : ModelsBase
    {
        /// <summary>
        /// Gets or sets when the route was captured.
        /// </summary>
        [Required]
        public DateTime Captured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the route's identifier.
        /// </summary>
        [Required]
        public string RouteIdentifier
        {
            get;
            set;
        }
    }
}