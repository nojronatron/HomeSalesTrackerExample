using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HomeSalesTrackerApp.CrudWindows
{
    class AddHomeViewModel
    {
        protected Home NewHome { get; set; }
        protected Person NewPerson { get; set; }
        protected Owner NewOwner { get; set; }
        public AddHomeViewModel()
        {
            NewHome = new Home();
            NewPerson = new Person();
            NewOwner = new Owner();
        }
        protected void AddNewHome()
        {
            if (NewHome != null)
            {
                ((App)Application.Current)._homesCollection.Add(NewHome);
            }
        }
    }
}
