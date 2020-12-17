using HSTDataLayer;
using HSTDataLayer.Helpers;

using System;

namespace HomeSalesTrackerApp.Factory
{
    class CollectionFactory
    {
        public static HomesCollection GetHomesCollectionObject()
        {
            return new HomesCollection(EntityLists.GetTreeListOfHomes());
        }
        public static HomeSalesCollection GetHomeSalesCollectionObject()
        {
            return new HomeSalesCollection(EntityLists.GetListOfHomeSales());
        }
        public static RealEstateCosCollection GetRECosCollectionObject()
        {
            return new RealEstateCosCollection(EntityLists.GetTreeListOfRECompanies());
        }
        public static PeopleCollection<Person> GetPeopleCollectionObject()
        {
            return new PeopleCollection<Person>(EntityLists.GetListOfPeople());
        }
    }
}
