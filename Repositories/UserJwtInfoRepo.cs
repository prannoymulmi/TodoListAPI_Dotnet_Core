using System;
using System.Collections.Generic;
using System.Linq;
using ListsWebAPi.Entity;
using ListsWebAPi.Models;

namespace ListsWebAPi.Repositories
{
    /// <summary>
    /// A Repository which handles the CURD actions in the model USerJwtInfo
    /// </summary>
    public class UserJwtInfoRepo: IUserJwtInfoRepo
    {
        private readonly ApplicationDbContext _userJwtInfoRepo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userJwtInfoRepo"></param>
        public UserJwtInfoRepo(ApplicationDbContext userJwtInfoRepo)
        {
            _userJwtInfoRepo = userJwtInfoRepo;
        }
        
        public List<UserJwtInfo> GetUserJwtInfos(string userId)
        {
            var info = _userJwtInfoRepo.UserJwtInfos.Where(x => x.AspNetUserId == userId).ToList();
            return info;
        }

        /// <summary>
        /// This method sets the new User Info required by the JWT token Manager to create tokens for a specific user
        /// </summary>
        /// <param name="user"></param>
        public void SetUserJwtInfoByUserId(ApplicationUser user)
        {
            _userJwtInfoRepo.UserJwtInfos.Add(new UserJwtInfo()
            {
                AspNetUserId = user.Id,
                Id = Guid.NewGuid(),
                UserSecurityKey = Guid.NewGuid(),
                Audience = $"http://www.{user.UserName}.com",
                Issuer = $"http://www.{user.UserName}.com"
            });
            
           var saved =  _userJwtInfoRepo.SaveChanges();
        }
    }
}