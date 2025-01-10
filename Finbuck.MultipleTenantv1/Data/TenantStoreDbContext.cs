using Microsoft.EntityFrameworkCore;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;

public class TenantStoreDbContext : EFCoreStoreDbContext<TenantInfo>
{
    public TenantStoreDbContext(DbContextOptions<TenantStoreDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TenantInfo>().ToTable("Tenants");
        modelBuilder.Entity<TenantInfo>().HasKey(t => t.Id);
    }
}
