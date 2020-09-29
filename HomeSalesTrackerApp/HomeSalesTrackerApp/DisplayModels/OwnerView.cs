namespace HomeSalesTrackerApp.DisplayModels
{
    public class OwnerView : PersonView
    {
        public int OwnerID { get; set; }
        public string PreferredLender { get; set; }
        public override string PersonType => this.GetType().Name;
        public override string GetPersonType()
        {
            return this.GetPersonType();
        }

    }
}
