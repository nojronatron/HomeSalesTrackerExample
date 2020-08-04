using HomeSalesTrackerDataLayer;
using HSTDataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;

namespace HSTDataLayer
{
    /// <summary>
    /// Class to contain all code needed to communicate between UI and DB
    /// </summary>
    public class LogicBroker
    {
        /// <summary>
        /// Method that inits a new DB and calls to load the db with XML file data
        /// </summary>
        /// <returns></returns>
        public static bool InitDatabase()
        {
            HSTContextInitializer.InitDB();
            return true;
        }

        /// <summary>
        /// Method that handles Main App calls to load db with XML file data.
        /// </summary>
        /// <returns></returns>
        public static bool LoadData()
        {
            HSTContextInitializer.LoadDataIntoDatabase();
            return true;
        }

        /// <summary>
        /// Method that handles Main App calls to shutdown (dump DB to XML Files with overwrite)
        /// </summary>
        /// <returns></returns>
        public static bool BackUpDatabase()
        {
            //  TODO: decide whether to use Bool returns on each of these calls or not
            bool peopleFlushed = 
            BackUpDataFiles.FlushPeopleTableToXml();
            if (peopleFlushed)
            {
                bool ownersFlushed =
                BackUpDataFiles.FlushOwnersTableToXml();
                if (ownersFlushed)
                {
                    bool homesFlushed =
                BackUpDataFiles.FlushHomesTableToXml();
                    if (homesFlushed)
                    {
                        bool reCompaniesFlushed =
                BackUpDataFiles.FlushRealEstateCompaniesTableToXml();
                        if (reCompaniesFlushed)
                        {
                            bool agentsFlushed =
                BackUpDataFiles.FlushAgentsTableToXml();
                            if (agentsFlushed)
                            {
                                bool buyersFlushed =
                BackUpDataFiles.FlushBuyersTableToXml();
                                if (buyersFlushed)
                                {
                                    bool homeSalesFlushed =
                BackUpDataFiles.FlushHomeSalesTableToXml();
                                    if (homeSalesFlushed)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// NOT TESTED. Should take a generic item and enable storing to the DB via EF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool SaveEntity<T>(T item)
        {
            bool result = false;
            string name = item.GetType().Name;
            using (var context = new HSTDataModel())
            {
                switch (name)
                {
                    case "Person":
                        {
                            context.People.Add(item as Person);
                            break;
                        }
                    case "Owner":
                        {
                            context.Owners.Add(item as Owner);
                            break;
                        }
                    case "Home":
                        {
                            context.Homes.Add(item as Home);
                            break;
                        }
                    case "RealEstateCompany":
                        {
                            context.Homes.Add(item as Home);
                            break;
                        }
                    case "Agent":
                        {
                            context.Homes.Add(item as Home);
                            break;
                        }
                    case "Buyer":
                        {
                            context.Homes.Add(item as Home);
                            break;
                        }
                    case "HomeSale":
                        {
                            context.Homes.Add(item as Home);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                int itemsAffected = context.SaveChanges();
                if (itemsAffected == 1)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionT"></param>
        /// <returns></returns>
        public bool UpdateEntities<T>(T[] collectionT)
        {
            using (var context = new HSTDataModel())
            {
                foreach (var item in collectionT)
                {
                    if (collectionT.GetType().Name == "Person")
                    {
                        //  TODO: Figure out how to make this work
                        //  context.People.AddOrUpdate(item as IEnumerable<Person>);
                        context.People.AddOrUpdate(item as Person);
                    }
                }
            }
            throw new NotImplementedException();
        }

    }
}
