using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeDisplayModel : IDataErrorInfo, INotifyPropertyChanged
    {
        public int HomeID { get; set; }
        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != _address)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _zip;
        public string Zip
        {
            get
            {
                return _zip;
            }
            set
            {
                if (value != _zip)
                {
                    _zip = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ZipCode
        {
            get
            {
                return $"{Zip.Substring(0, 5)}-{Zip.Substring(5, 4)}";
            }
        }

        public string Error => "Error method called.";

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "Address":
                        {
                            if (this.Address.Length < 1 || this.Address.Length > 50)
                            {
                                result = "Address limited to 50 characters.";
                            }
                            break;
                        }
                    case "City":
                        {
                            if (this.City.Length < 2 || this.City.Length > 30)
                            {
                                result = "City limited to 50 characters.";
                            }
                            break;
                        }
                    case "State":
                        {
                            if (this.State.Length != 2)
                            {
                                result = "State must be 2 alpha-numeric characters.";
                            }
                            break;
                        }
                    case "Zip":
                        {
                            if (this.Zip.Length < 5 || this.Zip.Length > 9)
                            {
                                result = "Zip code must be 5 or 9 digits without spaces.";
                            }
                            break;
                        }
                    default:
                        {
                            result = string.Empty;
                            break;
                        }
                }

                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return $"{ this.HomeID } { this.Address } { this.City } { this.State } { this.Zip }";
        }

        public virtual string ToStackedString()
        {
            return $"HomeID: { this.HomeID }\n" +
                $"Address: { this.Address }\n" +
                $"City: { this.City }\n" +
                $"State: { this.State }\n" +
                $"Zip Code: { this.Zip.Substring(0,5) }-{ this.Zip.Substring(5,4) }";
        }
    }
}
