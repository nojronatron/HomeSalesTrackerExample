using HSTDataLayer.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.Factory
{
    class CollectionFactory
    {
        public static HomesCollection GetHomesCollectionObject()
        {
            return new HomesCollection(EntityLists.GetTreeListOfHomes());
        }
    }
}
