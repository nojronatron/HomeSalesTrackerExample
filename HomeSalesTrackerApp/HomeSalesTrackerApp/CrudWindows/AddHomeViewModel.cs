using HomeSalesTrackerApp.DisplayModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HomeSalesTrackerApp.CrudWindows
{
    public class AddHomeViewModel
    {
        public HomeDisplayModel NewHome { get; set; }
        public HSTDataLayer.Person SelectedOwner { get; set; }
        public IList<HSTDataLayer.Person> ExistingOwnersList { get; private set; }
        public string PreferredLender { get; set; } = string.Empty;
        public AddHomeViewModel()
        {
            NewHome = new HomeDisplayModel()
            {
                Address = "enter new address",
                City = "enter city name",
                State = "AK",
                Zip = "123451234"
            };
            ExistingOwnersList = new List<HSTDataLayer.Person>();
            LoadOwnersList();
            SelectedOwner = ExistingOwnersList[0];
            AddHomeView.RaiseNewHomeCreatedEvent += AddHomeView_RaiseNewHomeCreatedEvent;
        }

        private void AddHomeView_RaiseNewHomeCreatedEvent(object sender, HSTDataLayer.Person e)
        {
            //  listener
            SelectedOwner = (HSTDataLayer.Person)e as HSTDataLayer.Person;
            AddNewHome();
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
        }

    }
}
