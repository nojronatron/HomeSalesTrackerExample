using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;
using HSTDataLayer.Helpers;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp
{
    public class HomesCollection : IEnumerable<Home>
    {
        private CollectionMonitor collectionMonitor = null;
        private List<Home> _homesList = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HomesCollection()
        {
            //_homesList = new List<Home>();
            _homesList = new List<Home>(EntityLists.GetTreeListOfHomes());
            collectionMonitor = new CollectionMonitor();
        }

        public HomesCollection(List<Home> homes)
        {
            _homesList = homes;
            collectionMonitor = new CollectionMonitor();
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
        public int Add(Home home)
        {
            if (home != null)
            {
                int preCount = this.Count;
                Home collectionHome = _homesList.SingleOrDefault(h => h.Address == home.Address &&
                                                                      h.Zip == home.Zip);

                if (collectionHome == null)
                {

                    if (LogicBroker.StoreItem<Home>(home))
                    {
                        Home storedHome = LogicBroker.GetHome(home.HomeID);

                        if (storedHome != null)
                        {
                            this._homesList.Add(storedHome);

                            if (this.Count > preCount)
                            {
                                collectionMonitor.SendNotifications(1, "Home");
                                return 1;
                            }
                        }
                    }
                }

            }

            return 0;
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
        /// Updates an existing home instance in database and Collection using Home arg. Returns 1 if change(s) applied, 0 if no changes made.
        /// </summary>
        /// <param name="home"></param>
        /// <returns name="int"></returns>
        public int Update(Home home)
        {
            if (home != null)
            {

                int homeIDX = _homesList.FindIndex(h => h.HomeID == home.HomeID);
                Home collectionHome = null;

                if (homeIDX > -1)
                {
                    collectionHome = _homesList[homeIDX];
                }

                if (collectionHome != null)
                {

                    if (LogicBroker.UpdateExistingItem<Home>(home))
                    {
                        Home storedHome = LogicBroker.GetHome(home.HomeID);

                        if (storedHome != null)
                        {
                            this._homesList[homeIDX] = storedHome;
                            collectionMonitor.SendNotifications(1, "Home");
                            return 1;
                        }
                    }
                }
                else
                {
                    return this.Add(home);
                }
            }

            return 0;
        }

        /// <summary>
        /// Accepts a Home instance and attempts to remove from the Database and the Collection. Returns True if succeeds, False if no changes made.
        /// </summary>
        /// <param name="home"></param>
        /// <returns></returns>
        public bool Remove(Home home)
        {
            if (home != null)
            {

                if (_homesList.Contains(home))
                {

                    if (LogicBroker.RemoveEntity<Home>(home))
                    {

                        if (_homesList.Remove(home))
                        {
                            collectionMonitor.SendNotifications(1, "Home");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes an item from this collection by its HomeID.
        /// </summary>
        /// <param name="homeID"></param>
        public bool Remove(int homeID)
        {
            bool result = false;
            Home homeToRemove = _homesList.Where(hs => hs.HomeID == homeID).FirstOrDefault();

            if (homeToRemove != null)
            {
                result = this.Remove(homeToRemove);
            }

            return result;
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
