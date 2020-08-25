using HSTDataLayer.Helpers;
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
        /// Take a generic item and enable storing to the DB via EF if different than existing item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool SaveEntity<T>(T item)
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
                            context.People.AddOrUpdate(x => new { x.FirstName, x.LastName }, person);
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
                    case "RealEstateCompany":
                        {
                            RealEstateCompany reco = item as RealEstateCompany;
                            context.RealEstateCompanies.AddOrUpdate(re => new { re.CompanyID }, reco);
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
                    case "HomeSale":
                        {
                            HomeSale homesale = item as HomeSale;
                            context.HomeSales.AddOrUpdate(hs => new { hs.HomeID, hs.SaleAmount, hs.MarketDate }, homesale);
                            break;
                        }
                    default:
                        {
                            itemsAffected = -1;
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
                            homeSaleToDelete.Agent = null;
                            homeSaleToDelete.Buyer = null;
                            homeSaleToDelete.Home = null;
                            homeSaleToDelete.RealEstateCompany = null;
                            context.HomeSales.Remove(homeSaleToDelete);
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
        /// Take a generic item and enable updating a field in an existing entry via EF. If no existing Entities match, new Entity is created.
        /// People Typed Entities must contain a new or existing Person Type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool UpdateEntity<T>(T item)
        {
            bool result = false;
            int itemsAffected = 0;
            string name = item.GetType().Name;
            using (var context = new HSTDataModel())
            {
                var agent = new Agent();
                var buyer = new Buyer();
                var owner = new Owner();
                var home = new Home();
                var homeSale = new HomeSale();
                var person = new Person();
                var realEstateCompany = new RealEstateCompany();

                switch (name)
                {
                    case "Agent":
                        {
                            agent = item as Agent;
                            Agent agentToUpdate = context.Agents.Find(agent.AgentID);
                            if (agentToUpdate != null)
                            {
                                agentToUpdate.CompanyID = agent.CompanyID;
                                agentToUpdate.CommissionPercent = agent.CommissionPercent;
                            }
                            else
                            {
                                agentToUpdate = new Agent()
                                {
                                    CompanyID = agent.CompanyID,
                                    CommissionPercent = agent.CommissionPercent,
                                    AgentID = agent.Person.PersonID
                                };
                                context.Agents.Add(agentToUpdate);
                                //person = context.People.Find(agent.AgentID);
                                //if (person != null)
                                //{
                                //    agent.Person = person;
                                //    person.Agent = agent;
                                //}
                                //else
                                //{
                                //    context.Agents.Add(agent);
                                //}
                            }

                            break;
                        }
                    case "Buyer":
                        {
                            buyer = item as Buyer;
                            Buyer buyerToUpdate = context.Buyers.Find(buyer.BuyerID);
                            if (buyerToUpdate != null)
                            {
                                buyerToUpdate.CreditRating = buyer.CreditRating;
                            }
                            else
                            {
                                person = context.People.Find(buyer.BuyerID);
                                if (person != null)
                                {
                                    buyer.Person = person;
                                    person.Buyer = buyer;
                                }
                                else
                                {
                                    context.Buyers.Add(buyer);
                                }
                            }

                            break;
                        }
                    case "Owner":
                        {
                            owner = item as Owner;
                            Owner ownerToUpdate = context.Owners.Find(owner.OwnerID);
                            if (ownerToUpdate != null)
                            {
                                ownerToUpdate.PreferredLender = owner.PreferredLender;
                            }
                            else
                            {
                                person = context.People.Find(owner.OwnerID);
                                if (person != null)
                                {
                                    owner.Person = person;
                                    person.Owner = owner;
                                }
                                else
                                {
                                    context.Owners.Add(owner);
                                }
                            }

                            break;
                        }
                    case "Person":
                        {
                            person = item as Person;
                            Person personToUpdate = (from p in context.People
                                                     where p.FirstName == person.FirstName &&
                                                     p.LastName == person.LastName
                                                     select p).FirstOrDefault();

                            if (personToUpdate == null)
                            {
                                context.People.Add(person);
                            }
                            else
                            {
                                personToUpdate.Phone = person.Phone;
                                personToUpdate.Email = person.Email ?? null;
                            }

                            break;
                        }
                    case "Home":
                        {
                            home = item as Home;

                            Home homeToUpdate = (from h in context.Homes
                                                 where h.Address == home.Address &&
                                                 h.Zip == home.Zip
                                                 select h).FirstOrDefault();

                            if (homeToUpdate == null)
                            {
                                context.Homes.Add(home);
                            }
                            else
                            {
                                homeToUpdate.Address = home.Address;
                                homeToUpdate.City = home.City;
                                homeToUpdate.State = home.State;
                                homeToUpdate.Zip = home.Zip;
                                homeToUpdate.Owner = home.Owner;
                            }
                            
                            break;
                        }
                    case "HomeSale":
                        {
                            homeSale = item as HomeSale;
                            HomeSale homesaleToUpdate = context.HomeSales.Find(homeSale.SaleID);
                            if (homesaleToUpdate != null)
                            {
                                homesaleToUpdate.HomeID = homeSale.HomeID;
                                homesaleToUpdate.SoldDate = homeSale.SoldDate ?? null;
                                homesaleToUpdate.AgentID = homeSale.AgentID;
                                homesaleToUpdate.SaleAmount = homeSale.SaleAmount;
                                homesaleToUpdate.BuyerID = homeSale.BuyerID ?? null;
                                homesaleToUpdate.MarketDate = homeSale.MarketDate;
                                homesaleToUpdate.CompanyID = homeSale.CompanyID;
                            }

                            break;
                        }
                    case "RealEstateCompany":
                        {
                            realEstateCompany = item as RealEstateCompany;
                            RealEstateCompany recoToUpdate = context.RealEstateCompanies.Find(realEstateCompany.CompanyID);
                            if(recoToUpdate != null)
                            {
                                recoToUpdate.CompanyName = realEstateCompany.CompanyName;
                                recoToUpdate.Phone = realEstateCompany.Phone;
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
