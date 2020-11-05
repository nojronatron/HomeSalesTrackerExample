using System;

namespace HomeSalesTrackerApp.Helpers
{
    public struct NotificationData
    {
		private int _changeCount;

		public int ChangeCount
		{
			get { return _changeCount; }
			//	set { _changeCount = value; }
		}

		private string _dataType;

		public string DataType
		{
			get { return _dataType; }
			//	set { _dataType = value; }
		}

		public NotificationData(int changeCount, string dataType)
		{
			this._changeCount = changeCount;
			this._dataType = dataType;
		}

	}
}
