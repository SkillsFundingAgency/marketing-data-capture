namespace SaatchiDataCapture.Data.Models
{
    using System;

    public class Person : ModelsBase
    {
        public DateTime Created
        {
            get;
            set;
        }

        public DateTime Enrolled
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }
    }
}
