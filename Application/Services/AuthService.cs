using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Core.Models;
using Core.Interfaces;
using Core.AuthModels;
using Microsoft.Extensions.Configuration;
using Core.AuthDto;
using AutoMapper;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _jwtSecret;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtSecret = configuration["Jwt:Secret"];
        _mapper = mapper;
    }

    public async Task<AuthResponse> AuthenticateAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

        if (user == null || !VerifyPassword(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }


        var token = GenerateJwtToken(user);
        var mappedUser = _mapper.Map<AuthResponseDto>(user);
        return new AuthResponse
        {
            Token = token,
            User = mappedUser
        };
    }

    private bool VerifyPassword(string inputPassword, string storedPasswordHash)
    {
        return inputPassword == storedPasswordHash;
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal DecodeJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecret))
            };

            // Validate token and get principal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            return principal;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to decode token.", ex);
        }
    }

}
