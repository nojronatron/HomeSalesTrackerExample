namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeDisplayModel
    {
        public int HomeID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public override string ToString()
        {
            return $"{ this.HomeID } { this.Address } { this.City } { this.State } { this.Zip }";
        }

        public virtual string ToStackedString()
        {
            return $"HomeID: { this.HomeID }\n" +
                $"Address: { this.Address }\n" +
                $"City: { this.City }\n" +
                $"State: { this.State }\n" +
                $"Zip Code: { this.Zip.Substring(0,5) }-{ this.Zip.Substring(5,4) }";
        }
    }
}
