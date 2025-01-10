using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly TenantStoreDbContext _tenantStoreDbContext;

    public TenantController(TenantStoreDbContext tenantStoreDbContext)
    {
        _tenantStoreDbContext = tenantStoreDbContext;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTenant([FromBody] TenantInfo newTenant)
    {
        // Kiểm tra nếu tenant đã tồn tại
        var existingTenant = await _tenantStoreDbContext.TenantInfo.FirstOrDefaultAsync(t => t.Identifier == newTenant.Identifier);
        if (existingTenant != null)
        {
            return BadRequest(new { Message = "Tenant already exists." });
        }

        // Kiểm tra ConnectionString hợp lệ khi UseDefaultConnection = false
        if (!newTenant.UseDefaultConnection && string.IsNullOrWhiteSpace(newTenant.ConnectionString))
        {
            return BadRequest(new { Message = "ConnectionString must be provided if UseDefaultConnection is false." });
        }

        // Thêm tenant mới vào bảng Tenants
        _tenantStoreDbContext.TenantInfo.Add(newTenant);
        await _tenantStoreDbContext.SaveChangesAsync();

        // Tạo cơ sở dữ liệu mới nếu cần
        if (!newTenant.UseDefaultConnection && !string.IsNullOrEmpty(newTenant.ConnectionString))
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbConText>();
                optionsBuilder.UseSqlServer(newTenant.ConnectionString);

                using (var appDbContext = new AppDbConText(optionsBuilder.Options, newTenant))
                {
                    await appDbContext.Database.EnsureCreatedAsync(); // Tạo cơ sở dữ liệu nếu chưa tồn tại
                    await appDbContext.Database.MigrateAsync(); // Áp dụng migrations
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to create database or tables.", Error = ex.Message });
            }
        }

        return Ok(new { Message = "Tenant and database created successfully." });
    }


    [HttpGet("getTenants")]
    public async Task<IActionResult> GetTenants()
    {
        var tenants = await _tenantStoreDbContext.TenantInfo.ToListAsync();
        return Ok(tenants);
    }

    [HttpGet("getTenantById")]
    public async Task<IActionResult> GetTenantById(string id)
    {
        var tenant = await _tenantStoreDbContext.TenantInfo.FirstOrDefaultAsync(t => t.Id == id);
        return Ok(tenant);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTenant([FromBody] TenantInfo tenant)
    {
        _tenantStoreDbContext.TenantInfo.Update(tenant);
        await _tenantStoreDbContext.SaveChangesAsync();
        return Ok(new { Message = "Tenant updated successfully" });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteTenant(string id)
    {
        var tenant = await _tenantStoreDbContext.TenantInfo.FirstOrDefaultAsync(t => t.Id == id);
        if (tenant == null)
        {
            return NotFound(new { Message = "Tenant not found" });
        }
        _tenantStoreDbContext.TenantInfo.Remove(tenant);
        await _tenantStoreDbContext.SaveChangesAsync();
        return Ok(new { Message = "Tenant deleted successfully" });
    }
}
