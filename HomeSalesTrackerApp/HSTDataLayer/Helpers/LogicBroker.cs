using HSTDataLayer.Helpers;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace HSTDataLayer
{
    /// <summary>
    /// Contain all code needed to communicate between UI and DB
    /// </summary>
    public class LogicBroker
    {
        /// <summary>
        /// Takes a Caller's Person type arg and returns the found Person Entity from DB, otherise returns null Person Type.
        /// </summary>
        /// <typeparam name="Person"></typeparam>
        /// <param name="person"></param>
        /// <returns></returns>
        public static Person GetPerson(int personID)
        {
            Person result = null;
            if (personID < 0)
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.People
                    .Where(p => p.PersonID == personID)
                    .Include(a => a.Agent)
                    .Include(b => b.Buyer)
                    .Include(o => o.Owner)
                    .FirstOrDefault();//Find(personID);
            }

            return result;
        }

        public static Person GetPerson(string firstName, string lastName)
        {
            Person result = null;
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.People
                    .Where(p =>
                        p.FirstName == firstName && p.LastName == lastName)
                    .Include(a => a.Agent)
                    .Include(b => b.Buyer)
                    .Include(o => o.Owner)
                    .FirstOrDefault();
            }

            return result;
        }

        public static Home GetHome(int homeID)
        {
            Home result = null;
            if (homeID < 0)
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.Homes
                    .Where(h => h.HomeID == homeID)
                    .Include(hs => hs.HomeSales)
                    .Include(o => o.Owner)
                    .FirstOrDefault();//Find(homeID);
            }

            return result;
        }

        public static Home GetHome(string address, string zip)
        {
            Home result = null;
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(zip))
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.Homes
                    .Where(h =>
                        h.Address == address && h.Zip == zip)
                    .Include(hs => hs.HomeSales)
                    .Include(o => o.Owner)
                    .FirstOrDefault();
            }

            return result;
        }

        public static HomeSale GetHomeSale(int homesaleID)
        {
            HomeSale result = null;
            if (homesaleID < 0)
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.HomeSales
                    .Where(hs =>
                        hs.SaleID == homesaleID)
                    .Include(hs => hs.Home)
                    .Include(hs => hs.RealEstateCompany)
                    .Include(hs => hs.Agent)
                    .Include(hs => hs.Buyer)
                    .FirstOrDefault();
            }

            return result;
        }

        public static HomeSale GetHomeSale(DateTime marketDate, decimal saleAmount)
        {
            HomeSale result = null;
            if (marketDate != null && saleAmount > 0m)
            {
                using (var context = new HSTDataModel())
                {
                    result = context.HomeSales
                        .Where(hs =>
                            hs.MarketDate == marketDate && hs.SaleAmount == saleAmount)
                        .Include(hs => hs.Home)
                        .Include(hs => hs.RealEstateCompany)
                        .Include(hs => hs.Agent)
                        .Include(hs => hs.Buyer)
                        .FirstOrDefault();
                }
            }

            return result;
        }

        public static RealEstateCompany GetReCompany(int companyID)
        {
            RealEstateCompany result = null;
            if (companyID < 0)
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.RealEstateCompanies
                    .Where(re => re.CompanyID == companyID)
                    .Include(a => a.Agents)
                    .Include(hs => hs.HomeSales)
                    .FirstOrDefault();//Find(companyID);
            }

            return result;
        }

        public static RealEstateCompany GetReCompany(string companyName)
        {
            RealEstateCompany result = null;
            if (string.IsNullOrEmpty(companyName))
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.RealEstateCompanies
                    .Where(re => re.CompanyName == companyName)
                    .Include(a => a.Agents)
                    .Include(hs => hs.HomeSales)
                    .FirstOrDefault();
            }

            return result;
        }

        public static Agent GetAgent(int agentID)
        {
            Agent result = null;
            if (agentID < 0)
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.Agents
                    .Where(a => a.AgentID == agentID)
                    .Include(hs => hs.HomeSales)
                    .Include(p => p.Person)
                    .Include(re => re.RealEstateCompany)
                    .FirstOrDefault();//Find(agentID);
            }

            return result;
        }
        
        public static Buyer GetBuyer(int buyerID)
        {
            Buyer result = null;
            if (buyerID < 0)
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.Buyers
                    .Where(b => b.BuyerID == buyerID)
                    .Include(hs => hs.HomeSales)
                    .Include(p => p.Person)
                    .FirstOrDefault();//Find(buyerID);
            }

            return result;
        }

        public static Owner GetOwner(int ownerID)
        {
            Owner result = null;
            if (ownerID < 0)
            {
                return result;
            }

            using (var context = new HSTDataModel())
            {
                result = context.Owners
                    .Where(o => o.OwnerID == ownerID)
                    .Include(h => h.Homes)
                    .Include(p => p.Person)
                    .FirstOrDefault();//Find(ownerID);
            }

            return result;
        }

        /// <summary>
        /// Inits a new DB and calls to load the db with XML file data
        /// </summary>
        /// <returns></returns>
        public static bool InitDatabase()
        {
            HSTContextInitializer.InitDB();
            return true;
        }

        /// <summary>
        /// Load DB with XML file data.
        /// </summary>
        /// <returns></returns>
        public static bool LoadData()
        {
            HSTContextInitializer.LoadDataIntoDatabase();
            return true;
        }

        /// <summary>
        /// Dump Database Tables to XML Files with overwrite enabled.
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
        /// Takes a fundamental HomeSaleTracker type and attempts to add or update the database.
        /// Returns 1 if something was saved or updated, 0 otherwise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool StoreItem<T>(T item)
        {
            bool result = false;
            int itemsAffected = 0;
            string name = item.GetType().Name;
            using (var context = new HSTDataModel())
            {
                switch (name)
                {
                    case "Person":
                        {
                            Person person = item as Person;
                            context.People.AddOrUpdate(p => new { p.FirstName, p.LastName }, person);
                            break;
                        }
                    case "Agent":
                        {
                            Agent agent = item as Agent;
                            context.Agents.AddOrUpdate(a => new { a.AgentID }, agent);
                            break;
                        }
                    case "Buyer":
                        {
                            Buyer buyer = item as Buyer;
                            context.Buyers.AddOrUpdate(b => new { b.BuyerID }, buyer);
                            break;
                        }
                    case "Owner":
                        {
                            Owner owner = item as Owner;
                            context.Owners.AddOrUpdate(o => new { o.OwnerID }, owner);
                            break;
                        }
                    case "Home":
                        {
                            Home home = item as Home;
                            context.Homes.AddOrUpdate(h => new { h.Address, h.Zip }, home);
                            break;
                        }
                    case "HomeSale":
                        {
                            HomeSale homeSale = item as HomeSale;
                            context.HomeSales.AddOrUpdate(hs => new { hs.SaleID }, homeSale);
                            break;
                        }
                    case "RealEstateCompany":
                        {
                            RealEstateCompany reco = item as RealEstateCompany;
                            context.RealEstateCompanies.AddOrUpdate(re => new { re.CompanyID }, reco);
                            break;
                        }
                    default:
                        {
                            itemsAffected = -1;
                            break;
                        }
                }

                itemsAffected = context.SaveChanges();

                if (itemsAffected > 0)
                {
                    result = true;
                }

                if (itemsAffected < 1)
                {
                    result = false;
                }

            }

            return result;
        }

        /// <summary>
        /// Take a generic item (EXCEPT HomeSale) and enable updating the existing item in the store with properties of the item passed-in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool UpdateExistingItem<T>(T item)
        {
            bool result = false;
            int itemsAffected = 0;
            string name = item.GetType().Name;
            using (var context = new HSTDataModel())
            {
                switch (name)
                {
                    case "Person":
                        {
                            Person person = item as Person;
                            Person dbPerson = context.People.Find(person.PersonID);
                            
                            if (dbPerson == null)
                            {
                                break;
                            }
                            
                            if (dbPerson.Equals(person))
                            {
                                dbPerson.Email = person.Email;
                                dbPerson.Phone = person.Phone;
                            }
                            
                            break;
                        }
                    case "Owner":
                        {
                            Owner owner = item as Owner;
                            Owner dbOwner = context.Owners.Find(owner.OwnerID);
                            
                            if (dbOwner == null)
                            {
                                break;
                            }

                            if (dbOwner.Homes.Count != owner.Homes.Count && owner.Homes.Count > 0)
                            {
                                dbOwner.Homes = owner.Homes;
                            }

                            dbOwner.PreferredLender = owner.PreferredLender;
                            break;
                        }
                    case "Home":
                        {
                            Home home = item as Home;
                            Home dbHome = context.Homes.Find(home.HomeID);

                            if (dbHome == null)
                            {
                                break;
                            }

                            if (dbHome.HomeSales.Count != home.HomeSales.Count && home.HomeSales.Count > 0)
                            {
                                dbHome.HomeSales = home.HomeSales;
                            }

                            if (dbHome.Owner != null && dbHome.Owner != home.Owner)
                            {
                                dbHome.Owner = home.Owner;
                            }

                            break;
                        }
                    case "RealEstateCompany":
                        {
                            RealEstateCompany reco = item as RealEstateCompany;
                            RealEstateCompany dbReco = context.RealEstateCompanies.Find(reco.CompanyID);

                            if (dbReco == null)
                            {
                                break;
                            }

                            if (dbReco.Phone != reco.Phone)
                            {
                                dbReco.Phone = reco.Phone;
                            }
                            
                            if (dbReco.HomeSales.Count != reco.HomeSales.Count && reco.HomeSales.Count > 0)
                            {
                                dbReco.HomeSales = reco.HomeSales;
                            }

                            if (dbReco.Agents.Count != reco.Agents.Count)
                            {
                                dbReco.Agents = reco.Agents;
                            }

                            break;
                        }
                    case "Agent":
                        {
                            Agent agent = item as Agent;
                            Agent dbAgent = context.Agents.Find(agent.AgentID);

                            if (dbAgent == null)
                            {
                                break;
                            }

                            if (dbAgent.HomeSales.Count != agent.HomeSales.Count && agent.HomeSales.Count > 0)
                            {
                                dbAgent.HomeSales = agent.HomeSales;
                            }

                            dbAgent.CommissionPercent = agent.CommissionPercent;
                            dbAgent.CompanyID = agent.CompanyID;

                            break;
                        }
                    case "Buyer":
                        {
                            Buyer buyer = item as Buyer;
                            Buyer dbBuyer = context.Buyers.Find(buyer.BuyerID);

                            if (dbBuyer == null)
                            {
                                break;
                            }

                            if (dbBuyer.HomeSales.Count != buyer.HomeSales.Count && buyer.HomeSales.Count > 0)
                            {
                                dbBuyer.HomeSales = buyer.HomeSales;
                            }

                            dbBuyer.CreditRating = buyer.CreditRating;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                try
                {
                    itemsAffected = context.SaveChanges();
                }
                catch
                {
                    _ = 0;
                }

                if (itemsAffected > 0)
                {
                    result = true;
                }

                if (itemsAffected < 0)
                {
                    result = false;
                }

            }
            return result;
        }

        /// <summary>
        /// Take a generic item and enable removing from the DB via EF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool RemoveEntity<T>(T item)
        {
            bool result = false;
            int itemsAffected = 0;
            string name = item.GetType().Name;
            using (var context = new HSTDataModel())
            {
                switch (name)
                {
                    case "Person":
                        {
                            Person person = (item as Person);
                            Person personToDelete = context.People.Find(person.PersonID);
                            
                            if (personToDelete != null)
                            {
                                context.People.Remove(personToDelete);
                            }

                            break;
                        }
                    case "Owner":
                        {
                            Owner owner = (item as Owner);
                            Owner ownerToDelete = context.Owners.Find(owner.OwnerID);
                            
                            if (ownerToDelete != null)
                            {
                                context.Owners.Remove(ownerToDelete);
                            }

                            break;
                        }
                    case "Home":
                        {
                            Home home = (item as Home);
                            Home homeToDelete = context.Homes.Find(home.HomeID);

                            if (homeToDelete != null)
                            {
                                context.Homes.Remove(homeToDelete);
                            }

                            break;
                        }
                    case "RealEstateCompany":
                        {
                            RealEstateCompany reco = (item as RealEstateCompany);
                            RealEstateCompany recoToDelete = context.RealEstateCompanies.Find(reco.CompanyID);

                            if (recoToDelete != null)
                            {
                                context.RealEstateCompanies.Remove(recoToDelete);
                            }

                            break;
                        }
                    case "Agent":
                        {
                            Agent agent = (item as Agent);
                            Agent agentToDelete = context.Agents.Find(agent.AgentID);

                            if (agentToDelete != null)
                            {
                                context.Agents.Remove(agentToDelete);
                            }

                            break;
                        }
                    case "Buyer":
                        {
                            Buyer buyer = (item as Buyer);
                            Buyer buyerToDelete = context.Buyers.Find(buyer.BuyerID);
                            
                            if (buyerToDelete != null)
                            {
                                context.Buyers.Remove(buyerToDelete);
                            }

                            break;
                        }
                    case "HomeSale":
                        {
                            HomeSale homeSale = (item as HomeSale);
                            HomeSale homeSaleToDelete = context.HomeSales.Find(homeSale.SaleID);

                            if (homeSaleToDelete != null)
                            {
                                context.HomeSales.Remove(homeSaleToDelete);
                            }

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                try
                {
                    itemsAffected = context.SaveChanges();
                }
                catch
                {
                    _ = 0;
                }

                if (itemsAffected > 0)
                {
                    result = true;
                }

                if (itemsAffected < 0)
                {
                    result = false;
                }
            }

            return result;
        }

    }
}
