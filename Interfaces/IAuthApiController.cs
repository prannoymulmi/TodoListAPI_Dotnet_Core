using System.Runtime.CompilerServices;
using ListsWebAPi.Controllers;
using ListsWebAPi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ListsWebAPi.Interfaces
{
    public interface IAuthApiController
    {
        object Login([FromBody] LoginViewModel model);
        object Logout([FromBody] LogoutViewModel model);
    }
}