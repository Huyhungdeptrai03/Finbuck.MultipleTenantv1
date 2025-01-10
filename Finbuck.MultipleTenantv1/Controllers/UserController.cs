using Finbuck.MultipleTenantv1.Entiti;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/{tenantId}/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbConText _dbContext;

    public UserController(AppDbConText dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return Ok(new { Message = "User created successfully" });
    }

    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("getUserById")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        return Ok(user);
    }

    [HttpPut("updateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return Ok(new { Message = "User updated successfully" });
    }

    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return Ok(new { Message = "User deleted successfully" });
    }
}
