using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

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
                searchResults.AddRange(MainWindow.peopleCollection.OfType<Person>().Where(
                    p =>
                        p.PersonID.ToString().Contains(capSearchTerm) ||
                        p.FirstName.ToUpper().Contains(capSearchTerm) ||
                        p.LastName.ToUpper().Contains(capSearchTerm) ||
                        p.Phone.ToUpper().Contains(capSearchTerm)  ||
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

    }
}