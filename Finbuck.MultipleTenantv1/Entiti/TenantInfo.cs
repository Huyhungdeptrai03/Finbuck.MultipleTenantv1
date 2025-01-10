using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;

public class TenantInfo : ITenantInfo
{
    public string Id { get; set; }
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string ConnectionString { get; set; } // Dùng database riêng
    public bool UseDefaultConnection { get; set; } = true; // Mặc định dùng database chung
}
