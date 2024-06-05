using Core;
using EntityModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class LibraryDbContext : IdentityDbContext, IDbContext
{
    public LibraryDbContext() { }
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public virtual DbSet<Subcategory> Subcategory { get; set; }
    public virtual DbSet<Category> Category { get; set; }

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
