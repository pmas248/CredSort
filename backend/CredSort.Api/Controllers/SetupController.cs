using CredSort.Api.Data;
using CredSort.Api.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SetupController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SetupController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        // 1. Create Tenants (Firms)
        var fletcher = new Tenant { Name = "Fletcher Construction" };
        var hawkins = new Tenant { Name = "Hawkins" };

        _context.Tenants.AddRange(fletcher, hawkins);
        await _context.SaveChangesAsync();

        // 2. Create Users (Directly in DB to bypass the TenantProvider filter for seeding)
        // We manually set the TenantId here for the initial setup
        var user1 = new User { FirstName = "John", LastName = "Fletcher", Email = "john@fletcher.co.nz", TenantId = fletcher.Id };
        var user2 = new User { FirstName = "Sarah", LastName = "Hawkins", Email = "sarah@hawkins.co.nz", TenantId = hawkins.Id };

        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Seeded!",
            fletcherId = fletcher.Id,
            hawkinsId = hawkins.Id
        });
    }
}