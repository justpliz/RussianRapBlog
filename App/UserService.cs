using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Constants;
using Models.Settings;
using Services.Interfaces;

namespace Services
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
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
                return $"Email {user.Email} is already registered.";

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception("Не удалось создать пользователя"); //TODO Добавить вывод ошибок из result

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            return $"User Registered with username {user.UserName}";
        }

        /// <inheritdoc />
        public async Task<AuthenticationResponseDto> GetTokenAsync(TokenRequestDto model)
        {
            var authenticationDto = new AuthenticationResponseDto();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                authenticationDto.IsAuthenticated = false;
                authenticationDto.Message = $"No Accounts Registered with {model.Email}.";
                return authenticationDto;
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authenticationDto.IsAuthenticated = true;
                var jwtSecurityToken = await CreateJwtToken(user);
                authenticationDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationDto.Email = user.Email;
                authenticationDto.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationDto.Roles = rolesList.ToList();
                return authenticationDto;
            }

            authenticationDto.IsAuthenticated = false;
            authenticationDto.Message = $"Incorrect Credentials for user {user.Email}.";
            return authenticationDto;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
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