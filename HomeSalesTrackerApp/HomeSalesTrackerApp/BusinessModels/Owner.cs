namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class Owner : Person
    {
        public Owner()
        {
            Homes = new HashSet<Home>();
        }

        public int OwnerID { get; set; }

        public string PreferredLender { get; set; }

        public virtual ICollection<Home> Homes { get; set; }

        public virtual Person Person { get; set; }

        public override string ToString()
        {
            return $"{ base.ToString() } { this.PreferredLender }";
        }

    }
}
