using System;
using System.Collections;
using System.Collections.Generic;
using HSTDataLayer;

namespace HomeSalesTrackerApp
{
    public class HomesCollection : IEnumerable<Home>//, INotifyPropertyChanged
    {
        private List<Home> _homesList = null;

        //  https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.7.2
        //public event PropertyChangedEventHandler PropertyChanged;

        //  https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.7.2
        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        /// <summary>
        /// Constructor.
        /// </summary>
        public HomesCollection()
        {
            _homesList = new List<Home>();
        }

        public HomesCollection(List<Home> homes)
        {
            _homesList = homes;
            _homesList.Sort();
        }

        public int Count { get { return _homesList.Count; } }
        
        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Home this[int idx]
        {
            get
            {
                if (idx < 0 || idx > Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    return _homesList[idx];
                }
            }
        }

        /// <summary>
        /// Adds an instance to this item list.
        /// </summary>
        /// <param name="home"></param>
        public void Add(Home home)
        {
            if (home != null)
            {
                _homesList.Add(home);
            }
            _homesList.Sort();
        }

        /// <summary>
        /// Returns a specific item as identified by HomeID
        /// </summary>
        /// <param name="homeID"></param>
        /// <returns></returns>
        public Home Retreive(int homeID)
        {
            Home result = null;
            result = _homesList.Find(x => x.HomeID == homeID);
            return result;
        }

        /// <summary>
        /// Updates an existing ID via fully-fledged object instance.
        /// </summary>
        /// <param name="home"></param>
        /// <returns></returns>
        public int Update(Home home)
        {
            int elementsUpdated = 0;
            if (home != null)
            {
                int homeIdToUpdate = home.HomeID;
                int idx = _homesList.FindIndex(x => x.HomeID == homeIdToUpdate);
                Home selectedHome = _homesList[idx];
                if (selectedHome.Address != home.Address.Trim())
                {
                    _homesList[idx].Address = home.Address;
                    elementsUpdated++;
                }
                if (selectedHome.City != home.City.Trim())
                {
                    _homesList[idx].City = home.City;
                    elementsUpdated++;
                }
                if (selectedHome.State != home.State.Trim())
                {
                    _homesList[idx].State = home.State;
                    elementsUpdated++;
                }
                if (selectedHome.Zip != home.Zip.Trim())
                {
                    _homesList[idx].State = home.State;
                    elementsUpdated++;
                }
                if (selectedHome.OwnerID != home.OwnerID)
                {
                    _homesList[idx].OwnerID = home.OwnerID;
                    elementsUpdated++;
                }
                _homesList.Sort();
            }
            return elementsUpdated;
        }

        /// <summary>
        /// Removes an item from this collection by its HomeID.
        /// </summary>
        /// <param name="homeID"></param>
        public void Remove(int homeID)
        {
            if (homeID > 0)
            {
                int homeIdx = _homesList.FindIndex(x => x.HomeID == homeID);
                _homesList.RemoveAt(homeIdx);
                _homesList.Sort();
            }
        }

        IEnumerator<Home> IEnumerable<Home>.GetEnumerator()
        {
            return ((IEnumerable<Home>)_homesList).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<Home>)_homesList).GetEnumerator();
        }
    }

}
