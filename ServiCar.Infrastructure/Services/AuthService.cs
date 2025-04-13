using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Servicar.Domain.DTOs;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
using ServiCar.Domain.Generics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Servicar.Infrastruture.Services
{
    public interface IAuthService
    {
        Task<Result<string, ErrorDTO>> Register(UserRegisterDTO model);
        Task<Result<LoginResponseDTO, ErrorDTO>> Login(LoginDTO model);
        (bool, dynamic) Refresh(TokenApiDTO tokenApiModel);
        bool Revoke(string username);
        Task<Result<string, ErrorDTO>> SwitchToWorkerAccount(int userId);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponseDTO, ErrorDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "User not found."
                };

                return Result<LoginResponseDTO, ErrorDTO>.Fail(error);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Wrong credentials."
                };

                return Result<LoginResponseDTO, ErrorDTO>.Fail(error);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var tokenData = GenerateAccessToken(authClaims);

            var response = new LoginResponseDTO
            {
                AccessToken = tokenData.AccessToken,
                tokenExpiry = tokenData.TokenExpiry,
                User = new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsCompanyWorker = user.IsCompanyWorker,
                    Role = userRoles.FirstOrDefault() ?? string.Empty
                }
            };

            return Result<LoginResponseDTO, ErrorDTO>.Success(response);
        }

        public async Task<Result<string, ErrorDTO>> Register(UserRegisterDTO model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "User already exists."
                };

                return Result<string, ErrorDTO>.Fail(error);
            }

            var newUser = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                IsCompanyWorker = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "User creation failed. Please check user details and try again."
                };

                return Result<string, ErrorDTO>.Fail(error);
            }

            IdentityResult roleAssignResult = await _userManager.AddToRoleAsync(newUser, "User");

            if (!roleAssignResult.Succeeded)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = roleAssignResult.Errors.FirstOrDefault()?.Description ?? "Failed to assign role."
                };

                return Result<string, ErrorDTO>.Fail(error);
            }

            return Result<string, ErrorDTO>.Success("User created successfully.");
        }

        private TokenDTO GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var authSiginingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var _TokenExpiryTimeInMinutes = Convert.ToInt16(_configuration["Jwt:ExpiryTimeInMinute"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_TokenExpiryTimeInMinutes),
                //Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(authSiginingKey, SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var tokenModel = new TokenDTO
            {
                AccessToken = tokenHandler.WriteToken(securityToken),
                TokenExpiry = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(5),
            };

            return tokenModel;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public (bool, dynamic) Refresh(TokenApiDTO tokenApiModel)
        {
            if (tokenApiModel is null)
                return (false, "Invalid request.");
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            //var user = _userContext.LoginModels.SingleOrDefault(u => u.UserName == username);

            //if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            //    return (false, "Invalid request");

            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            //user.RefreshToken = newRefreshToken;

            //_userContext.SaveChanges();

            return (true, new TokenRefreshResponseDTO { AccessToken = newAccessToken.AccessToken, RefreshToken = newRefreshToken });
        }

        public bool Revoke(string username)
        {
            //var user = _dbContext.Users.SingleOrDefault(u => u.UserName == username);

            //if (user == null) return  false;

            //user.RefreshToken = null;

            //_userContext.SaveChanges();

            return true;
        }

        public async Task<Result<string, ErrorDTO>> SwitchToWorkerAccount(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user == null)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "User not found."
                    };

                    return Result<string, ErrorDTO>.Fail(error);
                }

                var roleAssignResult = await _userManager.AddToRoleAsync(user, "Worker");

                if (!roleAssignResult.Succeeded)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = roleAssignResult.Errors.FirstOrDefault()?.Description ?? "Failed to assign role."
                    };

                    return Result<string, ErrorDTO>.Fail(error);
                }

                user.IsCompanyWorker = true;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = updateResult.Errors.FirstOrDefault()?.Description ?? "Role assigned but failed to update field isCompanyWorker."
                    };

                    return Result<string, ErrorDTO>.Fail(error);
                }

                return Result<string, ErrorDTO>.Success("Switched to worker account successfully.");
            }
            catch (Exception ex) 
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Something went wrong. Please check UserId and try again."
                };

                return Result<string, ErrorDTO>.Fail(error);
            }
        }
    }

}
