using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace HomeSalesTrackerDataLayer
{
    public static class FilesHelper
    {
        private static List<FileInfo> _fullFilePaths = null;
        private static List<string> _filenames = new List<string>()
        {
            "People.xml",
            "Owners.xml",
            "Homes.xml",
            "RealEstateCompanies.xml",
            "Agents.xml",
            "Buyers.xml",
            "HomeSales.xml"
        };
        private static List<string> _descendantNames = new List<string>()
        {
            "Person",
            "Owner",
            "Home",
            "RealEstateCompany",
            "Agent",
            "Buyer",
            "HomeSale"
        };

        public static void SetFullFilePaths(List<FileInfo> fileInfoFullNames)
        {
            if (fileInfoFullNames.Count > 0)
            {
                _fullFilePaths = new List<FileInfo>(fileInfoFullNames);
            }
            else
            {
                _fullFilePaths = new List<FileInfo>();
            }
        }

        public static List<FileInfo> GetFullFilePaths()
        {
            return _fullFilePaths;
        }

        public static List<string> GetFileNames()
        {
            return _filenames;
        }

        public static List<string> GetDescendantNames()
        {
            return _descendantNames;
        }

        /// <summary>
        /// Accept a List of XDocuments and a List of string filenames and write out the XDocuments to XML files in the current Path.
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="filenames"></param>
        /// <returns></returns>
        public static bool WriteOutXmlFiles(XDocument table, string filename)
        {
            FileInfo currentFile = GetFullFilePaths().Find(x => x.Name == filename);
            table.Save(currentFile.FullName);
            return true;
        }

        /// <summary>
        /// Performs recursive scan for a list of filenames starting with ..\..\
        /// Returns a List of FileInfo objects representing xml files matching parameter names.
        /// </summary>
        /// <param name="filenames"></param>
        /// <returns>List of FileInfos</returns>
        public static List<FileInfo> GetFileInfos(List<string> filenames)
        {
            //string rootDirName = @"..\..\";
            string rootDirName = Directory.GetCurrentDirectory();
            FileInfo[] files;
            DirectoryInfo rootDirectory;
            List<FileInfo> xmlFullFilePaths = null;

            try
            {
                rootDirectory = new DirectoryInfo(rootDirName);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Console.WriteLine(dnfe.ToString());
                return xmlFullFilePaths;
            }

            files = rootDirectory.GetFiles("*.xml", SearchOption.AllDirectories);
            if (files.GetLength(0) == 0)
            {
                Console.WriteLine($"No XML files found in { rootDirName }.");
                return xmlFullFilePaths;
            }

            xmlFullFilePaths = new List<FileInfo>(filenames.Count);
            foreach (FileInfo file in files)
            {
                string test = file.Name;
                if (filenames.Contains(file.Name))
                {
                    xmlFullFilePaths.Add(file);
                }
            }

            xmlFullFilePaths.Distinct();

            if (xmlFullFilePaths.Count < 7)
            {
                //  At least one XML file is missing - will return an empty List<FileInfo>
                return new List<FileInfo>();
            }
            if (xmlFullFilePaths.Count > 7)
            {
                //  Too many XML files found - will return an empty List<FileInfo>
                return new List<FileInfo>();
            }
            
            //  retain the full filepaths for use later
            SetFullFilePaths(xmlFullFilePaths);
            return xmlFullFilePaths;
        }
    }
}
