using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.Helpers
{
    public class CollectionMonitor : IObservable<NotificationData>
    {
        List<IObserver<NotificationData>> observers;

        public CollectionMonitor()
        {
            observers = new List<IObserver<NotificationData>>();
        }

        public IDisposable Subscribe(IObserver<NotificationData> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<NotificationData>> _observers;
            private IObserver<NotificationData> _observer;

            public Unsubscriber(List<IObserver<NotificationData>> observers, IObserver<NotificationData> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null)
                {
                    _observers.Remove(_observer);
                }

            }

        }

        public void SendNotifications(int count, string dataType)
        {
            //  TODO: determine where to place an OnComplete() method call aka transmission commplete

            if (count > 0 && !string.IsNullOrEmpty(dataType))
            {
                NotificationData notificationData = new NotificationData(count, dataType);

                foreach (var observer in observers)
                {
                    observer.OnNext(notificationData);

                }

            }

        }

    }
}
