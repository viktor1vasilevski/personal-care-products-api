using Core;
using EntityModels.Models;
using EntityModels.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context;

public class LibraryDbContext : IdentityDbContext, IDbContext
{
    private IHttpContextAccessor _httpContextAccessor;
    public LibraryDbContext() 
    {

    }
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options) 
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false)
                                .AddJsonFile($"appsetting.{envName}.json", optional: true);

        var conf = builder.Build();

        var connString = conf.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connString);
    }

    public virtual DbSet<Subcategory> Subcategory { get; set; }
    public virtual DbSet<Category> Category { get; set; }
    public virtual DbSet<Product> Product { get; set; }

    public override int SaveChanges()
    {
        // Get all the entities that inherit from AuditableEntity
        // and have a state of Added or Modified
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableBaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        // For each entity we will set the Audit properties
        foreach (var entityEntry in entries)
        {
            // If the entity state is Added let's set
            // the CreatedAt and CreatedBy properties
            if (entityEntry.State == EntityState.Added)
            {
                ((AuditableBaseEntity)entityEntry.Entity).Created = DateTime.UtcNow;
                ((AuditableBaseEntity)entityEntry.Entity).CreatedBy = this._httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }
            else
            {
                // If the state is Modified then we don't want
                // to modify the CreatedAt and CreatedBy properties
                // so we set their state as IsModified to false
                Entry((AuditableBaseEntity)entityEntry.Entity).Property(p => p.Created).IsModified = false;
                Entry((AuditableBaseEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
            }

            if(entityEntry.State == EntityState.Modified) 
            {
                ((AuditableBaseEntity)entityEntry.Entity).LastModified = DateTime.UtcNow;
                ((AuditableBaseEntity)entityEntry.Entity).LastModifiedBy = this._httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }


        }
        // After we set all the needed properties
        // we call the base implementation of SaveChanges
        // to actually save our entities in the database
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Subcategory>()
            .HasOne(x => x.Category)
            .WithMany(x => x.Subcategory)
            .HasForeignKey(x => x.CategoryId);

        builder.Entity<Product>()
            .HasOne(x => x.Subcategory)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.SubcategoryId);


        base.OnModelCreating(builder);
    }


}
