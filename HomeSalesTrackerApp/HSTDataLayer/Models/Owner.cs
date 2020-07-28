namespace HSTDataLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Owner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Owner()
        {
            Homes = new HashSet<Home>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OwnerID { get; set; }

        [StringLength(30)]
        public string PreferredLender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Home> Homes { get; set; }

        public virtual Person Person { get; set; }
    }
}
