using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeSalesTrackerApp.Report_Models
{
	public class AgentsReportModel :
		PersonBaseModel,
		IEqualityComparer<AgentsReportModel>, IEquatable<AgentsReportModel>
	{
		
		private int _agentID;

		public int AgentID
		{
			get { return _agentID; }
			set
			{
				if (_agentID != value)
				{
					_agentID = value;
				}
			}
		}

		private string _realEstateCompany;

		public string RealEstateCompany
		{
			get { return _realEstateCompany; }
			set
			{
				if (_realEstateCompany != value)
				{
					_realEstateCompany = value;
				}
			}
		}

		private decimal _commission;

		public decimal Commission
		{
			get { return _commission; }
			set
			{
				if (_commission != value)
				{
					_commission = value;
				}
			}
		}

		private int _totalHomesSold;

		public int TotalHomesSold
		{
			get { return _totalHomesSold; }
			set
			{
				if (_totalHomesSold != value)
				{
					_totalHomesSold = value;
				}
			}
		}

		private int _homesOnMarket;

		public int HomesOnMarket
		{
			get { return _homesOnMarket; }
			set
			{
				if (_homesOnMarket != value)
				{
					_homesOnMarket = value;
				}
			}
		}

		private decimal _ttlCommissionsPaid;

		public decimal TotalCommissionsPaid
		{
			get { return _ttlCommissionsPaid; }
			set
			{
				if (_ttlCommissionsPaid != value)
				{
					_ttlCommissionsPaid = value;
				}
			}
		}

		private decimal _ttlSalesOfSoldHomes;

		public decimal TotalSales
		{
			get { return _ttlSalesOfSoldHomes; }
			set
			{
				if (_ttlSalesOfSoldHomes != value)
				{
					_ttlSalesOfSoldHomes = value;
				}
			}
		}

		public bool Equals(AgentsReportModel x, AgentsReportModel y)
		{
			if (x == null && y == null)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			return x.FirstName == y.FirstName && x.LastName == y.LastName &&
				x.AgentID == y.AgentID;
		}

		public int GetHashCode(AgentsReportModel obj)
		{
			return (this.FirstName + this.LastName + this.AgentID).GetHashCode();
		}

		public bool Equals(AgentsReportModel other)
		{
			if (Object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (Object.ReferenceEquals(this, other))
			{
				return true;
			}
			return FirstName.Equals(other.FirstName) && 
				this.LastName.Equals(other.LastName) &&
				this.AgentID.Equals(other.AgentID);
		}

		public override int GetHashCode()
		{
			return -2088337453 + AgentID.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public static bool operator ==(AgentsReportModel left, AgentsReportModel right)
		{
			return EqualityComparer<AgentsReportModel>.Default.Equals(left, right);
		}

		public static bool operator !=(AgentsReportModel left, AgentsReportModel right)
		{
			return !(left == right);
		}
	}
}
