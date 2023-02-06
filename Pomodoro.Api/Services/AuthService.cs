using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.Api.ViewModels.Auth;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pomodoro.Api.Services
{
    /// <summary>
    /// Create Jwt, perform registration and login operations.
    /// </summary>
    public class AuthService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthService> logger;
        private readonly UserManager<IdentityUser<Guid>> userManager;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">Set of key/value application configuration properties <see cref="IConfiguration"/>.</param>
        /// <param name="logger">Logger <see cref="ILogger"/>.</param>
        /// <param name="userManager">API for managing user in persistence store <see cref="UserManager{TUser}"/>.</param>
        /// <param name="userRepository">API for CRUD operations with application user <see cref="IUserRepository"/>.</param>
        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            UserManager<IdentityUser<Guid>> userManager,
            IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.userManager = userManager;
            this.userRepository = userRepository;
        }


        public async Task<LoginResponseViewModel> LoginAsync(LoginRequestViewModel loginRequest)
        {
            if (loginRequest is null)
            {
                throw new ArgumentNullException($"{nameof(loginRequest)} is null");
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Genererate JWT.
        /// </summary>
        /// <param name="user">Represent a user in identity system <see cref="IdentityUser{TKey}"/>.</param>
        /// <param name="appUser">Represent user in application <see cref="AppUser"/>.</param>
        /// <returns>JWT <see cref="JwtSecurityToken"/>.</returns>
        public JwtSecurityToken GetToken(IdentityUser<Guid> user, AppUser appUser)
        {
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: this.configuration["JwtSettings:Issuer"],
                audience: this.configuration["JwtSettings:Audience"],
                claims: this.GetClaims(user, appUser),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(
                    this.configuration["JwtSettings:ExpirationTimeMinutes"])),
                signingCredentials: this.GetSigningCredentials());

            this.logger.LogInformation($"Generate JWT for user ${user.Email}");

            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(this.configuration["JwtSettings:SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(IdentityUser<Guid> user, AppUser appUser)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, appUser.Name),
                new Claim("userId", appUser.Id.ToString()),
            };
            return claims;
        }
    }
}
