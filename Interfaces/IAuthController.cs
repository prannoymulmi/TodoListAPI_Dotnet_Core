using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ListsWebAPi.Models;
using ListsWebAPi.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ListsWebAPi.Interfaces
{
    public interface IAuthController
    {
        Task<IdentityResult> RegisterUser(RegisterViewModel user);
        void AddNewUserJwtInfo(ApplicationUser user);
        Task<Boolean> ValidateLoginCedentials(String Email, String Password);
        Task<ApplicationUser> GetUserDetailsByEmail(String Email);
        ApplicationUser GetApplicationUser();
    }
}