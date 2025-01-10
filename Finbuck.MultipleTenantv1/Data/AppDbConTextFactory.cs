using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbConTextFactory : IDesignTimeDbContextFactory<AppDbConText>
{
    public AppDbConText CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbConText>();
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-NDACBFQ\\SQLEXPRESS01;Initial Catalog=DefaultTenantDB;Integrated Security=True;Trust Server Certificate=True");

        return new AppDbConText(optionsBuilder.Options, new TenantInfo
        {
            Id = "default",
            Identifier = "default",
            Name = "Default Tenant",
            ConnectionString = "Data Source=DESKTOP-NDACBFQ\\SQLEXPRESS01;Initial Catalog=DefaultTenantDB;Integrated Security=True;Trust Server Certificate=True",
            UseDefaultConnection = true
        });
    }
}
