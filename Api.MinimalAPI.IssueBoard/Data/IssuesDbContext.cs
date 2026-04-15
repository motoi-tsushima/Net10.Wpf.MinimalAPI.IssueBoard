using Api.MinimalAPI.IssueBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.MinimalAPI.IssueBoard.Data;

public partial class IssuesDbContext : DbContext
{
    public IssuesDbContext(DbContextOptions<IssuesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Issue> Issues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AuthorName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(30);
            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000).IsRequired();
            entity.Property(e => e.Resolution).HasMaxLength(2000);
            entity.Property(e => e.ResolverName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
