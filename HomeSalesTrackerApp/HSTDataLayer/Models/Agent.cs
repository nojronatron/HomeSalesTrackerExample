namespace HSTDataLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            HomeSales = new HashSet<HomeSale>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AgentID { get; set; }

        public int? CompanyID { get; set; }

        public decimal CommissionPercent { get; set; }

        public virtual Person Person { get; set; }

        public virtual RealEstateCompany RealEstateCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeSale> HomeSales { get; set; }
    }
}
