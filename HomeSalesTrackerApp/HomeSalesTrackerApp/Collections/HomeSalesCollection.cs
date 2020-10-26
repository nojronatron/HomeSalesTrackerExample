using HSTDataLayer;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp
{
    public class HomeSalesCollection : IEnumerable<HomeSale>
    {
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
        /// Add a HomeSale instance to this collection. Will only accept object that has SoldDate member that is not null.
        /// </summary>
        /// <param name="homeSale"></param>
        public bool Add(HomeSale homeSale)
        {
            if (homeSale == null)
            {
                return false;
            }

            var inCollection = _homeSalesList.SingleOrDefault(hs => hs.HomeID == homeSale.HomeID
                                                              && hs.SaleAmount == homeSale.SaleAmount);
            if (inCollection == null)
            {
                if(LogicBroker.SaveEntity<HomeSale>(homeSale))
                {
                    HomeSale dbHomeSale = LogicBroker.GetHomeSale(homeSale.HomeID);
                    _homeSalesList.Add(dbHomeSale);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a single HomeSale instance item, by its SaleID.
        /// </summary>
        /// <param name="saleID"></param>
        /// <returns></returns>
        public HomeSale Retreive(int saleID)
        {
            HomeSale result = null;
            result = _homeSalesList.Find(x => x.SaleID == saleID);
            return result;
        }

        /// <summary>
        /// Find an existing entry and update it to the members in the input parameter. Return 1 if updated, 0 if not.
        /// </summary>
        /// <param name="homeSale"></param>
        /// <returns></returns>
        public int Update(HomeSale homeSale)
        {
            if (homeSale == null)
            {
                return 0;
            }

            int homeSaleIDX = _homeSalesList.FindIndex(hs => hs.SaleID == homeSale.SaleID);
            HomeSale collectionHomeSale = _homeSalesList[homeSaleIDX];
            if (homeSale.Equals(collectionHomeSale))
            {
                if (LogicBroker.UpdateEntity<HomeSale>(homeSale)) 
                {
                    HomeSale dbHomeSale = LogicBroker.GetHomeSale(homeSaleIDX);
                    _homeSalesList[homeSaleIDX] = dbHomeSale;
                    return 1;
                }

            }
            else
            {
                if (this.Add(homeSale))
                {
                    return 1;
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
            bool result = false;
            if (homesale != null)
            {
                result = _homeSalesList.Remove(homesale);
            }
            return result;
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
                _homeSalesList.Remove(hsToRemove);
                result = true;
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
