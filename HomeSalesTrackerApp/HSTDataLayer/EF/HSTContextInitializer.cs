using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.IO;
using HomeSalesTrackerDataLayer;
using System.Data.Entity.Validation;

namespace HSTDataLayer
{
    public class HSTContextInitializer : DropCreateDatabaseAlways<HSTDataModel>
    {
        private static List<string> filenames = FilesHelper.GetFileNames();
        private static List<string> descendantNames = FilesHelper.GetDescendantNames();

        protected override void Seed(HSTDataModel context)
        {
            base.Seed(context);
        }

        public static void InitDB()
        {
            using (HSTDataModel context = new HSTDataModel())
            {
                var people = (from p in context.People
                              select p).ToList();
                List<Person> ppl = new List<Person>(people);    //  TODO: Is this List<Person> even necessary in a DB initializer?
            }
        }

        public static void LoadDataIntoDatabase()
        {
            using (HSTDataModel context = new HSTDataModel())
            {
                //  load data into Context in the specified order
                List<FileInfo> filePaths = FilesHelper.GetFileInfos(filenames);
                int counter = 0;
                while (counter < filenames.Count)
                {
                    string descendantName = descendantNames[counter].Trim();
                    FileInfo filepath = filePaths.Find(x => x.Name == filenames[counter]);
                    switch (counter)
                    {
                        case 0:
                            {
                                var people = XmlHelper.GetPeople(filepath, descendantName);
                                foreach (var p in people)
                                {
                                    context.People.Add(p);
                                }
                                break;
                            }
                        case 1:
                            {
                                var owners = XmlHelper.GetOwners(filepath, descendantName);
                                foreach (var o in owners)
                                {
                                    context.Owners.Add(o);
                                }
                                break;
                            }
                        case 2:
                            {
                                var homes = XmlHelper.GetHomes(filepath, descendantName);
                                context.Homes.AddRange(homes);
                                break;
                            }
                        case 3:
                            {
                                var recos = XmlHelper.GetRealEstateCompanies(filepath, descendantName);
                                foreach (var re in recos)
                                {
                                    context.RealEstateCompanies.Add(re);
                                }
                                break;
                            }
                        case 4:
                            {
                                var agents = XmlHelper.GetAgents(filepath, descendantName);
                                foreach (var a in agents)
                                {
                                    context.Agents.Add(a);
                                }
                                break;
                            }
                        case 5:
                            {
                                var buyers = XmlHelper.GetBuyers(filepath, descendantName);
                                foreach (var b in buyers)
                                {
                                    context.Buyers.Add(b);
                                }
                                break;
                            }
                        case 6:
                            {
                                var homeSales = XmlHelper.GetHomeSales(filepath, descendantName);
                                foreach (var hs in homeSales)
                                {
                                    context.HomeSales.Add(hs);
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    }
                    counter++;
                }

                try
                {
                    //  save changes
                    int recordsAffected = context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in errors.ValidationErrors)
                        {
                            // get the error message 
                            string errorMessage = validationError.ErrorMessage;
                            Console.WriteLine(errorMessage);
                        }
                    }
                }
            }
        }
    }
}
