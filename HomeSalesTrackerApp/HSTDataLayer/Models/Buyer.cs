namespace HSTDataLayer
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Buyer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Buyer()
        {
            HomeSales = new HashSet<HomeSale>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BuyerID { get; set; }

        public int? CreditRating { get; set; }

        public virtual Person Person { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeSale> HomeSales { get; set; }
    }
}
