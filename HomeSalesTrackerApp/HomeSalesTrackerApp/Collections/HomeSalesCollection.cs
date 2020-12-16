using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp
{
    public class HomeSalesCollection : IEnumerable<HomeSale>
    {
        private CollectionMonitor collectionMonitor = null;
        private List<HomeSale> _homeSalesList = null;

        /// <summary>
        /// Default constructor initializes an empty Collection of type HomeSale.
        /// </summary>
        public HomeSalesCollection()
        {
            _homeSalesList = new List<HomeSale>();
        }

        /// <summary>
        /// Custom constructor initializes this Collection with an existing List of type HomeSale instances.
        /// </summary>
        /// <param name="homeSales"></param>
        public HomeSalesCollection(List<HomeSale> homeSales)
        {
            _homeSalesList = homeSales;
            collectionMonitor = new CollectionMonitor();
        }

        public int Count { get { return _homeSalesList.Count; } }

        /// <summary>
        /// Indexer. Allows indexing into this collection using square brackets.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public HomeSale this[int idx]
        {
            get
            {
                if (idx < 0 || idx > Count - 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    return _homeSalesList[idx];
                }
            }
        }

        /// <summary>
        /// Add a HomeSale instance to this collection. Will only accept object whose MarketDate and SaleAmount members are not null 
        /// and the homesale instance is not already in the collection.
        /// </summary>
        /// <param name="homeSale"></param>
        public int Add(HomeSale homeSale)
        {
            if (homeSale != null)
            {
                int preCount = this.Count;
                HomeSale collectionHomeSale = _homeSalesList.SingleOrDefault(hs => hs.MarketDate == homeSale.MarketDate &&
                                                                              hs.SaleAmount == homeSale.SaleAmount);

                if (collectionHomeSale == null)
                {

                    if (LogicBroker.StoreItem<HomeSale>(homeSale))
                    {
                        HomeSale storedHomeSale = LogicBroker.GetHomeSale(homeSale.SaleID);

                        if (storedHomeSale != null)
                        {
                            _homeSalesList.Add(storedHomeSale);

                            if (this.Count > preCount)
                            {
                                collectionMonitor.SendNotifications(1, "HomeSale");
                                return 1;
                            }
                        }
                    }
                }

            }

            return 0;
        }

        /// <summary>
        /// Accepts an integer homesaleID and returns the HomeSale instance that matches.
        /// Returs null if no instance was found.
        /// </summary>
        /// <param name="homesaleID"></param>
        /// <returns></returns>
        public HomeSale Retreive(int homesaleID)
        {
            HomeSale result = null;
            result = _homeSalesList.Where(hfs => hfs.SaleID == homesaleID).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Accepts a Home instance and returns the HomeSale instances related to it.
        /// Returns a null List if no instances found.
        /// </summary>
        /// <param name="home"></param>
        /// <returns></returns>
        public List<HomeSale> Retreive(Home home)
        {
            List<HomeSale> result = null;
            result = _homeSalesList.Where(hfs => hfs.HomeID == home.HomeID).ToList();
            return result;
        }

        /// <summary>
        /// Find an existing entry and update it to the members in the input parameter. Return 1 if updated, 0 if not.
        /// Equals and IEquatable overloaded: MarketDate and SaleAmount are the inspected Properties.
        /// </summary>
        /// <param name="homeSale"></param>
        /// <returns></returns>
        public int Update(HomeSale homeSale)
        {
            if (homeSale != null)
            {
                int homeSaleIDX = _homeSalesList.FindIndex(hs => hs.SaleID == homeSale.SaleID);
                HomeSale collectionHomeSale = null;

                if (homeSaleIDX > -1)
                {
                    collectionHomeSale = _homeSalesList[homeSaleIDX];
                }

                if (collectionHomeSale != null)
                {

                    if (LogicBroker.UpdateExistingItem<HomeSale>(homeSale))
                    {
                        HomeSale storedHomeSale = LogicBroker.GetHomeSale(homeSale.SaleID);

                        if (storedHomeSale != null)
                        {
                            this._homeSalesList[homeSaleIDX] = storedHomeSale;
                            collectionMonitor.SendNotifications(1, "HomeSale");
                            return 1;
                        }
                    }

                }
                else
                {
                    return this.Add(homeSale);
                }

            }

            return 0;
        }

        /// <summary>
        /// Removes a specified instance from this Collection by instance. Returns true if succeeds, otherwise false.
        /// </summary>
        /// <param name="homesale"></param>
        /// <returns></returns>
        public bool Remove(HomeSale homesale)
        {
            if (homesale != null)
            {

                if (_homeSalesList.Contains(homesale))
                {

                    if (LogicBroker.RemoveEntity<HomeSale>(homesale))
                    {

                        if (_homeSalesList.Remove(homesale))
                        {
                            collectionMonitor.SendNotifications(1, "HomeSale");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes a specified instance from this Collection by SaleID. Returns true if succeeds, otherwise false.
        /// </summary>
        /// <param name="saleid"></param>
        /// <returns></returns>
        public bool Remove(int saleid)
        {
            bool result = false;
            HomeSale hsToRemove = _homeSalesList.Where(hs => hs.SaleID == saleid).FirstOrDefault();
            
            if (hsToRemove != null)
            {
                result = this.Remove(hsToRemove);
            }
            
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<HomeSale>)_homeSalesList).GetEnumerator();
        }

        public IEnumerator<HomeSale> GetEnumerator()
        {
            return ((IEnumerable<HomeSale>)_homeSalesList).GetEnumerator();
        }

    }
}
