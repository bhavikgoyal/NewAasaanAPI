using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aasaan_API.Security
{
  public class TokenManager
  {
    private const string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
    public static string GenerateToken(string UserName, string Password, Int32 RoleId, Int32 ID)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("Name", UserName.ToString()), new Claim("Password", Password.ToString()), new Claim("RoleId", RoleId.ToString()), new Claim("USID", ID.ToString()) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public static string GenerateTokens(string Mobile, string DeviceId, Int32 RoleId, Int32 ID)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("Name", Mobile.ToString()), new Claim("Password", DeviceId.ToString()), new Claim("RoleId", RoleId.ToString()), new Claim("USID", ID.ToString()) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public static ClaimsPrincipal GetPrincipal(string token)
    {
      try
      {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
        if (jwtToken == null) return null;
        byte[] key = Convert.FromBase64String(Secret);
        TokenValidationParameters parameters = new TokenValidationParameters()
        {
          RequireExpirationTime = true,
          ValidateIssuer = false,
          ValidateAudience = false,
          IssuerSigningKey = new SymmetricSecurityKey(key)
        };
        SecurityToken securityToken;
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
        return principal;
      }
      catch
      {
        return null;
      }
    }
  }
}
