using Microsoft.EntityFrameworkCore;

public class FileUploadDbContext : DbContext
{
    public FileUploadDbContext(DbContextOptions<FileUploadDbContext> options) : base(options) { }

    public DbSet<FileRecord> FileRecords { get; set; }
    public DbSet<FileShareRecord> FileShareRecords { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                var now = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedAt = now;
                        baseEntity.UpdatedAt = now;
                        break;
                    case EntityState.Modified:
                        baseEntity.UpdatedAt = now;
                        break;
                }
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
