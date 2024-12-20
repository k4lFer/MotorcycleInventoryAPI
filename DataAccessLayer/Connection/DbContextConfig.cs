using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Entity;
using DataTransferLayer.OtherObject;

namespace DataAccessLayer.Connection
{
    public partial class DataBaseContext
    {
        private void ConfigureEntityRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.role)
                    .IsRequired()
                    .HasConversion(v => v.ToString(), v => (Hierarchy)Enum.Parse(typeof(Hierarchy), v))
                    .HasColumnType("enum('Admin','Manager')");

                entity.HasMany(e => e.ChildSales)
                    .WithOne(e => e.ParentOwner)
                    .HasForeignKey(e => e.ownerId)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasMany(e => e.ChildSales)
                    .WithOne(e => e.ParentUser)
                    .HasForeignKey(e => e.userId)
                    .IsRequired();
            });

            modelBuilder.Entity<Types>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasMany(e => e.ChildMotorcycle)
                    .WithOne(e => e.ParentTypes)
                    .HasForeignKey(e => e.typeId)
                    .IsRequired();
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasMany(e => e.ChildMotorcycle)
                    .WithOne(e => e.ParentBrands)
                    .HasForeignKey(e => e.brandId)
                    .IsRequired();
            });

            modelBuilder.Entity<Motorcycle>(entity =>
            {
                entity.HasKey(e => e.id);
                
                entity.Property(e => e.status)
                    .IsRequired()
                    .HasConversion(v => v.ToString(), v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v))
                    .HasColumnType("enum('available','not_available')")
                    .IsRequired();
                
                entity.HasOne(e => e.ParentBrands)
                    .WithMany(e => e.ChildMotorcycle)
                    .HasForeignKey(e => e.brandId)
                    .IsRequired();
                
                entity.HasOne(e => e.ParentTypes)
                    .WithMany(e => e.ChildMotorcycle)
                    .HasForeignKey(e => e.typeId)
                    .IsRequired();
                
                entity.HasMany(e => e.ChildSalesDetails)
                    .WithOne(e => e.ParentMotorcyle)
                    .HasForeignKey(e => e.motorcycleId)
                    .IsRequired();
            });

            modelBuilder.Entity<Sales>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasOne(e => e.ParentOwner)
                    .WithMany(e => e.ChildSales)
                    .HasForeignKey(e => e.ownerId)
                    .IsRequired();
                entity.HasOne(e => e.ParentUser)
                    .WithMany(e => e.ChildSales)
                    .HasForeignKey(e => e.userId)
                    .IsRequired();
                entity.HasMany(e => e.ChildSalesDetails)
                    .WithOne(e => e.ParentSales)
                    .HasForeignKey(e => e.saleId)
                    .IsRequired();
                entity.HasMany(e => e.ChildSalesServices)
                    .WithOne(e => e.ParentSales)
                    .HasForeignKey(e => e.saleId)
                    .IsRequired();

            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasMany(e => e.ChildSalesServices)
                    .WithOne(e => e.ParentService)
                    .HasForeignKey(e => e.serviceId)
                    .IsRequired();
            });

            modelBuilder.Entity<SalesService>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasOne(e => e.ParentSales)
                    .WithMany(e => e.ChildSalesServices)
                    .HasForeignKey(e => e.saleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.ParentService)
                    .WithMany(e => e.ChildSalesServices)
                    .HasForeignKey(e => e.serviceId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SalesDetails>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasOne(e => e.ParentSales)
                    .WithMany(e => e.ChildSalesDetails)
                    .HasForeignKey(e => e.saleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.ParentMotorcyle)
                    .WithMany(e => e.ChildSalesDetails)
                    .HasForeignKey(e => e.motorcycleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
