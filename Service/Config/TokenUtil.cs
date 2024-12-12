using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;
using Microsoft.IdentityModel.Tokens;
using Service.Helper;

namespace Service.Config
{
    public class TokenUtil
    {
        public static Task<string> GenerateAccessToken(DtoAuthentication user)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppSettings.GetAccessJwtSecret())
            );
            
            List<Claim> claims =
            [
                new ("id", user.id.ToString()),
                new (ClaimTypes.Role, user.role.ToString())
            ];
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AppSettings.GetOriginIssuer(),
                Audience = AppSettings.GetOriginAudience(),
                Expires = DateTime.UtcNow.AddHours(24),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public static Task<string>  GenerateRefreshToken(DtoAuthentication user)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppSettings.GetRefreshJwtSecret())
            );
            
            List<Claim> claims =
            [
                new ("id", user.id.ToString()),
                new ("username", user.username),
                new (ClaimTypes.Role, user.role.ToString()),
                new ("status", user.status.ToString())
            ];
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AppSettings.GetOriginIssuer(),
                Audience = AppSettings.GetOriginAudience(),
                Expires = DateTime.UtcNow.AddDays(7),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public static async Task<(Tokens, DtoMessage)> GenerateAccessTokenFromRefreshToken(string refreshToken, string secret)
        {
            Tokens tokens = new Tokens();
            DtoMessage message = new ();
            try
            {
                (ClaimsPrincipal primary, bool isExpired) = await ValidateToken(refreshToken, secret);
                if (isExpired)
                {
                    message.ListMessage.Add("El token de actualización ha expirado.");
                    return (tokens, message);
                }
                if (primary == null)
                {
                    message.ListMessage.Add("Token de actualización no recibido o no válido.");
                    return (tokens, message);
                }
                Claim userClaim = primary.Claims.FirstOrDefault(c => c.Type == "id");
                if (userClaim == null)
                {
                    message.ListMessage.Add("Token de actualización no válido.");
                    return (tokens, message);
                }
                DtoAuthentication dtoUser = new DtoAuthentication
                {
                    id = Guid.Parse(userClaim.Value),
                    role = (Hierarchy)Enum.Parse(typeof(Hierarchy), primary.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)
                };
                message.Success();
                tokens.accessToken = await GenerateAccessToken(dtoUser);
                tokens.refreshToken = refreshToken;
                return (tokens, message);
            }
            catch (SecurityTokenException ex)
            {
                message.ListMessage.Add(ex.Message);
                return (tokens, message);
            }
        }

        private static Task<(ClaimsPrincipal?, bool isExpired)> ValidateToken(string token, string secret)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(secret);
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = AppSettings.GetOriginIssuer(),
                ValidAudience = AppSettings.GetOriginAudience(),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                ClaimsPrincipal primary = tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out SecurityToken validatedToken
                );
                bool isExpired = validatedToken is JwtSecurityToken jwtToken && jwtToken.ValidTo < DateTime.UtcNow;
                return Task.FromResult(
                    (validatedToken is JwtSecurityToken jwt && jwt.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) && !isExpired
                        ? primary
                        : null, isExpired)
                );
            }
            catch (SecurityTokenExpiredException)
            {
                return Task.FromResult<(ClaimsPrincipal, bool)>((null, true));
            }
            catch
            {
                return Task.FromResult<(ClaimsPrincipal, bool)>((null, false));
            }
        }

        public static string GetUserIdFromAccessToken(string accessToken)
        {
            String token = accessToken.Replace("Bearer ", "");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken? jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            Claim? accessClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "id");
            return accessClaim?.Value ?? string.Empty;
        }
    }
}
