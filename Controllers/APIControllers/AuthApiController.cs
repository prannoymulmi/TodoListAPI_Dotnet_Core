using System;
using System.Collections.Generic;
using System.Security.Claims;
using ListsWebAPi.Interfaces;
using ListsWebAPi.Models;
using ListsWebAPi.Repositories;
using ListsWebAPi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ListsWebAPi.Controllers.APIControllers
{
    /// <summary>
    /// The API controller which is related to Authorization
    /// </summary>
    [Route("api/v1/auth")]
    public class AuthApiController : Controller, IAuthController
    {
        private readonly AuthController _authController;

        public AuthApiController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            , IPasswordHasher<ApplicationUser> passwordHasher, IUserJwtInfoRepo userJwtInfoRepo)
        {
            _authController = new AuthController(userManager, signInManager, roleManager, passwordHasher, userJwtInfoRepo);
        }

        /// <summary>
        /// Sends a Token If login is Sucessful
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// TODO: Put appropriate return code for failure
        [HttpPost("login")]
        public object Login([FromBody] LoginViewModel model)
        {
            var isUserValidated = _authController.ValidateLoginCedentials(model.Email, model.Password);
            var user = _authController.GetUserDetailsByEmail(model.Email).Result;
            
            if (isUserValidated.Result)
            {
                var claimsIdentity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.Id),
                }, "Custom");
                
                _authController.setClaims(claimsIdentity);
                var token = _authController.CreateToken(user.Id);
                return new
                {
                    token
                };
            }
            
            return new 
            {
                token = "sdasdsad"
            };
        }

        public void Logout()
        {
            
        }
    }
}