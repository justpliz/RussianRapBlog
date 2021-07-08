using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Constants;
using Models.Exceptions;
using Models.Settings;
using Services.Interfaces;

namespace Services
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly ILogger<UserService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                Description = dto.Description
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (userWithSameEmail != null)
                throw new BusinessException($"Почта {dto.Email} занята");

            var result = await _userManager.CreateAsync(user, dto.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description).ToList());
                throw new BusinessException($"Не удалось создать пользователя. Ошибки: {errors}");
            }

            await _userManager.AddToRoleAsync(user, Roles.User.ToString()).ConfigureAwait(false);
            _logger.LogInformation($"Пользователь {user.UserName} зарегистрирован");
            return $"Пользователь {user.UserName} зарегистрирован";
        }

        /// <inheritdoc />
        public async Task<AuthenticationResponseDto>
            GetTokenAsync(TokenRequestDto dto) //TODO Код сп*****ный у индуса. Перепилить
        {
            var authenticationDto = new AuthenticationResponseDto();
            var user = await _userManager.FindByEmailAsync(dto.Email).ConfigureAwait(false);
            if (user == null)
            {
                authenticationDto.IsAuthenticated = false;
                authenticationDto.Message = $"Пользователь с Email {dto.Email} не зарегистирован.";
                return authenticationDto;
            }

            if (await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                authenticationDto.IsAuthenticated = true;
                var jwtSecurityToken = await CreateJwtToken(user);
                authenticationDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationDto.Email = user.Email;
                authenticationDto.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationDto.Roles = rolesList.ToList();
                _logger.LogInformation($"Пользователь {user.UserName} вошел в систему");
                return authenticationDto;
            }

            authenticationDto.IsAuthenticated = false;
            authenticationDto.Message = $"Неверный пароль для {user.Email}.";
            return authenticationDto;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var roleClaims = roles.Select(t => new Claim("roles", t)).ToList();
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                _jwt.Issuer,
                _jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}