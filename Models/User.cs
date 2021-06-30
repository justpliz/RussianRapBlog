﻿using Microsoft.AspNetCore.Identity;

namespace Models
{
    /// <summary>
    ///     Пользователь
    /// </summary>
    public class User : IdentityUser
    {
        public string Description { get; set; }
    }
}