using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
