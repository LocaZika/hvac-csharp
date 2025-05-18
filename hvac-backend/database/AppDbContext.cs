using hvac_backend.global.transfomers;
using hvac_backend.products.entities;
using hvac_backend.users.customers.entities;
using Microsoft.EntityFrameworkCore;

namespace hvac_backend.database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
  //---| Product DbSet |---//
  public required DbSet<Product> Products { get; set; }
  public required DbSet<ProductImg> ProductImgs { get; set; }
  public required DbSet<ProductImg> ProductDetailImgs { get; set; }

  //---| Customer DbSet |---//
  public required DbSet<Customer> Customers { get; set; }
  // Override
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    //---| Product |---//
    modelBuilder.Entity<Product>(entity => {
      entity.ToTable("cars", "products");
      entity.HasKey(p => p.Id);
      entity.Property(p => p.Id).UseIdentityAlwaysColumn();
      entity.Property(p => p.Name).HasMaxLength(50);
      entity.Property(p => p.Brand).HasMaxLength(15);
      entity.Property(p => p.Price).HasColumnType("numeric(10,2)");
      entity.Property(p => p.Type).HasMaxLength(10);
      entity.Property(p => p.Vin).HasMaxLength(12);
      entity.Property(p => p.Stock).HasMaxLength(12);
      entity.Property(p => p.Created_at).HasColumnType("timestamptz").HasDefaultValueSql("now()");
      entity.Property(p => p.Updated_at).HasColumnType("timestamptz").HasDefaultValueSql("now()");
      // Khóa ngoại
      entity.HasMany(p => p.Imgs)
        .WithOne(i => i.Product)
        .HasForeignKey(i => i.ProductId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired(false)
        .HasConstraintName("FK_PRODUCT_IMGS");
      entity.HasMany(p => p.DetailImgs)
        .WithOne(di => di.Product)
        .HasForeignKey(di => di.ProductId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired(false)
        .HasConstraintName("FK_PRODUCT_DETAIL_IMGS");
    });
    // Product Imgs //
    modelBuilder.Entity<ProductImg>(entity => {
      entity.ToTable("imgs", "products");
      entity.HasKey(pi => pi.Id);
      entity.Property(pi => pi.Id).UseIdentityAlwaysColumn();
      entity.Property(pi => pi.Alt).HasMaxLength(15);
    });
    modelBuilder.Entity<ProductDetailImg>(entity => {
      entity.ToTable("detailImgs", "products");
      entity.HasKey(pdi => pdi.Id);
      entity.Property(pdi => pdi.Id).UseIdentityAlwaysColumn();
      entity.Property(pdi => pdi.Alt).HasMaxLength(15);
    });
    //---| Users |---//
    //---| Customers |---//
    modelBuilder.Entity<Customer>(entity => {
      entity.ToTable("customers", "users");
      entity.HasKey(c => c.Id);
      entity.Property(c => c.Id).UseIdentityAlwaysColumn();
      entity.Property(c => c.Email).HasMaxLength(30);
      entity.Property(c => c.Password).HasMaxLength(20);
      entity.Property(c => c.Phone).HasMaxLength(12);
      entity.Property(c => c.CodeExpire).HasColumnType("timestamptz");
      entity.Property(c => c.Created_at).HasColumnType("timestamptz").HasDefaultValueSql("now()");
      entity.Property(c => c.Updated_at).HasColumnType("timestamptz").HasDefaultValueSql("now()");
    });

  }
}
