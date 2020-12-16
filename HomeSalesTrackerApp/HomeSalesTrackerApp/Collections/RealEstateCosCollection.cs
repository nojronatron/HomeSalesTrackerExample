using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp
{
    public class RealEstateCosCollection : IEnumerable<RealEstateCompany>
    {
        private List<RealEstateCompany> _recoList = null;
        public int Count { get { return _recoList.Count; } }
        public CollectionMonitor collectionMonitor = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public RealEstateCosCollection()
        {
            _recoList = new List<RealEstateCompany>();
        }

        /// <summary>
        /// Constructor will initialize this Collection with a List of type RealEstateCompany instances.
        /// </summary>
        /// <param name="reCompanies"></param>
        public RealEstateCosCollection(List<RealEstateCompany> reCompanies)
        {
            _recoList = reCompanies;
            collectionMonitor = new CollectionMonitor();
        }

        /// <summary>
        /// Indexer. Allows indexing into this Collection using square brackets.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public RealEstateCompany this[int idx]
        {
            get
            {
                if (idx < 0 || idx > Count)
                {
                    throw new ArgumentOutOfRangeException("Unable to retrieve item from an invalid index.");
                }
                else
                {
                    return _recoList[idx];
                }
            }
        }

        /// <summary>
        /// Attempts to add a RealEstateCompany instance to the Collection and the Database. Returns True if succeeds, False if otherwise.
        /// </summary>
        /// <param name="realEstateCompany"></param>
        /// <returns></returns>
        public int Add(RealEstateCompany realEstateCompany)
        {
            if (realEstateCompany != null)
            {
                int preCount = this.Count;
                RealEstateCompany collectionReco = _recoList
                    .SingleOrDefault(re =>
                        re.CompanyName == realEstateCompany.CompanyName);

                if (collectionReco == null)
                {

                    if (LogicBroker.StoreItem<RealEstateCompany>(realEstateCompany))
                    {
                        RealEstateCompany storedReco = LogicBroker.GetReCompany(realEstateCompany.CompanyID);

                        if (storedReco != null)
                        {
                            this._recoList.Add(storedReco);

                            if (this.Count > preCount)
                            {
                                collectionMonitor.SendNotifications(1, "RECo");
                                return 1;
                            }
                        }
                    }
                }

            }

            return 0;
        }

        /// <summary>
        /// Retreives a RealEstateCompany instance from the Collection by CompanyID. Returns Null if CompanyID not in Collection.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public RealEstateCompany Retrieve(int companyID)
        {
            var reco = new RealEstateCompany();

            if (companyID > -1)
            {
                reco = _recoList.Find(r => r.CompanyID == companyID);
            }

            return reco;
        }

        /// <summary>
        /// Attempts to update a RealEstateCompany instance in the Collection. Returns 1 if succeeds, 0 if fails.
        /// </summary>
        /// <param name="realEstateCompany"></param>
        /// <returns></returns>
        public int Update(RealEstateCompany realEstateCompany)
        {
            if (realEstateCompany != null)
            {
                int realEstateCompanyIDX = _recoList.FindIndex(r => r.CompanyID == realEstateCompany.CompanyID);
                RealEstateCompany collectionRECo = null;

                if (realEstateCompanyIDX > -1)
                {
                    collectionRECo = _recoList[realEstateCompanyIDX];
                }

                if (collectionRECo != null)
                {
                    if (LogicBroker.UpdateExistingItem<RealEstateCompany>(realEstateCompany))
                    {
                        RealEstateCompany storedRECo = LogicBroker.GetReCompany(realEstateCompany.CompanyID);

                        if (storedRECo != null)
                        {
                            this._recoList[realEstateCompanyIDX] = storedRECo;
                            collectionMonitor.SendNotifications(1, "RECo");
                            return 1;
                        }
                    }
                }
                else
                {
                    return this.Add(realEstateCompany);
                }

            }

            return 0;
        }

        /// <summary>
        /// Attempts to remove a RealEstateCompany instance from the Collection and the Database. Returns True if succeeds, False if otherwise.
        /// </summary>
        /// <param name="realEstateCompany"></param>
        /// <returns></returns>
        public bool Remove(RealEstateCompany realEstateCompany)
        {
            if (realEstateCompany != null)
            {

                if (_recoList.Contains(realEstateCompany))
                {

                    if (LogicBroker.RemoveEntity<RealEstateCompany>(realEstateCompany))
                    {

                        if (_recoList.Remove(realEstateCompany))
                        {
                            collectionMonitor.SendNotifications(1, "RECo");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to remove a RealEstateCompany instance from teh Collection and the Database, by CompanyID. Returns True if succeeds, False if otherwise.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool Remove(int companyID)
        {
            bool result = false;
            RealEstateCompany recoToRemove = _recoList.Where(re => re.CompanyID == companyID).FirstOrDefault();

            if (recoToRemove != null)
            {
                result = this.Remove(recoToRemove);
            }

            return result;
        }

        public IEnumerator<RealEstateCompany> GetEnumerator()
        {
            return ((IEnumerable<RealEstateCompany>)_recoList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<RealEstateCompany>)_recoList).GetEnumerator();
        }

    }

}
