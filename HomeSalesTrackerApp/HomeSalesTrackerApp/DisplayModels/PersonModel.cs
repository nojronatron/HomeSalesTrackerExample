using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class PersonModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private bool _isValid;

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (value != _isValid)
                {
                    _isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PersonID { get; set; }
        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (value != _lastName)
                {
                    value = _lastName;
                    OnPropertyChanged();
                }
            }
        }
        private string _phone = "- not provided -";
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _eMail;
        public string Email
        {
            get
            {
                return _eMail;
            }
            set
            {
                if (value != _eMail)
                {
                    _eMail = value;
                    OnPropertyChanged();
                }
            }
        }
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

        public string Error => "Error method called.";

        public string this[string columnName]
        {
            get
            {
                this.IsValid = false;
                string result = string.Empty;
                switch (columnName)
                {
                    case "FirstName":
                        {
                            var min = 1;
                            var max = 50;
                            if (this.FirstName.Length < min || this.FirstName.Length > max)
                            {
                                result = $"First Name must be between { min } and { max } characters.";
                            }
                            else
                            {
                                this.IsValid = true;
                            }
                            break;
                        }
                    case "LastName":
                        {
                            var min = 1;
                            var max = 50;
                            if (this.LastName.Length < min || this.FirstName.Length > max)
                            {
                                result = $"First Name must be between { min } and { max } characters.";
                            }
                            else
                            {
                                this.IsValid = true;
                            }
                            break;
                        }
                    case "Phone":
                        {
                            var min = 7;
                            var max = 10;
                            if (this.Phone.Length < min || this.Phone.Length > max)
                            {
                                result = $"Phone number must be { min } to { max } digits long.";
                            }
                            else
                            {
                                this.IsValid = true;
                            }
                            break;
                        }
                    case "Email":
                        {
                            var min = 5;
                            var max = 50;
                            if ( !(this.Email.Contains("@") && this.Email.Contains(".")) || this.Email.Length < min || this.Email.Length > max)
                            {
                                result = $"Email must be in correct format and be { min } to { max } characters long.";
                            }
                            else
                            {
                                this.IsValid = true;
                            }
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                return result;
            }
            set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
