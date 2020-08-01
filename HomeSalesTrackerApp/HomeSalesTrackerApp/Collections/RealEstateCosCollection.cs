using System;
using System.Collections;
using System.Collections.Generic;
using HSTDataLayer;

namespace HomeSalesTrackerApp
{
    public class RealEstateCosCollection : IEnumerable<RealEstateCompany>
    {
        private List<RealEstateCompany> _recoList = null;
        public int Count {  get { return _recoList.Count; } }

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
            _recoList.Sort();
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

        public bool Add(RealEstateCompany realEstateCompany)
        {
            bool result = false;
            if (realEstateCompany != null)
            {
                int recoIdx = _recoList.FindIndex(r => r.CompanyID == realEstateCompany.CompanyID);
                if (recoIdx < 0)
                {
                    _recoList.Add(realEstateCompany);
                    result = true;
                    _recoList.Sort();
                }
            }
            return result;
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
            int elementsUpdated = 0;
            if (realEstateCompany != null)
            {
                int recoToUpdate = realEstateCompany.CompanyID;
                int idx = _recoList.FindIndex(r => r.CompanyID == recoToUpdate);
                RealEstateCompany selectedReco = _recoList[idx];
                if (selectedReco.CompanyName != realEstateCompany.CompanyName)
                {
                    _recoList[idx].CompanyName = realEstateCompany.CompanyName;
                    elementsUpdated++;
                }
                if (selectedReco.Phone != realEstateCompany.Phone)
                {
                    _recoList[idx].Phone = realEstateCompany.Phone;
                    elementsUpdated++;
                }
                if (elementsUpdated > 0)
                {
                    _recoList.Sort();
                }
            }
            return elementsUpdated;
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
