using HSTDataLayer;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp
{
    public class PeopleCollection<T> : ICollection<T>, IEnumerable<T>, IObservable<T>
                            where T : Person
    {

        private static List<T> _peopleList = null;
        private static int position = 0;
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

        public bool IsReadOnly => ((ICollection<T>)_peopleList).IsReadOnly;

        /// <summary>
        /// Allows caller to add a Person to the Collection based on PersonID
        /// </summary>
        /// <param name="person"></param>
        public bool Add(T person)
        {
            int count = this.Count;
            bool result = false;
            var collectionPerson = _peopleList.SingleOrDefault(p => p.PersonID == person.PersonID);
            if (collectionPerson == null)
            {
                if (LogicBroker.SaveEntity<Person>(person))
                {
                    _peopleList.Add(person);
                    if (this.Count > count)
                    {
                        //  TODO: send a Person object via Delegate to subscriber(s)
                        result = true;
                    }
                }
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
            position = -1;
        }

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

        public int UpdatePerson(Person person)
        {
            int result = 0;
            if (person == null)
            {
                return 0;
            }

            int personIDX = _peopleList.FindIndex(p => p.PersonID == person.PersonID);
            Person collectionPerson = _peopleList[personIDX];
            if (person.Equals(collectionPerson))
            {
                if (LogicBroker.UpdateEntity<Person>(person))
                {
                    Person dbPerson = LogicBroker.GetPerson(collectionPerson.PersonID);
                    this[personIDX] = (T)dbPerson;
                    result = 1;
                }
            }
            else
            {
                if (LogicBroker.SaveEntity<Person>(person))
                {
                    Person dbPerson = LogicBroker.GetPerson(collectionPerson.PersonID);
                    T newDbPerson = (T)dbPerson;
                    _peopleList.Add(newDbPerson);
                    result = 1;
                }
            }

            return result;
        }

        public int UpdateAgent(Agent agent)
        {

            return 0;
        }

        public int UpdateBuyer(Buyer buyer)
        {

            return 0;
        }

        public int UpdateOwner(Owner owner)
        {

            return 0;
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            ((ICollection<T>)_peopleList).Add(item);
        }

        public void Clear()
        {
            ((ICollection<T>)_peopleList).Clear();
        }

        public bool Contains(T item)
        {
            return ((ICollection<T>)_peopleList).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)_peopleList).CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return ((ICollection<T>)_peopleList).Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_peopleList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)_peopleList).GetEnumerator();
        }
    }
}
