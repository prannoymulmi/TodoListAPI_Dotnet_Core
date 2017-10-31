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
    /// This controller contians the endpoints carrying out CURD operations for lists
    /// TODO: End points for adding, updating, and deleting the Lists
    /// </summary>
    
    [Route("api/v1/lists")]
    public class ListApiController : Controller
    {
        private readonly ITokenManagerController _tokenManagerController;
        
        public ListApiController(ITokenManagerController tokenManagerController)
        {
                _tokenManagerController = tokenManagerController;
        }
 
        [HttpGet("validate")]
        public object GetList(string token)
        {
            var sucess = _tokenManagerController.ValidateToken(token);
            return new {sucess};
        }
    }
}