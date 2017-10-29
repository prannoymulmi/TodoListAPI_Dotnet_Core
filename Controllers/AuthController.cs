using System;
using System.Threading.Tasks;
using ListsWebAPi.Models;
using ListsWebAPi.Repositories;
using ListsWebAPi.ViewModels;
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
        private readonly IUserJwtInfoRepo _userJwtInfoRepo;
        public ApplicationUser newUser { get; set; }
       
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            , IPasswordHasher<ApplicationUser> passwordHasher, IUserJwtInfoRepo userJwtInfoRepo, IWhiteListedTokensRepo whiteListedTokensRepo): base(userJwtInfoRepo, whiteListedTokensRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _userJwtInfoRepo = userJwtInfoRepo;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// TODO: Set UseerJWTInfos with the new SecurityKey and Id and issuers
        public async Task<IdentityResult> RegisterUser(RegisterViewModel user)
        {
            var identityUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email
            };
           
            var signUpSucces = await _userManager.CreateAsync(identityUser, user.Password);
            
            //If Sigin is succeded _usermanager adds all the remaining attribute to identity User
            if (signUpSucces.Succeeded)
            {
                AddNewUserJwtInfo(identityUser);
                newUser = identityUser;
            }
            return signUpSucces;

        }

        public void AddNewUserJwtInfo(ApplicationUser user)
        {
            _userJwtInfoRepo.SetUserJwtInfoByUserId(user);
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