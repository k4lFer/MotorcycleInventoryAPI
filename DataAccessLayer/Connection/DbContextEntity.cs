using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entity;

namespace DataAccessLayer.Connection
{
    public partial class DataBaseContext
    {
        private void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>().ToTable("owners");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Brand>().ToTable("brands");
            modelBuilder.Entity<Types>().ToTable("types");
            modelBuilder.Entity<Motorcycle>().ToTable("motorcycles");
            modelBuilder.Entity<Service>().ToTable("services");
            modelBuilder.Entity<Sales>().ToTable("sales");
            modelBuilder.Entity<SalesService>().ToTable("sales_services");
            modelBuilder.Entity<SalesDetails>().ToTable("sales_details");

            ConfigureEntityRelationships(modelBuilder);
        }
        
        public DbSet<Owner> Owners { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<Motorcycle> Motorcyles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesService> SalesServices { get; set; }
        public DbSet<SalesDetails> SalesDetails { get; set; }
    }
}