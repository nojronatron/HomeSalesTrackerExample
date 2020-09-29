namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeSearchView : HomeView
    {
        public override string ToString()
        {
            return $"{ this.HomeID }{ this.Address }{ this.City }{ this.State }{ this.Zip }";
        }
    }

}
