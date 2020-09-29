using HomeSalesTrackerDataLayer;

using System.Linq;
using System.Xml.Linq;

namespace HSTDataLayer.Helpers
{
    public class BackUpDataFiles
    {
        private static XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");

        /// <summary>
        /// Flush People entities to an XDocument and write it to file
        /// </summary>
        /// <returns></returns>
        public static bool FlushPeopleTableToXml()
        {
            bool done = false;
            using (var context = new HSTDataModel())
            {
                //var peopleList = EntityLists.GetListOfPeople();
                var peopleList = context.People.OrderBy(p => p.PersonID).ToList();
                var xDocument = new XDocument(
                    new XDeclaration(xDeclaration),
                    new XElement("People",
                    from p in peopleList
                    select new XElement("Person",
                    new XElement("PersonID", p.PersonID),
                    new XElement("FirstName", p.FirstName),
                    new XElement("LastName", p.LastName),
                    new XElement("Phone", p.Phone),
                    new XElement("Email", p.Email)
                    )));
                string filename = "People.xml";
                done = FilesHelper.WriteOutXmlFiles(xDocument, filename);
            }
            return done;
        }

        /// <summary>
        /// Flush Owners entities to an XDocument and write it to file
        /// </summary>
        /// <returns></returns>
        public static bool FlushOwnersTableToXml()
        {
            bool done = false;
            using (var context = new HSTDataModel())
            {
                var ownersList = context.Owners.OrderBy(o => o.OwnerID).ToList();
                var xDocument = new XDocument(
                    new XDeclaration(xDeclaration),
                    new XElement("Owners",
                    from o in ownersList
                    select new XElement("Owner",
                        new XElement("OwnerID", o.OwnerID),
                        new XElement("PreferredLender", o.PreferredLender)
                    )));
                string filename = "Owners.xml";
                done = FilesHelper.WriteOutXmlFiles(xDocument, filename);
            }
            return done;
        }

        public static bool FlushHomesTableToXml()
        {
            bool done = false;
            using (var context = new HSTDataModel())
            {
                var homesList = context.Homes.OrderBy(h => h.HomeID).ToList();
                var xDocument = new XDocument(
                    new XDeclaration(xDeclaration),
                    new XElement("Homes",
                    from h in homesList
                    select new XElement("Home",
                        new XElement("HomeID", h.HomeID),
                        new XElement("Address", h.Address),
                        new XElement("City", h.City),
                        new XElement("State", h.State),
                        new XElement("Zip", h.Zip),
                        new XElement("OwnerID", h.OwnerID)
                )));
                string filename = "Homes.xml";
                done = FilesHelper.WriteOutXmlFiles(xDocument, filename);
            }
            return done;
        }

        public static bool FlushRealEstateCompaniesTableToXml()
        {
            bool done = false;
            using (var context = new HSTDataModel())
            {
                var reCosList = context.RealEstateCompanies.OrderBy(re => re.CompanyID).ToList();
                var xDocument = new XDocument(
                    new XDeclaration(xDeclaration),
                    new XElement("RealEstateCommpanies",
                    from re in reCosList
                    select new XElement("RealEstateCompany",
                        new XElement("CompanyID", re.CompanyID),
                        new XElement("CompanyName", re.CompanyName),
                        new XElement("Phone", re.Phone)
                )));
                string filename = "RealEstateCompanies.xml";
                done = FilesHelper.WriteOutXmlFiles(xDocument, filename);
            }
            return done;
        }

        public static bool FlushAgentsTableToXml()
        {
            bool done = false;
            using (var context = new HSTDataModel())
            {
                var agentsList = context.Agents.OrderBy(a => a.AgentID).ToList();
                var xDocument = new XDocument(
                    new XDeclaration(xDeclaration),
                    new XElement("Agents",
                    from a in agentsList
                    select new XElement("Agent",
                        new XElement("AgentID", a.AgentID),
                        new XElement("CompanyID", a.CompanyID),
                        new XElement("CommissionPercent", a.CommissionPercent)
                )));
                string filename = "Agents.xml";
                done = FilesHelper.WriteOutXmlFiles(xDocument, filename);
            }
            return done;
        }

        public static bool FlushBuyersTableToXml()
        {
            bool done = false;
            using (var context = new HSTDataModel())
            {
                var buyersList = context.Buyers.OrderBy(b => b.BuyerID).ToList();
                var xDocument = new XDocument(
                    new XDeclaration(xDeclaration),
                    new XElement("Buyers",
                    from b in buyersList
                    select new XElement("Buyer",
                        new XElement("BuyerID", b.BuyerID),
                        new XElement("CreditRating", b.CreditRating)
                )));
                string filename = "Buyers.xml";
                done = FilesHelper.WriteOutXmlFiles(xDocument, filename);
            }
            return done;
        }

        public static bool FlushHomeSalesTableToXml()
        {
            bool done = false;
            using (var context = new HSTDataModel())
            {
                var homesalesList = context.HomeSales.OrderBy(hs => hs.SaleID).ToList();
                var xDocument = new XDocument(
                    new XDeclaration(xDeclaration),
                    new XElement("HomeSales",
                    from hs in homesalesList
                    select new XElement("HomeSale",
                        new XElement("SaleID", hs.SaleID),
                        new XElement("HomeID", hs.HomeID),
                        new XElement("SoldDate", hs.SoldDate),
                        new XElement("AgentID", hs.AgentID),
                        new XElement("SaleAmount", hs.SaleAmount),
                        new XElement("BuyerID", hs.BuyerID),
                        new XElement("MarketDate", hs.MarketDate),
                        new XElement("CompanyID", hs.CompanyID)
                )));
                string filename = "HomeSales.xml";
                done = FilesHelper.WriteOutXmlFiles(xDocument, filename);
            }
            return done;
        }

    }
}
