using System.Runtime.CompilerServices;
using ListsWebAPi.Controllers;
using ListsWebAPi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ListsWebAPi.Interfaces
{
    public interface IAuthController
    {
        object Login([FromBody] LoginViewModel model);
        object Logout([FromBody] LogoutViewModel model);
    }
}