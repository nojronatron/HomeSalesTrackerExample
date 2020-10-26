using HSTDataLayer;

using System;
using System.Collections;
using System.Collections.Generic;

namespace HomeSalesTrackerApp
{
    public class HomesCollection : IEnumerable<Home>    //, INotifyPropertyChanged
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
                if (idx < 0 || idx > Count - 1)
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
        public bool Add(Home home)
        {
            if (home != null)
            {
                if (LogicBroker.SaveEntity<Home>(home))
                {
                    Home dbHome = LogicBroker.GetHome(home.HomeID);
                    _homesList.Add(dbHome);
                    return true;
                }
            }

            return false;
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
            if (home == null)   //  null check
            {
                return 0;
            }

            //  check if arg exists in collection
            int homeIDX = _homesList.FindIndex(h => h.HomeID == home.HomeID);
            Home collectionHome = _homesList[homeIDX];
            if (home.Equals(collectionHome))
            { 
                if (LogicBroker.UpdateEntity<Home>(home))
                {
                    Home dbHome = LogicBroker.GetHome(homeIDX);
                    _homesList[homeIDX] = dbHome;
                    return 1;
                }
            }
            else
            {
                if (this.Add(home))
                {
                    return 1;
                }
            }

            return 0;
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
