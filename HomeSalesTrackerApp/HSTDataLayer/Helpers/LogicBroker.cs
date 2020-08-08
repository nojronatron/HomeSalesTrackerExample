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
                            Person person = item as Person;
                            Person updatePerson = new Person()
                            {
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Phone = person.Phone,
                                Email = person.Email ?? null
                            };

                            context.People.AddOrUpdate(x => new { x.FirstName, x.LastName }, updatePerson);
                            //context.People.Add(item as Person);
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
                            Agent agent = item as Agent;
                            Agent updateAgent = new Agent()
                            {
                                CommissionPercent = agent.CommissionPercent,
                                CompanyID = agent.CompanyID ?? null
                            };

                            context.Agents.AddOrUpdate(a => new { a.AgentID }, updateAgent);
                            //context.Agents.Add(item as Agent);
                            break;
                        }
                    case "Buyer":
                        {
                            context.Buyers.Add(item as Buyer);
                            break;
                        }
                    case "HomeSale":
                        {
                            HomeSale homesale = item as HomeSale;
                            HomeSale updateHomesale = new HomeSale()
                            {
                                HomeID = homesale.HomeID,
                                SoldDate = homesale.SoldDate ?? null,
                                AgentID = homesale.AgentID,
                                SaleAmount = homesale.SaleAmount,
                                BuyerID = homesale.BuyerID ?? null,
                                MarketDate = homesale.MarketDate,
                                CompanyID = homesale.CompanyID,
                                Agent = homesale.Agent
                            };

                            context.HomeSales.AddOrUpdate(hs => new { hs.HomeID }, updateHomesale);
                            //context.HomeSales.Add(item as HomeSale);
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
                            int personID = (item as Person).PersonID;
                            Person personToDelete = context.People.Find(personID);
                            context.People.Remove(personToDelete);
                            break;
                        }
                    case "Owner":
                        {
                            int ownerID = (item as Owner).OwnerID;
                            Owner ownerToDelete = context.Owners.Find(ownerID);
                            context.Owners.Remove(ownerToDelete);
                            break;
                        }
                    case "Home":
                        {
                            int homeID = (item as Home).HomeID;
                            Home homeToDelete = context.Homes.Find(homeID);
                            context.Homes.Remove(homeToDelete);
                            break;
                        }
                    case "RealEstateCompany":
                        {
                            int recoID = (item as RealEstateCompany).CompanyID;
                            RealEstateCompany recoToDelete = context.RealEstateCompanies.Find(recoID);
                            context.RealEstateCompanies.Remove(recoToDelete);
                            break;
                        }
                    case "Agent":
                        {
                            int agentID = (item as Agent).AgentID;
                            Agent agentToDelete = context.Agents.Find(agentID);
                            context.Agents.Remove(agentToDelete);
                            break;
                        }
                    case "Buyer":
                        {
                            int buyerID = (item as Buyer).BuyerID;
                            Buyer buyerToDelete = context.Buyers.Find(buyerID);
                            context.Buyers.Remove(buyerToDelete);
                            break;
                        }
                    case "HomeSale":
                        {
                            int saleID = (item as HomeSale).SaleID;
                            HomeSale homeSaleToDelete = context.HomeSales.Find(saleID);
                            context.HomeSales.Remove(homeSaleToDelete);
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
        /// NOT TESTED. Should take a generic item and enable updating a field in an existing entry via EF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool UpdateEntity<T>(T item)
        {
            bool result = false;
            string name = item.GetType().Name;
            using (var context = new HSTDataModel())
            {
                switch (name)
                {
                    case "Person":
                        {
                            //  IComparable<Person> comparison uses FirstName and LastName fields
                            Person person = item as Person;
                            Person personToUpdate = context.People.Find(person.PersonID);
                            if (personToUpdate != null && personToUpdate.Equals(person))
                            {
                                personToUpdate.Phone = person.Phone;
                                personToUpdate.Email = person.Email ?? null;
                            }

                            break;
                        }
                    case "Owner":
                        {
                            //  Only comparable item is OwnerID
                            Owner owner = item as Owner;
                            Owner ownerToUpdate = context.Owners.Find(owner.OwnerID);
                            if (ownerToUpdate != null)
                            {
                                ownerToUpdate.PreferredLender = owner.PreferredLender;
                            }

                            break;
                        }
                    case "Home":
                        {
                            //  IComparable<Home> comparison uses Address and Zip fields
                            Home home = item as Home;
                            Home homeToUpdate = context.Homes.Find(home.HomeID);
                            if(homeToUpdate != null && homeToUpdate.Equals(home))
                            {
                                homeToUpdate.City = home.City;
                                homeToUpdate.State = home.State;
                                homeToUpdate.OwnerID = home.OwnerID;
                            };

                            break;
                        }
                    case "RealEstateCompany":
                        {
                            //  IComparable<RealEstateCompany> exists but stick with CompanyID instead
                            RealEstateCompany reco = item as RealEstateCompany;
                            RealEstateCompany recoToUpdate = context.RealEstateCompanies.Find(reco.CompanyID);
                            if(recoToUpdate != null)
                            {
                                recoToUpdate.CompanyName = reco.CompanyName;
                                recoToUpdate.Phone = reco.Phone;
                            }

                            break;
                        }
                    case "Agent":
                        {
                            //  IComparable<Agent> uses AgentID for comparison
                            Agent agent = item as Agent;
                            Agent agentToUpdate = context.Agents.Find(agent.AgentID);
                            if(agentToUpdate != null)
                            {
                                agentToUpdate.CompanyID = agent.CompanyID;
                                agentToUpdate.CommissionPercent = agent.CommissionPercent;
                            }

                            break;
                        }
                    case "Buyer":
                        {
                            //  IComparable<Buyer> not implemented
                            Buyer buyer = item as Buyer;
                            Buyer buyerToUpdate = context.Buyers.Find(buyer.BuyerID);
                            if(buyerToUpdate != null)
                            {
                                buyerToUpdate.CreditRating = buyer.CreditRating;
                            }

                            break;
                        }
                    case "HomeSale":
                        {
                            //  ICOmparable<HomeSale> implemented but will not use it here
                            HomeSale homesale = item as HomeSale;
                            HomeSale homesaleToUpdate = context.HomeSales.Find(homesale.SaleID);
                            if(homesaleToUpdate != null)
                            {
                                homesaleToUpdate.HomeID = homesale.HomeID;
                                homesaleToUpdate.SoldDate = homesale.SoldDate ?? null;
                                homesaleToUpdate.AgentID = homesale.AgentID;
                                homesaleToUpdate.SaleAmount = homesale.SaleAmount;
                                homesaleToUpdate.BuyerID = homesale.BuyerID ?? null;
                                homesaleToUpdate.MarketDate = homesale.MarketDate;
                                homesaleToUpdate.CompanyID = homesale.CompanyID;
                            }

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
