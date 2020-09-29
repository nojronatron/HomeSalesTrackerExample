namespace HSTDataLayer
{
    /// <summary>
    /// Inherits from Person. A Person can be an Agent, Buyer, and a Owner.
    /// </summary>
    public partial class Owner
    {
        public override string ToString()
        {
            return $"{ base.ToString() } { this.PreferredLender }";
        }
    }
}
