using Microsoft.EntityFrameworkCore;
using TesteImpacta.API.Models;

namespace TesteImpacta.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>()
            .Property(x => x.Titulo)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<TaskItem>()
            .Property(x => x.Status)
            .HasMaxLength(20);
    }
}