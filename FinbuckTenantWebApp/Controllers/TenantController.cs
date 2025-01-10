using Microsoft.AspNetCore.Mvc;

public class TenantController : Controller
{
    private readonly TenantService _tenantService;

    public TenantController(TenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tenants = await _tenantService.GetAllTenantsAsync();
        return View(tenants); // Truyền danh sách tenants sang view
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(); // Trả về form tạo mới tenant
    }

    [HttpPost]
    public async Task<IActionResult> Create(TenantInfo tenant)
    {
        var result = await _tenantService.CreateTenantAsync(tenant);
        if (result)
        {
            return RedirectToAction("Index"); // Quay lại danh sách tenants nếu thành công
        }

        ModelState.AddModelError("", "Failed to create tenant.");
        return View(tenant);
    }

    //swich tenant
    [HttpGet]
    public async Task<IActionResult> Switch(string tenantId)
    {
        var tenant = await _tenantService.GetTenantByIdAsync(tenantId);
        if (tenant == null)
        {
            return NotFound();
        }
        // Lưu thông tin tenant vào session
        HttpContext.Session.SetString("TenantId", tenant.Identifier);
        HttpContext.Session.SetString("ConnectionString", tenant.ConnectionString);
        return RedirectToAction("Index", "User", new { tenantId = tenant.Identifier });
    }

    //public IActionResult TenantSelector()
    //{
    //    var tenants = ; // Assume GetTenants() fetches the list of tenants
    //    ViewBag.Tenants = tenants;
    //    return View();
    //}

}
