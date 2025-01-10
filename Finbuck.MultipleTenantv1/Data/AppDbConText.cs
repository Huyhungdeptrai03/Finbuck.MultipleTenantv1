using Microsoft.EntityFrameworkCore;
using Finbuckle.MultiTenant;
using Finbuck.MultipleTenantv1.Entiti;
using Finbuckle.MultiTenant.Abstractions;

public class AppDbConText : DbContext
{
    private readonly TenantInfo _tenantInfo;

    public DbSet<User> Users { get; set; }

    public AppDbConText(DbContextOptions<AppDbConText> options, ITenantInfo tenantInfo) : base(options)
    {
        _tenantInfo = tenantInfo as TenantInfo;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_tenantInfo != null && !string.IsNullOrEmpty(_tenantInfo.ConnectionString) && !_tenantInfo.UseDefaultConnection)
        {
            optionsBuilder.UseSqlServer(_tenantInfo.ConnectionString);
        }
        else
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-NDACBFQ\\SQLEXPRESS01;Initial Catalog=DefaultTenantDB;Integrated Security=True;Trust Server Certificate=True");
        }

        base.OnConfiguring(optionsBuilder);
    }
}
