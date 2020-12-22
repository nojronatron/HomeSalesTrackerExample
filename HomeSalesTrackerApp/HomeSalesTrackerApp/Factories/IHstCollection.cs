using HomeSalesTrackerApp.Helpers;

using System;

namespace HomeSalesTrackerApp.Factories
{
    interface IHstCollection
    {
        CollectionMonitor collectionMonitor { get; }
        int Count { get; }
        bool Remove(int id);
    }
}
