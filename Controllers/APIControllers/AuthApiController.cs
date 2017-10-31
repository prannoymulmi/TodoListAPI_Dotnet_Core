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
    public class AuthApiController : Controller, IAuthApiController
    {
        private readonly AuthController _authController;
        private readonly IWhiteListedTokensRepo _whiteListedTokensRepo;

        public AuthApiController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            , IPasswordHasher<ApplicationUser> passwordHasher, IUserJwtInfoRepo userJwtInfoRepo, IWhiteListedTokensRepo whiteListedTokensRepo)
        {
            _authController = new AuthController(userManager, signInManager, roleManager, passwordHasher, userJwtInfoRepo, whiteListedTokensRepo);
            _whiteListedTokensRepo = whiteListedTokensRepo;
        }

        /// <summary>
        /// Sends a Token If login is Sucessful
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// TODO: Add the token to the WhiteListDb
        /// TODO: ADD Error Handling if adding to DB is not successful
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
                    new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                    new Claim("sub", user.Id),
                }, "Custom");
                
                _authController.setClaims(claimsIdentity);
                var token = _authController.CreateToken(user.Id);
                return new
                {
                    token
                };
            }

            return Unauthorized();
        }

        /// <summary>
        /// Registers a new User
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        /// TODO: ADD The token to the WhiteListDb
        /// TODO: ADD Error Handling if adding to DB is not successful
        [HttpPost("register")]
        public object Register([FromBody]RegisterViewModel newUser)
        {
            var register = _authController.RegisterUser(newUser).Result;
            if (register.Succeeded)
            {
                var claimsIdentity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, newUser.UserName),
                    new Claim(ClaimTypes.Email, newUser.Email),
                    new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                }, "Custom");
                
                _authController.setClaims(claimsIdentity);
                var token = _authController.CreateToken(_authController.newUser.Id);
                return Created("Login Successful", new {token});
            }

            return BadRequest(register.Errors);
        }
        
        
        
        /// <summary>
        /// TODO: Remove the Token from the WhitelistDb when loggin out
        /// TODO: ADD Error handling if CURD operation not sucessful
        /// </summary>
        [HttpPost("logout")]
        public object Logout([FromBody]LogoutViewModel model)
        {
            var doesExist = _authController.RemoveToken(model.token);
            return new
            {
                Sucess = doesExist
            };
        }
    }
}