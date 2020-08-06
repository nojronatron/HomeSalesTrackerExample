using HomeSalesTrackerDataLayer;
using HSTDataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Runtime.Remoting.Contexts;

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
                            context.RealEstateCompanies.Add(item as RealEstateCompany);
                            break;
                        }
                    case "Agent":
                        {
                            context.Agents.Add(item as Agent);
                            break;
                        }
                    case "Buyer":
                        {
                            context.Buyers.Add(item as Buyer);
                            break;
                        }
                    case "HomeSale":
                        {
                            context.HomeSales.Add(item as HomeSale);
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
        /// NOT TESTED. Should take a generic item and enable removing from the DB via EF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool RemoveEntity<T>(T item)
        {
            bool result = false;
            string name = item.GetType().Name;
            using (var context = new HSTDataModel())
            {
                switch (name)
                {
                    case "Person":
                        {
                            context.People.Remove(item as Person);
                            break;
                        }
                    case "Owner":
                        {
                            context.Owners.Remove(item as Owner);
                            break;
                        }
                    case "Home":
                        {
                            context.Homes.Remove(item as Home);
                            break;
                        }
                    case "RealEstateCompany":
                        {
                            context.RealEstateCompanies.Remove(item as RealEstateCompany);
                            break;
                        }
                    case "Agent":
                        {
                            context.Agents.Remove(item as Agent);
                            break;
                        }
                    case "Buyer":
                        {
                            context.Buyers.Remove(item as Buyer);
                            break;
                        }
                    case "HomeSale":
                        {
                            context.HomeSales.Remove(item as HomeSale);
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

    }
}
