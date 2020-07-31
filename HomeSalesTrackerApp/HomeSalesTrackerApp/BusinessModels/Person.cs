namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class Person
    {
        public int PersonID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public virtual Agent Agent { get; set; }

        public virtual Buyer Buyer { get; set; }

        public virtual Owner Owner { get; set; }
    }
}
