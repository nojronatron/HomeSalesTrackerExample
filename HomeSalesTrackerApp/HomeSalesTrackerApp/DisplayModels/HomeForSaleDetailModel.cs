using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeForSaleDetailModel: HomeForSaleModel
    {
		private string _recoPhone;
		private string _recoName;
		private string _ownerFullName => $"{ this.OwnerFirstName } { this.OwnerLastName }";
		private string _agentFullName => $"{ this.AgentFirstName } { this.AgentLastName }";

		public string OwnerFirstName { get; set; }
		public string OwnerLastName { get; set; }
		public string PreferredLender { get; set; }
		public string AgentFirstName { get; set; }
		public string AgentLastName { get; set; }
		public Decimal CommissionPercent { get; set; }

		
		public string RecoName
		{
			get
			{
				if (string.IsNullOrEmpty(_recoName))
				{
					return "Agent no longer active.";
				}
				return _recoName;
			}
			set
			{
				_recoName = value;
			}
		}

		public string RecoPhone 
		{ 
			get
			{
				if (string.IsNullOrEmpty(_recoPhone))
				{
					return string.Empty;
				}
				return $"({ _recoPhone.Substring(0, 3) }) { _recoPhone.Substring(3, 3) }-{ _recoPhone.Substring(6, 4) }";
			}
			set
			{
				_recoPhone = value;
			}
		}
		
		public override string ToString()
		{
			return $" { base.ToString() } { this._ownerFullName } {this._agentFullName } {this.PreferredLender } { this.RecoName } { this.RecoPhone }";
		}

		public override string ToStackedString()
		{
			return $"{ base.ToStackedString() }\n" +
				$"Owner: { this._ownerFullName }\n" +
				$"Owner's Preferred Lender: { this.PreferredLender }\n" +
				$"Agent: { this._agentFullName }\n" +
				$"Agent Commission: { this.CommissionPercent * 100 }%\n" +
				$"Real Estate Co: { this.RecoName }\n" +
				$"RE Co Phone: { this.RecoPhone }";
		}

	}
}
