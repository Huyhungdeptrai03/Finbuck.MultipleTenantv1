using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class TenantService
{
    private readonly HttpClient _httpClient;

    public TenantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TenantInfo>> GetAllTenantsAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:7109/api/Tenant/getTenants"); // Địa chỉ API của backend
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<TenantInfo>>(content);
    }

    public async Task<bool> CreateTenantAsync(TenantInfo tenant)
    {
        var json = JsonConvert.SerializeObject(tenant);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://localhost:7109/api/Tenant/create", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<TenantInfo> GetTenantByIdAsync(string tenantId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7109/api/Tenant/getTenants/{tenantId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TenantInfo>(content);
    }

    //public async Task<List<TenantInfo>> GetTenants()
    //{
    //    var response = await _httpClient.GetAsync("https://localhost:7109/api/Tenant/getTenants");
    //    response.EnsureSuccessStatusCode();
    //    var content = await response.Content.ReadAsStringAsync();
    //    return JsonConvert.DeserializeObject<List<TenantInfo>>(content);
    //}
}
