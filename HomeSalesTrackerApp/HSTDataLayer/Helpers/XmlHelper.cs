using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace HSTDataLayer
{
    /// <summary>
    /// Static Class has static Methods that process 
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Function returns an enumerable List of T representing descendant XML elements in filename.
        /// Filename must be FullName meaning full file path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <param name="descendantName"></param>
        /// <returns></returns>
        public static List<XElement> GetXmlFileData(string filename, string descendantName)
        {
            XDocument xdocFile = XDocument.Load(filename);
            var xmlData = (from el in xdocFile.Descendants(descendantName)
                           select el).ToList();
            //List<XDocument> xmlList = new List<XDocument();
            return xmlData;
        }

        /// <summary>
        /// Return a list of objects hydrated from XML file data. Depends on GetXmlFileData().
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static List<Person> GetPeople(FileInfo filepath, string objName)
        {
            List<Person> peopleList = new List<Person>();
            var xmlFileData = GetXmlFileData(filepath.FullName, objName);
            foreach (var p in xmlFileData)
            {
                Person person = new Person();
                person.PersonID = int.Parse(p.Element("PersonID").Value);
                person.FirstName = p.Element("FirstName")?.Value.Trim();
                person.LastName = p.Element("LastName")?.Value.Trim();
                person.Phone = p.Element("Phone")?.Value.Trim();
                person.Email = p.Element("Email")?.Value.Trim();
                peopleList.Add(person);
            }
            return peopleList;
        }

        /// <summary>
        /// Return a list of objects hydrated from XML file data. Depends on GetXmlFileData().
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static List<Owner> GetOwners(FileInfo filepath, string objName)
        {
            List<Owner> owners = new List<Owner>();
            var xmlFileData = GetXmlFileData(filepath.FullName, objName);
            foreach (var o in xmlFileData)
            {
                Owner owner = new Owner();
                owner.OwnerID = int.Parse(o.Element("OwnerID").Value);
                owner.PreferredLender = o.Element("PreferredLender")?.Value.Trim();
                owners.Add(owner);
            }
            return owners;
        }

        /// <summary>
        /// Return a list of objects hydrated from XML file data. Depends on GetXmlFileData().
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static List<Home> GetHomes(FileInfo filepath, string objName)
        {
            List<Home> homes = new List<Home>();
            var xmlFileData = GetXmlFileData(filepath.FullName, objName);
            foreach (var h in xmlFileData)
            {
                Home home = new Home();
                home.HomeID = int.Parse(h.Element("HomeID").Value);
                home.Address = h.Element("Address").Value.Trim();
                home.City = h.Element("City").Value.Trim();
                home.State = h.Element("State").Value.Trim();
                home.Zip = h.Element("Zip").Value.Trim();
                home.OwnerID = int.Parse(h.Element("OwnerID").Value);
                homes.Add(home);
            }
            return homes;
        }

        /// <summary>
        /// Return a list of objects hydrated from XML file data. Depends on GetXmlFileData().
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static List<RealEstateCompany> GetRealEstateCompanies(FileInfo filepath, string objName)
        {
            List<RealEstateCompany> rECompanies = new List<RealEstateCompany>();
            var xmlFileData = GetXmlFileData(filepath.FullName, objName);
            foreach (var r in xmlFileData)
            {
                RealEstateCompany reco = new RealEstateCompany();
                reco.CompanyID = int.Parse(r.Element("CompanyID").Value);
                reco.CompanyName = r.Element("CompanyName").Value.Trim();
                reco.Phone = r.Element("Phone")?.Value.Trim();
                rECompanies.Add(reco);
            }
            return rECompanies;
        }

        /// <summary>
        /// Return a list of objects hydrated from XML file data. Depends on GetXmlFileData().
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static List<Agent> GetAgents(FileInfo filepath, string objName)
        {
            List<Agent> agents = new List<Agent>();
            var xmlFileData = GetXmlFileData(filepath.FullName, objName);
            foreach (var a in xmlFileData)
            {
                Agent agent = new Agent();
                agent.AgentID = int.Parse(a.Element("AgentID").Value);
                var company_id = a.Element("CompanyID")?.Value;
                if (company_id == null || string.IsNullOrEmpty(company_id))
                {
                    agent.CompanyID = null;
                }
                else
                {
                    //agent.CompanyID = int.Parse(a.Element("CompanyID")?.Value);
                    agent.CompanyID = int.Parse(company_id);
                }
                agent.CommissionPercent = decimal.Parse(a.Element("CommissionPercent").Value);
                agents.Add(agent);
            }
            return agents;
        }

        /// <summary>
        /// Return a list of objects hydrated from XML file data. Depends on GetXmlFileData().
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static List<Buyer> GetBuyers(FileInfo filepath, string objName)
        {
            List<Buyer> buyers = new List<Buyer>();
            var xmlFileData = GetXmlFileData(filepath.FullName, objName);
            foreach (var b in xmlFileData)
            {
                Buyer buyer = new Buyer();
                buyer.BuyerID = int.Parse(b.Element("BuyerID").Value);
                var credit_rating = b.Element("CreditRating")?.Value;
                if (credit_rating == null || string.IsNullOrEmpty(credit_rating))
                {
                    buyer.CreditRating = null;
                }
                else
                {
                    buyer.CreditRating = int.Parse(credit_rating);
                }
                buyers.Add(buyer);
            }
            return buyers;
        }

        /// <summary>
        /// Return a list of objects hydrated from XML file data. Depends on GetXmlFileData().
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static List<HomeSale> GetHomeSales(FileInfo filepath, string objName)
        {
            List<HomeSale> homeSales = new List<HomeSale>();
            var xmlFileData = GetXmlFileData(filepath.FullName, objName);
            foreach (var hs in xmlFileData)
            {
                HomeSale homeSale = new HomeSale();
                homeSale.SaleID = int.Parse(hs.Element("SaleID").Value);
                homeSale.HomeID = int.Parse(hs.Element("HomeID")?.Value);
                var date_time = hs.Element("SoldDate")?.Value;
                if (date_time == null || date_time == string.Empty)
                {
                    homeSale.SoldDate = null;
                }
                else
                {
                    homeSale.SoldDate = DateTime.Parse(hs.Element("SoldDate").Value);
                }
                homeSale.AgentID = int.Parse(hs.Element("AgentID")?.Value);
                homeSale.SaleAmount = decimal.Parse(hs.Element("SaleAmount")?.Value);
                var buyer_id = hs.Element("BuyerID")?.Value;
                if (buyer_id == null || string.IsNullOrEmpty(buyer_id))
                {
                    homeSale.BuyerID = null;
                }
                else
                {
                    homeSale.BuyerID = int.Parse(buyer_id);
                }
                homeSale.MarketDate = DateTime.Parse(hs.Element("MarketDate")?.Value);
                homeSale.CompanyID = int.Parse(hs.Element("CompanyID")?.Value);
                homeSales.Add(homeSale);
            }
            return homeSales;
        }
    }
}
