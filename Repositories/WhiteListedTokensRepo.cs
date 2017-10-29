using System;
using System.Linq;
using ListsWebAPi.Entity;
using ListsWebAPi.Models;


namespace ListsWebAPi.Repositories
{
    /// <summary>
    /// This is a repository which controls the CURD operations for the whitelisted token managers
    /// </summary>
    public class WhiteListedTokensRepo: IWhiteListedTokensRepo
    {
        private  readonly ApplicationDbContext _whiteListedTokensRepo;

        public WhiteListedTokensRepo(ApplicationDbContext whiteListedTokensRepo)
        {
            _whiteListedTokensRepo = whiteListedTokensRepo;
        }
        
        /// <summary>
        /// Method which checks if a token exists in the whitelistDB
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool DoesTokenExist(string token)
        {
            var hasToken = _whiteListedTokensRepo.WhiteListedTokensList.Any(x => x.Token == token);
            return hasToken;
        }

        /// <summary>
        /// Adds new token to the whitelist
        /// </summary>
        /// <param name="token"></param>
        public void AddNewToken(string token)
        {
            var sucess = _whiteListedTokensRepo.WhiteListedTokensList.Add(new WhiteListedTokensList()
            {
                Id = Guid.NewGuid(),
                Token = token,
                TimestampCreated = DateTimeOffset.Now.ToUnixTimeSeconds()

            });
            _whiteListedTokensRepo.SaveChanges();
        }

        /// <summary>
        /// Deletes the given token in the whitelist db
        /// </summary>
        /// <param name="token"></param>
        public void DeleteToken(string token)
        {
            var item = _whiteListedTokensRepo.WhiteListedTokensList.Single(x => x.Token == token);
            var sucess = _whiteListedTokensRepo.WhiteListedTokensList.Remove(item);
            _whiteListedTokensRepo.SaveChanges();
        }
    }
}