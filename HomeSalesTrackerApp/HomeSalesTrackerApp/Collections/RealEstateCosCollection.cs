﻿using HomeSalesTrackerApp.Helpers;
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

        public RealEstateCompany Retrieve(int companyID)
        {
            var reco = new RealEstateCompany();

            if (companyID > -1)
            {
                reco = _recoList.Find(r => r.CompanyID == companyID);
            }

            return reco;
        }

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

        public void Remove(int companyID)
        {
            int preCount = this.Count;

            if (companyID > -1)
            {
                int recoIdx = _recoList.FindIndex(r => r.CompanyID == companyID);
                _recoList.RemoveAt(recoIdx);
            }

            if (preCount > this.Count)
            {
                collectionMonitor.SendNotifications(1, "RECo");
            }

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
