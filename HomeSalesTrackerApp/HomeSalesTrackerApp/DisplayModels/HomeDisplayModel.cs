using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeDisplayModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private bool _isValid;
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            private set
            {
                if (value != _isValid)
                {
                    _isValid = value;
                    OnPropertyChanged();
                }
            }
        }
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
                this.IsValid = false;
                string result = string.Empty;
                switch (columnName)
                {
                    case "Address":
                        {
                            var min = 1;
                            var max = 50;
                            if (this.Address.Length < min || this.Address.Length > max)
                            {
                                result = $"Address must be { min } to { max } characters.";
                            }
                            else
                            {
                                this.IsValid = true;
                            }
                            break;
                        }
                    case "City":
                        {
                            var min = 2;
                            var max = 30;
                            if (this.City.Length < min || this.City.Length > max)
                            {
                                result = $"City must be { min } to { max } characters.";
                            }
                            else
                            {
                                this.IsValid = true;
                            }
                            break;
                        }
                    case "State":
                        {
                            var minMax = 2;
                            if (this.State.Length != minMax)
                            {
                                result = $"State must be { minMax } letter abbreviation.";
                            }
                            else
                            {
                                this.IsValid = true;
                            }
                            break;
                        }
                    case "Zip":
                        {
                            var min = 5;
                            var max = 9;
                            if (!(int.TryParse(this.Zip, out int parsedZip)) || this.Zip.Length < min || this.Zip.Length > max)
                            {
                                result = "Zip code must be 5 or 9 digits without spaces or dashes.";
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
