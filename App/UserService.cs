using System;
using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
    }
}