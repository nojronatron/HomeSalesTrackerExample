using HSTDataLayer;
using HSTDataLayer.Helpers;

using System.Collections.Generic;

namespace HomeSalesTrackerApp.Helpers
{
    public class CollectionsManager
    {
        private Logger logger = null;
        public static RealEstateCosCollection reCosCollection = null;
        public static HomesCollection homesCollection = null;
        public static PeopleCollection<Person> peopleCollection = null;
        public static HomeSalesCollection homeSalesCollection = null;

        public CollectionsManager()
        {
            logger = new Logger();
            reCosCollection = new RealEstateCosCollection();
            homesCollection = new HomesCollection();
            peopleCollection = new PeopleCollection<Person>();
            homeSalesCollection = new HomeSalesCollection();
            logger.Data("CollectionsManager", "CTOR initialize executed");
            logger.Flush();
        }

        public bool InitializeCollections()
        {
            bool result = true;
            try
            {
                InitHomesCollection();
                InitHomeSalesCollection();
                InitPeopleCollection();
                InitRealEstateCompaniesCollection();
            }
            catch
            {
                logger.Data("InitializeCollections", "Failed.");
                logger.Flush();
                result = false;
            }
            return result;
        }

        public static void InitRealEstateCompaniesCollection()
        {
            reCosCollection = new RealEstateCosCollection();
            List<RealEstateCompany> recos = EntityLists.GetTreeListOfRECompanies();
            foreach (var reco in recos)
            {
                reCosCollection.Add(reco);
            }
        }

        public static void InitHomesCollection()
        {
            homesCollection = new HomesCollection();
            List<Home> homes = EntityLists.GetTreeListOfHomes();
            foreach (var home in homes)
            {
                homesCollection.Add(home);
            }
        }

        public static void InitPeopleCollection()
        {
            peopleCollection = new PeopleCollection<Person>();
            List<Person> people = EntityLists.GetListOfPeople();
            foreach (var person in people)
            {
                peopleCollection.Add(person);
            }
        }

        public static void InitHomeSalesCollection()
        {
            homeSalesCollection = new HomeSalesCollection();
            List<HomeSale> homeSales = EntityLists.GetListOfHomeSales();
            foreach (var homeSale in homeSales)
            {
                homeSalesCollection.Add(homeSale);
            }
        }

    }
}
