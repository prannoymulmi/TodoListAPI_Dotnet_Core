using System.Collections.Generic;
using System.Linq;
using ListsWebAPi.Entity;
using ListsWebAPi.Models;

namespace ListsWebAPi.Repositories
{
    public class UserJwtInfoRepo: IUserJwtInfoRepo
    {
        private readonly ApplicationDbContext _userJwtInfoRepo;

        public UserJwtInfoRepo(ApplicationDbContext userJwtInfoRepo)
        {
            _userJwtInfoRepo = userJwtInfoRepo;
        }
        
        public List<UserJwtInfo> GetUserJwtInfos(string userId)
        {
            var info = _userJwtInfoRepo.UserJwtInfos.Where(x => x.AspNetUserId == userId).ToList();
            return info;
        }
    }
}