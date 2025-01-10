using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure TenantStoreDbContext
builder.Services.AddDbContext<TenantStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MasterDatabase")));

builder.Services.AddScoped<ITenantInfo, TenantInfo>();

// Add Multi-Tenant Services
builder.Services.AddMultiTenant<TenantInfo>()
    .WithEFCoreStore<TenantStoreDbContext, TenantInfo>() // Dùng EF Core để lưu tenant
    .WithRouteStrategy(); // Xác định tenant qua route

// Configure AppDbContext
builder.Services.AddDbContext<AppDbConText>((serviceProvider, options) =>
{
    var tenantInfo = serviceProvider.GetRequiredService<ITenantInfo>() as TenantInfo;

    if (tenantInfo != null && !string.IsNullOrEmpty(tenantInfo.ConnectionString) && !tenantInfo.UseDefaultConnection)
    {
        options.UseSqlServer(tenantInfo.ConnectionString);
    }
    else
    {
        options.UseSqlServer("Data Source=DESKTOP-NDACBFQ\\SQLEXPRESS01;Initial Catalog=DefaultTenantDB;Integrated Security=True;Trust Server Certificate=True");
    }
});

// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Use Multi-Tenant middleware
app.UseMultiTenant();

app.MapControllers();

app.Run();
