using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data;

/// <summary>
/// Contexto do banco de dados - Equivalente ao Eloquent ORM do Laravel
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets - Equivalente aos Models do Laravel
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da tabela Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Password).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique(); // Email único
        });

        // Configuração da tabela Products
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Image).HasMaxLength(500);
        });

        // Configuração da tabela Favorites (relacionamento N:N entre User e Product)
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Relacionamento com User
            entity.HasOne(e => e.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Product
            entity.HasOne(e => e.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice único: um usuário não pode favoritar o mesmo produto duas vezes
            entity.HasIndex(e => new { e.UserId, e.ProductId }).IsUnique();
        });
    }
}
