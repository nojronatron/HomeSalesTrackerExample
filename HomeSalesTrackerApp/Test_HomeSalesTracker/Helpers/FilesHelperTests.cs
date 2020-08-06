using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace HomeSalesTrackerDataLayer.Tests
{
    [TestClass()]
    public class FilesHelperTests
    {
        private static string testFilename = "TestXml_TestFile.xml";
        private static string testFullFilename = Path.Combine(Directory.GetCurrentDirectory(), testFilename);
        private static List<string> filenames = new List<string>()
        {
            "People.xml",
            "Owners.xml",
            "Homes.xml",
            "RealEstateCompanies.xml",
            "Agents.xml",
            "Buyers.xml",
            "HomeSales.xml"
        };
        private static XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");
        private static XDocument testXDoc = null;

        [TestMethod()]
        public void WriteOutXmlFilesTest()
        {
            //  create a file, get the FileInfo object on it, set FI obj to a List<FileInfo> send List to FilesHelper.SetFullFilePaths() method

            testXDoc = new XDocument(
                new XDeclaration(xDeclaration),
                new XElement("TestRoots",
                new XElement("TestRoot",
                new XElement("TestChildNode1", "Testing1"),
                new XElement("TestChildNode2", "Testing2"),
                new XElement("TestChildNode3", "Testing3")
                )));

            bool actualResult = FilesHelper.WriteOutXmlFiles(testXDoc, "Agents.xml");

            FileInfo fi = new FileInfo(testFilename);
            bool actualExists = fi.Exists;
            bool actualSize = (fi.Length == 255);

            Assert.IsTrue(actualExists);
            Console.WriteLine($"Created: { testFullFilename }\n");
            Assert.AreEqual(actualExists, actualSize);
        }

        [TestMethod()]
        public void GetFileInfosTest()
        {
            int expectedResult = 7;
            
            List<FileInfo> fileInfosList = new List<FileInfo>();
            List<string> filenames = FilesHelper.GetFileNames();
            fileInfosList = FilesHelper.GetFileInfos(filenames);
            
            Assert.IsNotNull(fileInfosList);
            int actualResult = fileInfosList.Count;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCleanup()]
        public void CleanupFiles()
        {
            FileInfo fi = null;
            fi = new FileInfo(testFullFilename);
            if (fi.Exists)
            {
                Console.WriteLine($"\nDeleting { testFullFilename }");
                fi.Delete();
            }
        }
    }
}
 