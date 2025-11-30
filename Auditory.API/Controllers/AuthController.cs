using System.Security.Cryptography;
using Auditory.Application.DTO.Auth;
using Auditory.Domain.Entities;
using Auditory.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest;
using RegisterRequest = Microsoft.AspNetCore.Identity.Data.RegisterRequest;

namespace Auditory.API.Controllers;

public class AuthController : ControllerBase
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<RefreshToken> _refreshTokens;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    
    public AuthController(
        IMongoDatabase db,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _users = db.GetCollection<User>("users");
        _refreshTokens = db.GetCollection<RefreshToken>("refreshTokens");
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existing = await _users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
        if (existing != null)
            return BadRequest("Email already in use.");

        _passwordHasher.CreatePasswordHash(request.Password,
            out var hash, out var salt);

        var user = new User
        {
            Id = ObjectId.GenerateNewId(),
            Email = request.Email.Trim().ToLower(),
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        await _users.InsertOneAsync(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _users.Find(u => u.Email.Equals(request.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync();
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized("Invalid credentials.");

        var accessToken = _jwtService.GenerateAccessToken(user);

        var refreshToken = new RefreshToken
        {
            Id = ObjectId.GenerateNewId(),
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = HttpContext.Connection.RemoteIpAddress?.ToString()
        };

        await _refreshTokens.InsertOneAsync(refreshToken);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            UserId = user.Id.ToString(),
            Email = user.Email,
            Username = user.Username,
            Role = user.Role
        };

        return Ok(response);
    }
}