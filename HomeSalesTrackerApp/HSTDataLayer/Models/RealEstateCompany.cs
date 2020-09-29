namespace HSTDataLayer
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class RealEstateCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RealEstateCompany()
        {
            Agents = new HashSet<Agent>();
            HomeSales = new HashSet<HomeSale>();
        }

        [Key]
        public int CompanyID { get; set; }

        [Required]
        [StringLength(30)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agent> Agents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeSale> HomeSales { get; set; }
    }
}
