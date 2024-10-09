using Core;
using EntityModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context;

public class LibraryDbContext : IdentityDbContext, IDbContext
{
    public LibraryDbContext() { }
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

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
