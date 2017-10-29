namespace ListsWebAPi.Repositories
{
    public interface IWhiteListedTokensRepo
    {
        bool DoesTokenExist(string token);
        void AddNewToken(string token);
        void DeleteToken(string token);
    }
}