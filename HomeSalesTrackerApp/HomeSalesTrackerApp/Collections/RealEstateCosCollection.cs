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
                var collectionReco = _recoList.SingleOrDefault(re => re.CompanyName == realEstateCompany.CompanyName);
                int preCount = this.Count;
                
                if (collectionReco == null)
                {
                    if (LogicBroker.StoreItem<RealEstateCompany>(realEstateCompany))
                    {
                        //  TODO: Get a reference to the just-stored RECo
                        RealEstateCompany dbReco = LogicBroker.GetReCompany(realEstateCompany.CompanyID);
                        if (dbReco != null)
                        {
                            this._recoList.Add(dbReco);
                            if (this.Count > preCount)
                            {
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
                RealEstateCompany collectionRECo = _recoList[realEstateCompanyIDX];
                if (collectionRECo != null)
                {
                    RealEstateCompany dbRECo = null;

                    if (LogicBroker.StoreItem<RealEstateCompany>(realEstateCompany))
                    {
                        //  TODO: Get a reference to the stored item in EF
                        dbRECo = LogicBroker.GetReCompany(realEstateCompany.CompanyID);
                        if (dbRECo != null)
                        {
                            this._recoList[realEstateCompanyIDX] = dbRECo;
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
            if (companyID > -1)
            {
                int recoIdx = _recoList.FindIndex(r => r.CompanyID == companyID);
                _recoList.RemoveAt(recoIdx);
                _recoList.Sort();
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
