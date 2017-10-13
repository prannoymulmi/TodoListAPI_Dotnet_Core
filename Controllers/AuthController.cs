using System;
using System.Threading.Tasks;
using ListsWebAPi.Models;
using ListsWebAPi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ListsWebAPi.Controllers
{
    public class AuthController: TokenManagerController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
       
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            , IPasswordHasher<ApplicationUser> passwordHasher, IUserJwtInfoRepo userJwtInfoRepo): base(userJwtInfoRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            
        }
        
      
        /// <summary>
        /// Checks if the credentials of a user are valid
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<Boolean> ValidateLoginCedentials(String Email, String Password)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            
            if (user != null)
            {
                var verifyPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, Password);
                if (verifyPassword == PasswordVerificationResult.Success)
                {
                    return true;
                }
                return false;
            }
            
            return false;
        }

        public async Task<ApplicationUser> GetUserDetailsByEmail(String Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            return user;
        }
    }
}