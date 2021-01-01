using HomeSalesTrackerApp.DisplayModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.CrudWindows
{
    public class AddPersonViewModel
    {
        public PersonModel NewPerson { get; set; }
        public AddPersonViewModel()
        {
            NewPerson = new PersonModel()
            {
                FirstName = "enter first name",
                LastName = "enter last name",
                Email = "example: email@domain.ext",
                Phone = "3333334444"
            };
        }
    }
}
