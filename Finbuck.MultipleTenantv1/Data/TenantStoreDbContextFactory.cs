using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class TenantStoreDbContextFactory : IDesignTimeDbContextFactory<TenantStoreDbContext>
{
    public TenantStoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantStoreDbContext>();
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-NDACBFQ\\SQLEXPRESS01;Initial Catalog=MasterTenantDB;Integrated Security=True;Trust Server Certificate=True");

        return new TenantStoreDbContext(optionsBuilder.Options);
    }
}
