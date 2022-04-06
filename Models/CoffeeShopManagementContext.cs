using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace CoffeeShopManagement.Models
{
    public partial class CoffeeShopManagementContext : DbContext
    {
        public CoffeeShopManagementContext()
        {
        }

        public CoffeeShopManagementContext(DbContextOptions<CoffeeShopManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillInfo> BillInfos { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<FoodCategory> FoodCategories { get; set; }
        public virtual DbSet<TableFood> TableFoods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("CoffeeShopDB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK__Account__66DCF95D3ABF75BE");

                entity.ToTable("Account");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("userName");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.DateOfBirth)
                    .HasMaxLength(100)
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("fullName");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("passWord");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.BillId).HasColumnName("billId");

                entity.Property(e => e.DateCheckIn)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateCheckOut).HasColumnType("date");

                entity.Property(e => e.IdTable).HasColumnName("idTable");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.IdTableNavigation)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.IdTable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bill__idTable__440B1D61");
            });

            modelBuilder.Entity<BillInfo>(entity =>
            {
                entity.ToTable("BillInfo");

                entity.Property(e => e.BillInfoId).HasColumnName("billInfoId");

                entity.Property(e => e.IdBill).HasColumnName("idBill");

                entity.Property(e => e.IdFood).HasColumnName("idFood");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.IdBillNavigation)
                    .WithMany(p => p.BillInfos)
                    .HasForeignKey(d => d.IdBill)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillInfo__idBill__45F365D3");

                entity.HasOne(d => d.IdFoodNavigation)
                    .WithMany(p => p.BillInfos)
                    .HasForeignKey(d => d.IdFood)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillInfo__idFood__44FF419A");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("Food");

                entity.Property(e => e.FoodId).HasColumnName("foodId");

                entity.Property(e => e.FoodName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("foodName");

                entity.Property(e => e.IdCategory).HasColumnName("idCategory");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.IdCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Food__idCategory__46E78A0C");
            });

            modelBuilder.Entity<FoodCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__FoodCate__23CAF1D8D82AE09F");

                entity.ToTable("FoodCategory");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("categoryName");
            });

            modelBuilder.Entity<TableFood>(entity =>
            {
                entity.HasKey(e => e.TableId)
                    .HasName("PK__TableFoo__5408AD9AF619DFC9");

                entity.ToTable("TableFood");

                entity.Property(e => e.TableId).HasColumnName("tableId");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("tableName");

                entity.Property(e => e.TableStatus)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("tableStatus");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
