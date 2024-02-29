using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Services;

public class AuthManager : IAuthManager
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;
    private ApiUser _user;
    public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions= GenerazeTokenOptions(signingCredentials, claims);
       
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private JwtSecurityToken GenerazeTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.
            GetSection("lifetime").Value));
        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetSection("Issuer").Value,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
            ) ;
        return token;
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name, _user.UserName)
    };

        var roles = await _userManager.GetRolesAsync(_user);

         foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = _configuration.GetSection("Jwt").GetSection("Key").Value;
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new SigningCredentials(secret,SecurityAlgorithms.HmacSha256);
    }

    public async Task<bool> ValidateUser(LoginUserDTO userDTO)
    {
       _user = await _userManager.FindByNameAsync(userDTO.Email);
        return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
    }
}
