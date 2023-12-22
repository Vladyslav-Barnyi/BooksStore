using System.Text.Json;
using BooksStoreEntities.Entities;
using BooksStoreEntities.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooksStoreEntities;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        #region OneToOne
        
        // 1-1 User - ShoppingCart
        
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.ShoppingCart)
            .WithOne(sc => sc.ApplicationUser)
            .HasForeignKey<ShoppingCart>(u => u.UserId)
            .IsRequired();
        
        // 1-1 CartItem - Book 
        
        builder.Entity<CartItem>()
            .HasOne(ci => ci.Book)
            .WithMany(b => b.CartItems)
            .HasForeignKey(ci=>ci.BookId)
            .IsRequired();

        #endregion

        #region OneToMany

        // 1-M User - Orders

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .IsRequired();
        
        // 1-M ShoppingCart - CartItems
        
        builder.Entity<ShoppingCart>()
            .HasMany(sc => sc.CartItems)
            .WithOne(ci => ci.ShoppingCart)
            .HasForeignKey(ci => ci.ShoppingCartId)
            .IsRequired();
  
        #endregion

        #region ManyToMany

        builder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity<BooksAuthors>();
        
        builder.Entity<Book>()
            .HasMany(b => b.Genres)
            .WithMany(a => a.Books)
            .UsingEntity<BooksGenres>();

        #endregion

    }
    
    public override int SaveChanges()
    {
        PerformAudit();
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        PerformAudit();
        return base.SaveChangesAsync(ct);
    }
    
    private void PerformAudit()
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);
        
        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            var now = DateTime.UtcNow;

            var actionType = string.Empty;
            switch (entry.State)
            {
                case EntityState.Added:
                    actionType = nameof(EntityState.Added);
                    entity.CreatedAt = now;
                    break;
                case EntityState.Modified:
                    actionType = nameof(EntityState.Modified);
                    entity.UpdatedAt = now;
                    break;
            }

            var auditLog = new
            {
                EntityId = entity.Id,
                EntityName = entity.GetType().Name,
                ActionType = actionType,
                ActionDate = now
            };

            Log.Information(JsonSerializer.Serialize(auditLog));
        }
    }
}