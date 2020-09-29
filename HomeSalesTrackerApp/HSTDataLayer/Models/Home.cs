namespace HSTDataLayer
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Home
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Home()
        {
            HomeSales = new HashSet<HomeSale>();
        }

        public int HomeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(9)]
        public string Zip { get; set; }

        public int OwnerID { get; set; }

        public virtual Owner Owner { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeSale> HomeSales { get; set; }
    }
}
