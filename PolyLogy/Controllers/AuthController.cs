using Microsoft.AspNetCore.Mvc;
using PolyLogy.DTOs;
using PolyLogy.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto request)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { message = "Login successful", username = user.Username });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] LoginDto request)
    {
        if (_context.Users.Any(u => u.Username == request.Username))
            return BadRequest("User already exists");

        var user = new User
        {
            Username = request.Username,
            Password = request.Password
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("Registered successfully");
    }
}