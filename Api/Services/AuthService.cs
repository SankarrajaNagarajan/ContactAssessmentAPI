using ContactApi.Domain.DTOs;
using ContactApi.Domain.Models;
using Data.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ContactApi.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        _logger.LogInformation("Login attempt for user: {Username}", loginDto.Username);

        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null || user.PasswordHash != loginDto.Password)
        {
            _logger.LogWarning("Failed login for: {Username}", loginDto.Username);
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var token = GenerateJwtToken(user);
        _logger.LogInformation("User {Username} logged in successfully", user.Username);

        return new LoginResponseDto
        {
            Token = token,
            Expires = DateTime.UtcNow.AddHours(1)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
