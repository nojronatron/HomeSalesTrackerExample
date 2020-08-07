using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HSTDataLayer;

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
            //_homeSalesList.Sort();
        }

        public int Count {  get { return _homeSalesList.Count; } }

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
        public void Add(HomeSale homeSale)
        {
            if (homeSale != null)
            {
                var hsInList = _homeSalesList.SingleOrDefault(hs => hs.HomeID == homeSale.HomeID 
                                                              && hs.SaleAmount == homeSale.SaleAmount);
                if (hsInList == null )
                {
                    _homeSalesList.Add(homeSale);
                    //_homeSalesList.Sort();
                }
            }
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
            int itemUpdated = 0;
            if (homeSale != null)
            {
                int hsIndex = _homeSalesList.FindIndex(hs => hs.SaleID == homeSale.SaleID);
                if (hsIndex >= 0)
                {
                    _homeSalesList[hsIndex] = homeSale;
                    itemUpdated++;
                    //_homeSalesList.Sort();
                }
            }
            return itemUpdated;
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

        public int TotalHomesCurrentlyForSale()
        {
            throw new NotImplementedException("TotalHomesCurrentlyForSale() has not been implemented");
            //  return 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Home>)_homeSalesList).GetEnumerator();
        }

        public IEnumerator<HomeSale> GetEnumerator()
        {
            return ((IEnumerable<HomeSale>)_homeSalesList).GetEnumerator();
        }

    }
}
