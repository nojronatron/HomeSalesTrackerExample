namespace HSTDataLayer
{
    using System.ComponentModel.DataAnnotations;

    public partial class Person
    {
        public int PersonID { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public virtual Agent Agent { get; set; }

        public virtual Buyer Buyer { get; set; }

        public virtual Owner Owner { get; set; }
    }
}
