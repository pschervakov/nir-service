using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Mephi.Cyber.AuthService.Core.Utils
{
	public class Token
	{
		const string claimType = "username";
		public static string GenerateToken(string username, string mySecret, int expireSec=3600)
		{
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
			new Claim(claimType, username),
				}),
				Expires = System.DateTime.UtcNow.AddSeconds(expireSec),
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
		public static  bool ValidateCurrentToken(string token, string mySecret)
		{
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    IssuerSigningKey = mySecurityKey
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}
		public static bool ValidateRsaCurrentToken(string token, string key)
		{
			var rsa = RSA.Create();

			rsa.ImportSubjectPublicKeyInfo(source: Convert.FromBase64String(key), bytesRead: out int _);
			var mySecurityKey = new RsaSecurityKey(rsa);
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				_ = tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,


					IssuerSigningKey = mySecurityKey
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static  string GetClaim(string token, string claimType)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadJwtToken(token);
			var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
			return stringClaimValue;
		}
		public static string GenerateSharedSecret()
		{
			var secret = RandomPasswordGenerator.GeneratePassword(passwordSize: 130);
			return secret;
		}
		public static string GenerateAuthSecret()
		{
			var secret = RandomPasswordGenerator.GeneratePassword(passwordSize: 15);
			return secret;
		}
	}
}

