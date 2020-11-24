using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NetCoreWeb.Shared;
using NetCoreWeb.Shared.ViewModels;

namespace NetCoreWeb.Api.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<IdentityUser> userManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model is null)
                throw new NullReferenceException("Register Model is Null");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the entered password!",
                    IsSuccess = false
                };
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created Successfully",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "Could not create user",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}