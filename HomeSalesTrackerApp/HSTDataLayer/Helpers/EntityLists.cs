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
        /// <summary>
        /// A Person is the parent model for Agents, Buyers, and Owners, who contain PersonID reference pointers.
        /// </summary>
        /// <returns></returns>
        public static List<Person> GetListOfPeople()
        {
            List<Person> peopleList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                peopleList = new List<Person>();
                foreach (Person person in context.People.Include(o => o.Owner)
                                                        .Include(a => a.Agent)
                                                        .Include(b => b.Buyer))
                {
                    peopleList.Add(person);
                }
            }
            return peopleList;
        }

        /// <summary>
        /// Owner has a collection of Homes
        /// </summary>
        /// <returns></returns>
        public static List<Owner> GetTreeListOfOwners()
        {
            List<Owner> ownerList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                foreach (var owner in context.Owners.Include(h => h.Homes)
                                                    .Include(p => p.Person))
                {
                    ownerList.Add(owner);
                }
            }
            return ownerList;
        }

        /// <summary>
        /// Home has a collection of HomeSales
        /// </summary>
        /// <returns></returns>
        public static List<Home> GetTreeListOfHomes()
        {
            List<Home> homesList = null;
            using (HSTDataModel context = new HSTDataModel())
            {
                homesList = new List<Home>();
                foreach (Home home in context.Homes.Include(hs => hs.HomeSales)
                                                   .Include(o => o.Owner))
                {
                    homesList.Add(home);
                }
            }
            return homesList;
        }

        /// <summary>
        /// RealEstateCompany has a collection of Agents and HomeSales
        /// </summary>
        /// <returns></returns>
        public static List<RealEstateCompany> GetTreeListOfRECompanies()
        {
            var recosList = default(List<RealEstateCompany>);   //  allows using var, forces null if nullable type
            using (var context = new HSTDataModel())
            {
                recosList = new List<RealEstateCompany>();
                foreach (var reco in context.RealEstateCompanies.Include(hs => hs.HomeSales)
                                                                .Include(a => a.Agents))
                {
                    recosList.Add(reco);
                }
            }
            return recosList;
        }

        /// <summary>
        /// Agent has a collection of HomeSales
        /// </summary>
        /// <returns></returns>
        public static List<Agent> GetTreeListOfAgents()
        {
            var agentList = default(List<Agent>);
            using (HSTDataModel context = new HSTDataModel())
            {
                foreach (var agent in context.Agents.Include(p => p.Person)
                                                    .Include(r => r.RealEstateCompany)
                                                    .Include(a => a.HomeSales))
                {
                    agentList.Add(agent);
                }
            }
            return agentList;
        }

        /// <summary>
        /// Buyer has a collection of HomeSales
        /// </summary>
        /// <returns></returns>
        public static List<Buyer> GetTreeListOfBuyers()
        {
            var buyersList = default(List<Buyer>);
            using (var context = new HSTDataModel())
            {
                buyersList = new List<Buyer>();
                foreach (var buyer in context.Buyers.Include(p => p.Person)
                                                    .Include(b => b.HomeSales))
                {
                    buyersList.Add(buyer);
                }
            }
            return buyersList;
        }

        /// <summary>
        /// HomeSale does not contain any collection of other entities
        /// </summary>
        /// <returns></returns>
        public static List<HomeSale> GetListOfHomeSales()
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

    }
}
