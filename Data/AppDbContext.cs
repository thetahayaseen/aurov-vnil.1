using Microsoft.EntityFrameworkCore;
using mark_vnil._1.Models;

namespace mark_vnil._1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ROVDetectedItem>().HasIndex(o => o.Label);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<ROVStream> ROVStreams { get; set; }
    public DbSet<ROVDetectedItem> ROVDetectedItems { get; set; }
}