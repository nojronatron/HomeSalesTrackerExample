using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeSearchView
    {
        public int HomeID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public override string ToString()
        {
            return $"{ this.HomeID }{ this.Address }{ this.City }{ this.State }{ this.Zip }";
        }
    }

}
