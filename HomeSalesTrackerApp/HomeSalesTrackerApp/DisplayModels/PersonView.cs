using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class PersonView
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public virtual string PersonType { get; set; }
        public virtual string GetPersonType()
        {
            return this.PersonType;
        }

        public override string ToString()
        {
            //  TODO: solve why a null Phone property gets sent and which caller sends it
            long phonenumber = long.Parse(Phone);
            return $"{ this.PersonID }{ this.FirstName }{ this.LastName }{ string.Format("{0:###-###-####}", this.Phone)}{ this.Email }";
        }
    }

}
