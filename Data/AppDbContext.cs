using KeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KeeperApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<VisitorActionHistory> ActionHistories => Set<VisitorActionHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Visitor>(e =>
        {

            e.HasKey(x => x.Id);
            e.HasMany(x => x.History)
             .WithOne() 
             .HasForeignKey(s => s.VisitorId) 
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
