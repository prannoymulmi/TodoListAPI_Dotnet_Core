using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ListsWebAPi.Interfaces
{
    public interface ITokenManagerController
    {
        string CreateToken();
        bool ValidateToken(string token);
        void setClaims(ClaimsIdentity claimsIdentity);
    }
}