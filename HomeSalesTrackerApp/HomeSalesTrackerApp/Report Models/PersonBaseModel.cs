using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.Report_Models
{
    public class PersonBaseModel :
        IEqualityComparer<PersonBaseModel>, IEquatable<PersonBaseModel>
    {
        private int _personID;

        public int PersonID
        {
            get { return _personID; }
            set { _personID = value; }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                }
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                }
            }
        }

        private string _phone;

        public string Phone
        {
            get
            {
                return $"({ _phone.Substring(0, 3) }) { _phone.Substring(3, 3) }-{ _phone.Substring(6, 4) }";
            }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                }
            }
        }

        private string _eMail;

        public string EMail
        {
            get { return _eMail; }
            set
            {
                if (_eMail != value)
                {
                    _eMail = value;
                }
            }
        }

        public string FullName => $"{ this.FirstName } { this.LastName }";

        public override bool Equals(object obj)
        {
            return obj is PersonBaseModel model &&
                   PersonID == model.PersonID;
        }

        public override int GetHashCode()
        {
            return -360234075 + PersonID.GetHashCode();
        }

        public bool Equals(PersonBaseModel x, PersonBaseModel y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.PersonID == y.PersonID;
        }

        public int GetHashCode(PersonBaseModel obj)
        {
            return (this.PersonID).GetHashCode();
        }

        public bool Equals(PersonBaseModel other)
        {
            return other is PersonBaseModel model &&
                PersonID == model.PersonID;
        }

        public static bool operator ==(PersonBaseModel left, PersonBaseModel right)
        {
            return EqualityComparer<PersonBaseModel>.Default.Equals(left, right);
        }

        public static bool operator !=(PersonBaseModel left, PersonBaseModel right)
        {
            return !(left == right);
        }
    }
}
