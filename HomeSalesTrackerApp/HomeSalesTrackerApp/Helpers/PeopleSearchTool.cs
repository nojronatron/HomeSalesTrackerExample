using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HomeSalesTrackerApp.Helpers
{
    class PeopleSearchTool
    {
        public List<PersonModel> SearchResults { get; set; }

        private List<string> SearchTerms { get; set; }

        public PeopleSearchTool(List<string> searchTerms)
        {
            SearchTerms = searchTerms;
            PersonSearch();
        }

        private void PersonSearch()
        {
            if (SearchTerms.Count < 1)
            {
                SearchResults = new List<PersonModel>();
                return;
            }

            var searchResults = new List<Person>();

            foreach (var searchTerm in SearchTerms)
            {
                string capSearchTerm = searchTerm.ToUpper().Trim();
                //var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();

                searchResults.AddRange(((App)Application.Current)._peopleCollection.OfType<Person>().Where(
                    p =>
                        p.PersonID.ToString().Contains(capSearchTerm) ||
                        p.FirstName.ToUpper().Contains(capSearchTerm) ||
                        p.LastName.ToUpper().Contains(capSearchTerm) ||
                        p.Phone.ToUpper().Contains(capSearchTerm) ||
                        (!string.IsNullOrEmpty(p.Email) && p.Email.ToUpper().Contains(capSearchTerm))
                        ));

            }

            searchResults.Distinct();

            if (searchResults.Count > 0)
            {
                var collatedSearchResults = new List<PersonModel>();
                searchResults = searchResults.Distinct<Person>().ToList();

                PersonModel newPersonModel = null;

                foreach (var person in searchResults)
                {
                    if (person.Agent == null && person.Buyer == null && person.Owner == null)
                    {
                        newPersonModel = new PersonModel()
                        {
                            PersonID = person.PersonID,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            Phone = person.Phone,
                            Email = person.Email ?? null,
                            PersonType = string.Empty
                        };

                        collatedSearchResults.Add(newPersonModel);
                    }
                    else
                    {

                        if (person.Agent != null)
                        {
                            newPersonModel = new PersonModel()
                            {
                                PersonID = person.PersonID,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Phone = person.Phone,
                                Email = person.Email ?? null,
                                PersonType = person.Agent.GetType().Name
                            };

                            collatedSearchResults.Add(newPersonModel);
                        }

                        if (person.Buyer != null)
                        {
                            newPersonModel = new PersonModel()
                            {
                                PersonID = person.PersonID,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Phone = person.Phone,
                                Email = person.Email ?? null,
                                PersonType = person.Buyer.GetType().Name
                            };

                            collatedSearchResults.Add(newPersonModel);
                        }

                        if (person.Owner != null)
                        {
                            newPersonModel = new PersonModel()
                            {
                                PersonID = person.PersonID,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Phone = person.Phone,
                                Email = person.Email ?? null,
                                PersonType = person.Owner.GetType().Name
                            };

                            collatedSearchResults.Add(newPersonModel);
                        }

                    }
                }

                SearchResults = collatedSearchResults;
            }
        }

        public static string GetPersonDetails(PersonModel foundPerson)
        {
            //var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
            Person foundPersonFull = ((App)Application.Current)._peopleCollection.FirstOrDefault(p => p.PersonID == foundPerson.PersonID);

            if (foundPerson == null)
            {
                return string.Empty;
            }

            if (foundPerson.PersonType == new BuyerModel().PersonType)
            {
                List<SoldHomeModel> purchasedHomes = new List<SoldHomeModel>();
                //var fHomeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
                purchasedHomes = (from hfs in ((App)Application.Current)._homeSalesCollection
                                  where hfs.BuyerID == foundPerson.PersonID
                                  join h in ((App)Application.Current)._homesCollection on hfs.HomeID equals h.HomeID
                                  select new SoldHomeModel
                                  {
                                      HomeID = h.HomeID,
                                      Address = h.Address,
                                      City = h.City,
                                      State = h.State,
                                      Zip = h.Zip,
                                      SaleAmount = hfs.SaleAmount,
                                      SoldDate = hfs.SoldDate
                                  }).ToList();

                var buyerPerson = (from hfs in ((App)Application.Current)._homeSalesCollection
                                   where hfs.BuyerID == foundPerson.PersonID
                                   select new BuyerModel
                                   {
                                       PersonID = foundPersonFull.PersonID,
                                       FirstName = foundPersonFull.FirstName,
                                       LastName = foundPersonFull.LastName,
                                       Phone = foundPersonFull.Phone,
                                       Email = foundPersonFull.Email ?? "- not supplied -",
                                       PersonType = foundPerson.PersonType,
                                       CreditRating = hfs.Buyer.CreditRating,
                                       PurchasedHomes = purchasedHomes
                                   }).FirstOrDefault();

                return buyerPerson.ToStackedString();
            }

            if (foundPerson.PersonType == new OwnerModel().PersonType)
            {
                List<HomeDisplayModel> ownedHomes = new List<HomeDisplayModel>();
                ownedHomes = (from h in ((App)Application.Current)._homesCollection
                              where h.OwnerID == foundPerson.PersonID
                              select new HomeDisplayModel
                              {
                                  HomeID = h.HomeID,
                                  Address = h.Address,
                                  City = h.City,
                                  State = h.State,
                                  Zip = h.Zip
                              }).ToList();

                OwnerModel ownerPerson = (from p in ((App)Application.Current)._peopleCollection
                                          join h in ((App)Application.Current)._homesCollection on p.PersonID equals h.OwnerID
                                          where p.PersonID == foundPerson.PersonID
                                          select new OwnerModel
                                          {
                                              PreferredLender = p.Owner.PreferredLender,
                                              PersonID = foundPersonFull.PersonID,
                                              FirstName = foundPersonFull.FirstName,
                                              LastName = foundPersonFull.LastName,
                                              Phone = foundPersonFull.Phone,
                                              Email = foundPersonFull.Email ?? "- not supplied -",
                                              PersonType = foundPerson.PersonType,
                                              OwnerID = foundPersonFull.PersonID,
                                              OwnedHomes = ownedHomes
                                          }).FirstOrDefault();

                return ownerPerson.ToStackedString();
            }

            if (foundPerson.PersonType == new AgentModel().PersonType)
            {
                List<SoldHomeModel> soldHomes = new List<SoldHomeModel>();
                //var fHomeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
                soldHomes = (from hfs in ((App)Application.Current)._homeSalesCollection
                             where hfs.AgentID == foundPerson.PersonID &&
                             hfs.SoldDate != null
                             join h in ((App)Application.Current)._homesCollection on hfs.HomeID equals h.HomeID
                             select new SoldHomeModel
                             {
                                 HomeID = h.HomeID,
                                 Address = h.Address,
                                 City = h.City,
                                 State = h.State,
                                 Zip = h.Zip,
                                 SaleAmount = hfs.SaleAmount,
                                 SoldDate = hfs.SoldDate
                             }).ToList();

                List<HomeForSaleModel> homesForSale = new List<HomeForSaleModel>();
                homesForSale = (from hfs in ((App)Application.Current)._homeSalesCollection
                                where hfs.AgentID == foundPersonFull.PersonID &&
                                hfs.SoldDate == null
                                join h in ((App)Application.Current)._homesCollection on hfs.HomeID equals h.HomeID
                                select new HomeForSaleModel
                                {
                                    HomeID = hfs.HomeID,
                                    Address = h.Address,
                                    City = h.City,
                                    State = h.State,
                                    Zip = h.Zip,
                                    MarketDate = hfs.MarketDate,
                                    SaleAmount = hfs.SaleAmount
                                }).ToList();

                //var fRECOsCollection = CollectionFactory.GetRECosCollectionObject();
                var agentPerson = (from hfs in ((App)Application.Current)._homeSalesCollection
                                   join re in ((App)Application.Current)._recosCollection on hfs.CompanyID equals re.CompanyID
                                   select new AgentModel
                                   {
                                       PersonID = foundPersonFull.PersonID,
                                       FirstName = foundPersonFull.FirstName,
                                       LastName = foundPersonFull.LastName,
                                       Phone = foundPersonFull.Phone,
                                       Email = foundPersonFull.Email ?? "- not supplied -",
                                       PersonType = foundPerson.PersonType,
                                       CommissionRate = hfs.Agent.CommissionPercent,
                                       HomesOnMarket = homesForSale,
                                       SoldHomes = soldHomes,
                                       RECoID = re.CompanyID,
                                       RECompanyName = re.CompanyName,
                                       RECoPhone = re.Phone
                                   }).FirstOrDefault();

                return agentPerson.ToStackedString();

            }

            return string.Empty;
        }

    }
}
