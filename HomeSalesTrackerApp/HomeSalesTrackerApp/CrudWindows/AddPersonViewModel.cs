using HomeSalesTrackerApp.DisplayModels;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace HomeSalesTrackerApp.CrudWindows
{
    public class AddPersonViewModel
    {
        static public event PropertyChangedEventHandler PropertyChanged;
        public PersonModel NewPerson { get; set; }
        public AgentModel NewAgent { get; set; } = new AgentModel();
        public BuyerModel NewBuyer { get; set; } = new BuyerModel();
        public OwnerModel NewOwner { get; set; } = new OwnerModel();

        private string _personType;
        public string PersonType
        {
            get
            {
                return _personType;
            }
            private set
            {
                if (value != _personType)
                {
                    _personType = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_personType));
        }

        public AddPersonViewModel(string personType)
        {
            PersonType = personType;
            NewPerson = new PersonModel()
            {
                FirstName = "enter first name",
                LastName = "enter last name",
                Email = "example: email@domain.ext",
                Phone = "3333334444"
            };

            AddPersonView.RaiseNewPersonCreatedEvent += AddPersonView_RaiseNewPersonCreatedEvent1;
        }

        private void AddPersonView_RaiseNewPersonCreatedEvent1(object sender, string e)
        {
            AddNewPerson();
        }

        protected void AddNewPerson()
        {
            if (NewPerson != null)
            {
                var PersonToAdd = new HSTDataLayer.Person()
                {
                    FirstName = NewPerson.FirstName,
                    LastName = NewPerson.LastName,
                    Email = NewPerson.Email,
                    Phone = NewPerson.Phone
                    };

                int personID = ((App)Application.Current)._peopleCollection.Add(PersonToAdd);

            }
        }
    }
}
