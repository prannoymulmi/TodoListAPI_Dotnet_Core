using ListsWebAPi.Repositories;

namespace ListsWebAPi.Controllers
{
    /// <summary>
    /// TODO: Change how it is atm and use DI
    /// </summary>
    public class ListController
    {
        public ListController(IUserJwtInfoRepo userJwtInfoRepo, IWhiteListedTokensRepo whiteListedTokensRepo)
        {
        }
    }
}