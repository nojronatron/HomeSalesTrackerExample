namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class HomeSale : IEquatable<HomeSale>, IComparable<HomeSale>
    {
        public int SaleID { get; set; }

        public int HomeID { get; set; }

        public DateTime? SoldDate { get; set; }

        public int AgentID { get; set; }

        public decimal SaleAmount { get; set; }

        public int? BuyerID { get; set; }

        public DateTime MarketDate { get; set; }

        public int CompanyID { get; set; }

        public virtual Agent Agent { get; set; }

        public virtual Buyer Buyer { get; set; }

        public virtual Home Home { get; set; }

        public virtual RealEstateCompany RealEstateCompany { get; set; }

        /// <summary>
        /// Override ToString() to control a basic string-output of a HomeSale instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string marketDate = this.MarketDate.ToString("d");
            string soldDate = this.SoldDate.GetValueOrDefault().ToString("d");
            decimal saleAmount = this.SaleAmount;
            return $"Market Date: { marketDate }\nSold Date: { soldDate }\nSale Amount: {saleAmount:c}";
        }

        /// <summary>
        /// Required IComparable method implementation. Reference for multi-field sorting: https://stackoverflow.com/questions/4501501/custom-sorting-icomparer-on-three-fields
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int IComparable<HomeSale>.CompareTo(HomeSale other)
        {
            if (other == null)
            {
                return 1;
            }
            int result = this.MarketDate.Date.CompareTo(other.MarketDate.Date);
            if (result == 0)
            {
                result = this.SaleAmount.CompareTo(other.SaleAmount);
                if (result == 0)
                {
                    if (other.SoldDate.HasValue && other.SoldDate.HasValue)
                    {
                        DateTime thisTestValue = this.SoldDate.Value;
                        DateTime otherTestValue = other.SoldDate.Value;
                        result = thisTestValue.CompareTo(otherTestValue);
                    }
                }
            }
            return result;
        }

        public static explicit operator HomeSale(HSTDataLayer.HomeSale v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Required IEquatable method implementation.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool IEquatable<HomeSale>.Equals(HomeSale other)
        {
            if (other != null)
            {
                if (this.SoldDate == other.SoldDate &&
                    this.SaleAmount == other.SaleAmount &&
                    this.MarketDate == other.MarketDate)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Required Equals() override when implementing IEquitable T (IDE generated code)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is HomeSale sale &&
                   SoldDate == sale.SoldDate &&
                   SaleAmount == sale.SaleAmount &&
                   MarketDate == sale.MarketDate;
        }

        /// <summary>
        /// Recommended GetHashCode() override when overriding Equals() (IDE generated code]
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 1752065002;
            hashCode = hashCode * -1521134295 + SoldDate.GetHashCode();
            hashCode = hashCode * -1521134295 + SaleAmount.GetHashCode();
            hashCode = hashCode * -1521134295 + MarketDate.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Recommended Equal Operator override. Supports IEquatable implementation and Equals() override methods.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HomeSale left, HomeSale right)
        {
            return EqualityComparer<HomeSale>.Default.Equals(left, right);
        }

        /// <summary>
        /// Recommended Not Equal Operator override. Supports IEquatable implementation and Equals() override methods.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HomeSale left, HomeSale right)
        {
            return !(left == right);
        }

    }
}
