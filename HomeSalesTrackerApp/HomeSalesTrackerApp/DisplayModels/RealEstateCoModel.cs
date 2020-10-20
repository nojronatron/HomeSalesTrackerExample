namespace HomeSalesTrackerApp.DisplayModels
{
    public class RealEstateCoModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public string ToStackedString()
        {
            return $"Company ID: { CompanyID }\n" +
                $"RE Co Name: { CompanyName }\n" +
                $"RE Co Phone: { Phone }";
        }

    }

}
