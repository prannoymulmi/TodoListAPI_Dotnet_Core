using ListsWebAPi.Models;
using ListsWebAPi.Repositories;
using ListsWebAPi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ListsWebAPi.Controllers.APIControllers
{
    /// <summary>
    /// This controller contians the endpoints carrying out CURD operations for lists
    /// TODO: End points for adding, updating, and deleting the Lists
    /// </summary>
    
    [Route("api/v1/lists")]
    public class ListApiController : Controller
    {
        private readonly TokenManagerController _tokenManagerController;
        
        public ListApiController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            , IPasswordHasher<ApplicationUser> passwordHasher, IUserJwtInfoRepo userJwtInfoRepo, IWhiteListedTokensRepo whiteListedTokensRepo)
        {
                _tokenManagerController = new AuthController(userManager, signInManager, roleManager, passwordHasher, userJwtInfoRepo, whiteListedTokensRepo);
        }

        [HttpGet("validate")]
        public object getList(string token)
        {
            var sucess = _tokenManagerController.ValidateToken(token);
            return new {sucess};
        }
    }
}