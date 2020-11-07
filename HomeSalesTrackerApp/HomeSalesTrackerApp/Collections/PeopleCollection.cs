﻿using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp
{
    public class PeopleCollection<T> : ICollection<T>, IEnumerable<T>
                            where T : Person
    {

        private static List<T> _peopleList = null;
        private static int position = 0;
        public int Count => _peopleList.Count;

        public CollectionMonitor collectionMonitor = null;

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
            collectionMonitor = new CollectionMonitor();
        }

        public T Current => _peopleList[position];

        public bool IsReadOnly => ((ICollection<T>)_peopleList).IsReadOnly;

        /// <summary>
        /// Allows caller to add a Person to the Collection based on PersonID
        /// </summary>
        /// <param name="person"></param>
        public int Add(T person)
        {
            if (person != null)
            {
                int preCount = this.Count;
                var collectionPerson = _peopleList.SingleOrDefault(p => p.FirstName == person.FirstName &&
                                                                        p.LastName == person.LastName);
                if (collectionPerson == null)
                {
                    if (LogicBroker.StoreItem<Person>(person))
                    {
                        _peopleList.Add((T)person);

                        if (this.Count > preCount)
                        {
                            collectionMonitor.SendNotifications(1, "Person");
                            return 1;
                        }
                        //}
                    }
                }
            }

            return 0;
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

        public int UpdatePerson(T person)
        {
            if (person == null)
            {
                return 0;
            }

            int result = 0;
            int personIDX = _peopleList.FindIndex(p => p.PersonID == person.PersonID);
            Person collectionPerson = _peopleList[personIDX];

            if (collectionPerson != null)
            {
                if (LogicBroker.UpdateExistingItem<Person>(person))
                {
                    this[personIDX] = (T)person;
                    result = 1;
                }
            
            }
            
            if (collectionPerson == null)
            {
                result = this.Add(person);
            }

            result += UpdateAgent(person.Agent);
            result += UpdateBuyer(person.Buyer);
            result += UpdateOwner(person.Owner);

            if (result > 0)
            {
                collectionMonitor.SendNotifications(1, "Person");
            }

            return result;
        }

        internal int UpdateAgent(Agent agent)
        {
            if (agent == null || agent.AgentID < 0)
            {
                return 0;
            }

            Agent dbAgent = null;
            int personIDX = _peopleList.FindIndex(p => p.PersonID == agent.AgentID);
            Person collectionPerson = _peopleList[personIDX];

            if (collectionPerson.Agent == agent)
            {
                return 0;
            }

            int result = 0;
            bool agentStored = false;
            dbAgent = LogicBroker.GetAgent(collectionPerson.PersonID);

            if (dbAgent == null)
            {
                agentStored = LogicBroker.StoreItem<Agent>(agent);
            }
            else
            {
                agentStored = LogicBroker.UpdateExistingItem<Agent>(agent);
            }

            if (agentStored)
            {
                dbAgent = LogicBroker.GetAgent(collectionPerson.PersonID);
                this[personIDX].Agent = dbAgent;
                result = 1;
                collectionMonitor.SendNotifications(1, "Person");
                collectionMonitor.SendNotifications(1, "Agent");
            }

            return result;
        }

        internal int UpdateBuyer(Buyer buyer)
        {
            if (buyer == null || buyer.BuyerID < 0)
            {
                return 0;
            }

            Buyer dbBuyer = null;
            int personIDX = _peopleList.FindIndex(p => p.PersonID == buyer.BuyerID);
            Person collectionPerson = _peopleList[personIDX];

            if (collectionPerson.Buyer == buyer)
            {
                return 0;
            }

            int result = 0;
            bool buyerStored = false;
            dbBuyer = LogicBroker.GetBuyer(collectionPerson.PersonID);

            if (dbBuyer == null)
            {
                buyerStored = LogicBroker.StoreItem<Buyer>(buyer);
            }
            else
            {
                buyerStored = LogicBroker.UpdateExistingItem<Buyer>(buyer);
            }

            if (buyerStored)
            {
                dbBuyer = LogicBroker.GetBuyer(collectionPerson.PersonID);
                this[personIDX].Buyer = dbBuyer;
                result = 1;
                collectionMonitor.SendNotifications(1, "Person");
                collectionMonitor.SendNotifications(1, "Buyer");
            }

            return result;
        }

        internal int UpdateOwner(Owner owner)
        {
            if (owner == null || owner.OwnerID < 0)
            {
                return 0;
            }

            Owner dbOwner = null;
            int personIDX = _peopleList.FindIndex(p => p.PersonID == owner.OwnerID);
            Person collectionPerson = _peopleList[personIDX];

            if (collectionPerson.Owner == owner)
            {
                return 0;
            }

            int result = 0;
            bool ownerStored = false;
            dbOwner = LogicBroker.GetOwner(collectionPerson.PersonID);

            if (dbOwner == null)
            {
                ownerStored = LogicBroker.StoreItem<Owner>(owner);
            }
            else
            {
                ownerStored = LogicBroker.UpdateExistingItem<Owner>(owner);
            }

            if (ownerStored)
            {
                dbOwner = LogicBroker.GetOwner(collectionPerson.PersonID);
                this[personIDX].Owner = dbOwner;
                result = 1;
                collectionMonitor.SendNotifications(1, "Person");
                collectionMonitor.SendNotifications(1, "Owner");
            }

            return result;
        }

        void ICollection<T>.Add(T item)
        {
            ((ICollection<T>)_peopleList).Add(item);
            collectionMonitor.SendNotifications(1, "Person");
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
