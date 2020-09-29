namespace HomeSalesTrackerApp.DisplayModels
{
    public class PersonView
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public virtual string PersonType { get; set; } = string.Empty;

        public virtual string GetPersonType()
        {
            return this.PersonType;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Phone))
            {
                Phone = " ";
            }
            return $"{ this.PersonID }{ this.FirstName }{ this.LastName }{ this.Phone }{ this.Email }";
        }

    }
}
