using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace HSTDataLayer.Helpers
{
    public class EntityLists
    {
        public static List<Person> GetTreeListOfPeople()
        {
            List<Person> peopleList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                peopleList = new List<Person>();
                foreach (Person person in context.People.Include(a => a.Agent)
                                                        .Include(r => r.Agent.RealEstateCompany)
                                                        .Include(hs => hs.Agent.HomeSales)
                                                        .Include(o => o.Owner)
                                                        .Include(b => b.Buyer))
                {
                    peopleList.Add(person);
                }
            }
            return peopleList;
        }

        public static List<HomeSale> GetTreeListOfHomeSales()
        {
            List<HomeSale> homeSalesList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                homeSalesList = new List<HomeSale>();
                foreach (HomeSale homesale in context.HomeSales.Include(hs => hs.Agent)
                                                               .Include(a => a.RealEstateCompany)
                                                               .Include(b => b.Buyer)
                                                               .Include(h => h.Home))
                {
                    homeSalesList.Add(homesale);
                }
            }
            return homeSalesList;
        }

        public static List<Home> GetTreeListOfHomes()
        {
            List<Home> homesList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                homesList = new List<Home>();
                foreach (Home home in context.Homes.Include(hs => hs.HomeSales))
                {
                    homesList.Add(home);
                }
            }
            return homesList;
        }

        public static List<RealEstateCompany> GetTreeListOfRECompanies()
        {
            var recosList = default(List<RealEstateCompany>);   //  allows using var, forces null if nullable type
            using (var context = new HSTDataModel())
            {
                recosList = new List<RealEstateCompany>();
                foreach (var reco in context.RealEstateCompanies.Include(a => a.Agents)
                                                                              .Include(hs => hs.HomeSales))
                {
                    recosList.Add(reco);
                }
            }
            return recosList;
        }

        public static List<Owner> GetListOfOwners()
        {
            List<Owner> ownerList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                var owners = (context.Owners.OrderBy(o => o.OwnerID)).ToList();
                ownerList = new List<Owner>(owners);
            }
            return ownerList;
        }

        public static List<Home> GetListOfHomes()
        {
            List<Home> homesList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                var homes = (context.Homes.OrderBy(o => o.Zip)
                                          .ThenBy(o => o.Address)).ToList();
                homesList = new List<Home>(homes);
            }
            return homesList;
        }

        public static List<RealEstateCompany> GetListOfRECompanies()
        {
            List<RealEstateCompany> RECoList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                var reCos = (context.RealEstateCompanies.OrderBy(o => o.CompanyName)).ToList();
                RECoList = new List<RealEstateCompany>(reCos);
            }
            return RECoList;
        }

        public static List<Agent> GetListOfAgents()
        {
            List<Agent> agentList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                var agents = (context.Agents.OrderBy(a => a.AgentID)).ToList();
                agentList = new List<Agent>(agents);
            }
            return agentList;
        }

        public static List<Buyer> GetListOfBuyers()
        {
            List<Buyer> buyerList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                var buyers = (context.Buyers.OrderBy(b => b.BuyerID)).ToList();
                //var buyers = (from b in context.Buyers
                //              select b).ToList();
                buyerList = new List<Buyer>(buyers);
            }
            return buyerList;
        }

        public static List<Person> GetListOfPeople()
        {
            List<Person> peopleList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                var people = (context.People.OrderBy(o => o.LastName)
                                            .ThenBy(o => o.FirstName)).ToList();
                peopleList = new List<Person>(people);
            }
            return peopleList;
        }

        public static List<HomeSale> GetListOfHomeSales()
        {
            List<HomeSale> homeSalesList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                var homeSales = (context.HomeSales.OrderBy(o => o.SoldDate)
                                                  .ThenBy(o => o.MarketDate)
                                                  .ThenBy(o => o.SaleAmount)).ToList();
                homeSalesList = new List<HomeSale>(homeSales);
            }
            return homeSalesList;
        }
    }
}
