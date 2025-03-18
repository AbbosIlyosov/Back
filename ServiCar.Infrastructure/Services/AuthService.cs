using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Servicar.Infrastruture.Services
{
    public interface IAuthService
    {
        Task<ResponseDTO> Register(RegistrationDTO model);
        Task<ResponseDTO> Login(LoginDTO model);
        (bool, dynamic) Refresh(TokenApiDTO tokenApiModel);
        bool Revoke(string username);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ResponseDTO> Login(LoginDTO model)
        {
            var response = new ResponseDTO();

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "User not found.";

                return response;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                response.IsSuccess = false;
                response.Message = "Invalid password.";

                return response;
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            string token = GenerateAccessToken(authClaims);

            response.IsSuccess = true;
            response.Message = "User logged in successfully.";
            response.Data = new LoginResponseDataDTO
            {
                AccessToken = token,
                RefreshToken = GenerateRefreshToken(),
                User = new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    IsCompanyWorker = user.IsCompanyWorker,
                    Roles = userRoles
                }
            };

            return response;
        }

        public async Task<ResponseDTO> Register(RegistrationDTO model)
        {
            var response = new ResponseDTO();

            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                response.Message = "User already exists.";

                return response;
            }

            if (string.IsNullOrEmpty(model.Role))
            {
                model.Role = "User";
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
            {
                response.Message = "Role does not exist.";
                return response;
            }

            var newUser = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                IsCompanyWorker = model.Role.ToLower() == "worker",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                response.Message = "User creation failed. Please check user details and try again.";
                return response;
            }

            IdentityResult roleAssignResult = await userManager.AddToRoleAsync(newUser, model.Role);

            if (!roleAssignResult.Succeeded)
            {
                response.Message = roleAssignResult.Errors.FirstOrDefault()?.Description ?? "Failed to assign role.";
                return response;
            }

            response.IsSuccess = true;
            response.Message = "User created successfully.";
            response.Data = new UserDTO
            {
                Id = newUser.Id,
                Username = newUser.UserName,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber,
                IsCompanyWorker = newUser.IsCompanyWorker,
                Roles = new List<string> { model.Role }
            };

            return response;
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var authSiginingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var _TokenExpiryTimeInMinutes = Convert.ToInt16(_configuration["Jwt:ExpiryTimeInMinute"]);
            var tokeDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_TokenExpiryTimeInMinutes),
                //Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(authSiginingKey, SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokeDescriptor);
            return tokenHandler.WriteToken(securityToken);
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

            return (true, new TokenRefreshResponseDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }

        public bool Revoke(string username)
        {
            //var user = _dbContext.Users.SingleOrDefault(u => u.UserName == username);

            //if (user == null) return  false;

            //user.RefreshToken = null;

            //_userContext.SaveChanges();

            return true;
        }
    }

}
