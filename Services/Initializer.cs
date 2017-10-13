using System;
using System.Collections.Generic;
using System.Linq;
using ListsWebAPi.Entity;
using ListsWebAPi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

/**
This class is just for debugging purposes initializes the tables USer, Lists,ListItems for a first time use
**/
namespace ListsWebAPi.DbInitalizer
{
    public class Initializer
    {
        public static void InitializeContext(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            context.Database.EnsureCreated();

            if (context.User.Any())
            {
                return;
            }
            
            var identityUser = new ApplicationUser
            {
                UserName = "prannoym",
                Email = "prannoy.mulmi@gmail.com"

            };

            String URL = "http://www.prannoy.com";
            
            var signUpSucces = _userManager.CreateAsync(identityUser, "$$!5167Pm").Result;

            var user = _userManager.FindByEmailAsync(identityUser.Email);

            if (signUpSucces.Succeeded)
            {
                // UseManager inherits from Identity User and it injets a UserId in the indetityUser Object
                var id = identityUser.Id;
            
                // List Items Initializer
                var listItem = new ListItem
                {
                    ItemId = Guid.NewGuid(),
                    ItemName = "Jeans"
                };
                
                
            
                List<ListItem> items = new List<ListItem>();
                items.Add(listItem);
                items.Add(new ListItem
                {
                    ItemId = Guid.NewGuid(),
                    ItemName = "Potatoes"
                });
            
                context.ListItem.Add(listItem);
                context.SaveChanges();

                //Lists Initializer
                var lists = new Lists
                {
                    AspNetUserId = id,
                    ListId = Guid.NewGuid(),
                    ListName = "First List",
                    Items = items
                };
            
                context.Lists.Add(lists);
                context.SaveChanges();
                
                UserJwtInfo userJwtInfo= new UserJwtInfo
                {
                    Id = Guid.NewGuid(),
                    Issuer = URL,
                    Audience = URL,
                    AspNetUserId = id,
                    UserSecurityKey = Guid.NewGuid()
                }; 
                
                context.UserJwtInfos.Add(userJwtInfo);
                context.SaveChanges();
            }
            
        }
    }
}