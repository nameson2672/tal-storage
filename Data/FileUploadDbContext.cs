using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;
using TalStorage.Models;

//public class FileUploadDbContext : IdentityDbContext<ApplicationUser>
//{
//    private readonly IHttpContextAccessor _httpContextAccessor;
//    public FileUploadDbContext(DbContextOptions<FileUploadDbContext> options, IHttpContextAccessor httpContextAccessor)
//            : base(options)
//    {
//        _httpContextAccessor = httpContextAccessor;
//    }

//    public DbSet<FileRecord> FileRecords { get; set; }
//    public DbSet<FileShareRecord> FileShareRecords { get; set; }
//    protected override void OnModelCreating(ModelBuilder builder)
//    {
//        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

//        base.OnModelCreating(builder);
//        builder.Entity<ApplicationUser>().HasQueryFilter(e => e.IsDeleted == false);

//        builder.Entity<FileRecord>()
//               .HasMany<FileShareRecord>(s => s.FilesSharedWith)
//               .WithOne()
//               .OnDelete(DeleteBehavior.NoAction);

//    }
//    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        {
//            var entries = ChangeTracker
//            .Entries()
//                .Where(e => e.Entity is IBaseEntity &&
//                            (e.State == EntityState.Added || e.State == EntityState.Modified));

//            foreach (var entry in entries)
//            {
//                var entity = (IBaseEntity)entry.Entity;

//                if (entry.State == EntityState.Added)
//                {
//                    entity.CreatedAt = DateTime.UtcNow;
//                }

//            entity.UpdatedAt = DateTime.UtcNow;
//                // entity.UpdatedBy = _httpContextAccessor.HttpContext?.Items["UpdatedBy"]?.ToString() ?? "system";
//                entity.UpdatedBy = "system";
//            }

//            return await base.SaveChangesAsync(cancellationToken);
//        }
//}

public class FileUploadDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public DbSet<FileRecord> FileRecords { get; set; }
    public DbSet<FileShareRecord> FileShareRecords { get; set; }
    public FileUploadDbContext(DbContextOptions<FileUploadDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().HasQueryFilter(e => e.IsDeleted == false);
        builder.Entity<FileShareRecord>()
                  .HasOne(fs => fs.File)  // FileShareRecord has one FileRecord
                  .WithMany(fr => fr.FilesSharedWith) // FileRecord has many FileShareRecords
                  .HasForeignKey(fs => fs.FileId) // Define the foreign key explicitly
                  .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete issues

    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
        .Entries()
            .Where(e => e.Entity is IBaseEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (IBaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            entity.UpdatedAt = DateTime.UtcNow;
            // entity.UpdatedBy = _httpContextAccessor.HttpContext?.Items["UpdatedBy"]?.ToString() ?? "system";
            entity.UpdatedBy = "system";
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

