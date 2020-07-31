using HSTDataLayer;
using HSTDataLayer.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HomeSalesTrackerApp
{
    public class PeopleCollection<T> : IEnumerator<T>, IEnumerable<T>, IObservable<T>
                            where T : Person
    {
        public delegate void CollectionChangedHandler(Person person);
        public CollectionChangedHandler listOfHandlers;


        private static List<T> _peopleList = null;
        private static int position = -1;
        public int Count => _peopleList.Count;

        /// <summary>
        /// Constructor. Initializes an empty Collection.
        /// </summary>
        public PeopleCollection()
        {
            _peopleList = new List<T>();
        }

        public PeopleCollection(List<T> peopleList)
        {
            _peopleList = peopleList;
        }

        public T Current => _peopleList[position];
        object IEnumerator.Current => _peopleList[position];

        /// <summary>
        /// Allows caller to add a Person to the Collection based on PersonID
        /// </summary>
        /// <param name="person"></param>
        public bool Add(T person)
        {
            bool result = false;
            var people = _peopleList.SingleOrDefault(p => p.PersonID == person.PersonID);
            if (people == null)
            {
                _peopleList.Add(person);
                //LogicBroker.SaveEntity<Person>(person); //  TODO: acquire the new PersonID to put it into this collection (otherwise it is blank)
                listOfHandlers(person); //  sends Person object via Delegate to subscriber(s)
                result = true;
            }
            return result;
        }

        public T Get(int id)
        {
            return _peopleList.Where(p => p.PersonID == id)
                              .SingleOrDefault();
        }

        public void Dispose()
        {
            //  noop
        }

        /// <summary>
        /// IEnumerator T GetEnumerator() implementation by Carl.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)this;
        }

        //public IEnumerator<T> GetEnumerator()
        //{
        //    return ((IEnumerable<T>)_peopleList).GetEnumerator();
        //}

        public bool MoveNext()
        {
            position++;
            return (position < _peopleList.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        /// <summary>
        /// IEnumerator IEnumerable.GetEnumerator() implementation by Carl.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }

        /// <summary>
        /// Allow caller to index using square brackets through the collection
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return _peopleList[index];
            }
            set
            {
                _peopleList[index] = value;
                _peopleList.Sort();
            }
        }

        /// <summary>
        /// Returns PersonID by searching FirstName and LastName fields for matches. If both match, returns PersonID. If one or neither matches, return -1.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public int GetPersonIDbyName(string first, string last)
        {
            int personID = -1;
            foreach (Person p in _peopleList)
            {
                if (p.FirstName == first && p.LastName == last)
                {
                    personID = p.PersonID;
                }
            }
            return personID;
        }

        public int TotalHomesCurrentlyForSale()
        {

            return 0;
        }

        public Decimal TotalCommissionsPaid()
        {

            return 0.00m;
        }

        public Decimal TotalAmountOfSalesOfHomesSold()
        {

            return 0.00m;
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}
