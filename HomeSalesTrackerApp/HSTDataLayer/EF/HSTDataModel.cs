using System;
using System.Data.Entity;

namespace HSTDataLayer
{
 
    public partial class HSTDataModel : DbContext
    {
        public HSTDataModel()
            : base("name=HSTDataModelConnection")
        {
            Database.SetInitializer<HSTDataModel>(new DropCreateDatabaseAlways<HSTDataModel>());
            //Database.SetInitializer<HSTDataModel>(new CreateDatabaseIfNotExists<HSTDataModel>());
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            ////  disable initializer to avoid overwriting DB e.g. Production
            //  Database.SetInitializer<HSTDataModel>(null);
        }

        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<HomeSale> HomeSales { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<RealEstateCompany> RealEstateCompanies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .HasMany(e => e.HomeSales)
                .WithRequired(e => e.Agent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Home>()
                .Property(e => e.State)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Home>()
                .Property(e => e.Zip)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Home>()
                .HasMany(e => e.HomeSales)
                .WithRequired(e => e.Home)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Owner>()
                .HasMany(e => e.Homes)
                .WithRequired(e => e.Owner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .HasOptional(e => e.Agent)
                .WithRequired(e => e.Person);

            modelBuilder.Entity<Person>()
                .HasOptional(e => e.Buyer)
                .WithRequired(e => e.Person);

            modelBuilder.Entity<Person>()
                .HasOptional(e => e.Owner)
                .WithRequired(e => e.Person);

            modelBuilder.Entity<RealEstateCompany>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RealEstateCompany>()
                .HasMany(e => e.HomeSales)
                .WithRequired(e => e.RealEstateCompany)
                .WillCascadeOnDelete(false);
        }
    }
}
