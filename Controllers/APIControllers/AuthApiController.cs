﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using ListsWebAPi.Interfaces;
using ListsWebAPi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ListsWebAPi.Controllers.APIControllers
{
    /// <summary>
    /// The API controller which contains the endpoints concerned with authorization of a user
    /// </summary>
    [Route("api/v1/auth")]
    public class AuthApiController : Controller, IAuthApiController
    {
        private readonly IAuthController _authController;
        private readonly ITokenManagerController _tokenManagerController;
        /// <summary>
        /// Injected IAuthController and ItokenManager using Dependency Injection 
        /// DI is used so that no dependecies to AuthAPiController is present
        /// </summary>
        /// <param name="authController"></param>
        /// <param name="tokenManagerController"></param>
        public AuthApiController(IAuthController authController, ITokenManagerController tokenManagerController)
        {
            _authController = authController;
            _tokenManagerController = tokenManagerController;

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
                
                _tokenManagerController.setClaims(claimsIdentity);
                var token = _tokenManagerController.CreateToken(user.Id);
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
                
                _tokenManagerController.setClaims(claimsIdentity);
                var newUserId = _authController.GetApplicationUser().Id;
                var token = _tokenManagerController.CreateToken(newUserId);
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
            var doesExist = _tokenManagerController.RemoveToken(model.token);
            return new
            {
                Sucess = doesExist
            };
        }
    }
}