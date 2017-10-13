using System.Collections.Generic;
using ListsWebAPi.Models;

namespace ListsWebAPi.Repositories
{
    public interface IUserJwtInfoRepo
    {
        List<UserJwtInfo> GetUserJwtInfos(string userId);
    }
}