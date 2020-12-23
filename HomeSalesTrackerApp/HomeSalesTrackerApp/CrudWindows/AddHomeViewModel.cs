using HomeSalesTrackerApp.DisplayModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HomeSalesTrackerApp.CrudWindows
{
    class AddHomeViewModel
    {
        public HomeDisplayModel NewHome {   get; set; }
        HSTDataLayer.Person SelectedOwner { get; set; }
        IList<HSTDataLayer.Person> ExistingOwnersList { get; set; }
        string PreferredLender { get; set; } = string.Empty;
        public AddHomeViewModel()
        {
            LoadOwnersList();
            NewHome = new HomeDisplayModel();
            ExistingOwnersList = new List<HSTDataLayer.Person>();
            SelectedOwner = new HSTDataLayer.Person();
        }
        protected void AddNewHome()
        {
            if (NewHome != null)
            {
                var homeToAdd = new HSTDataLayer.Home()
                {
                    Address = NewHome.Address,
                    City = NewHome.City,
                    State = NewHome.State,
                    Zip = NewHome.Zip,
                    OwnerID = SelectedOwner.PersonID
                };

                ((App)Application.Current)._homesCollection.Add(homeToAdd);
            }
        }
        private void LoadOwnersList()
        {
            ExistingOwnersList = ((App)Application.Current)._peopleCollection.Where<HSTDataLayer.Person>(p => p.Owner != null).ToList();
            //ExistingOwnersList = (from p in ((App)Application.Current)._peopleCollection
            //                      where p.Owner != null
            //                      select p).ToList();
        }
    }
}
