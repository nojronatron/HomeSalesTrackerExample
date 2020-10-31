namespace HomeSalesTrackerApp.DisplayModels
{
    public class PersonModel
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; } = "- not provided -";
        public string Email { get; set; }
        public virtual string PersonType { get; set; } = string.Empty;
        public virtual string PersonDetails { get; set; } = string.Empty;

        public string FullName => $"{ this.FirstName } { this.LastName }";
        public string PhoneNumber
        {
            get
            {
                if (this.Phone.Length > 0)
                {
                    return $"({ this.Phone.Substring(0,3) }) { this.Phone.Substring(3,3) }-{ this.Phone.Substring(6,4) }";
                }
                return Phone;
            }
        }
        public virtual string GetPersonType()
        {
            return this.PersonType;
        }

        public override string ToString()
        {
            return $"{ this.PersonID } { this.FirstName } { this.LastName } { this.Phone } { this.Email }";
        }

        public virtual string ToStackedString()
        {
            return $"ID: { this.PersonID }\n" +
                $"Name: { this.FullName }\n" +
                $"Phone: { this.PhoneNumber }\n" +
                $"EMail: { this.Email }";
        }

    }
}
