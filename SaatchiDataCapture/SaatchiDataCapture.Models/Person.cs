namespace SaatchiDataCapture.Models
{
    /// <summary>
    /// Represents a person.
    /// </summary>
    public class Person : ModelsBase
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName
        {
            get;
            set;
        }
    }
}
