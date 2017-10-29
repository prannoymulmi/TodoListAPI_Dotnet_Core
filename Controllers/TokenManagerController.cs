using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ListsWebAPi.Interfaces;
using ListsWebAPi.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace ListsWebAPi.Controllers
{
    /// <summary>
    /// A controller which handles actions with tokens
    /// </summary>
    public class TokenManagerController: ITokenManagerController
    {
        protected ClaimsIdentity claimsIdentity { get; set; }
        private readonly IUserJwtInfoRepo _userJwtInfoRepo;
        private readonly IWhiteListedTokensRepo _whiteListedTokensRepo;

        public TokenManagerController(IUserJwtInfoRepo userJwtInfoRepo, IWhiteListedTokensRepo whiteListedTokensRepo)
        {
            _userJwtInfoRepo = userJwtInfoRepo;
            _whiteListedTokensRepo = whiteListedTokensRepo;
        }

        
       /// <summary>
       /// This is a Method which creates a JWT Token which will be then used to validate to get the resources from the API
       /// </summary>
       /// <returns></returns>
       /// TODO: ADD Proper Exception Message
        public string CreateToken(String userId)
        {
            SecurityTokenDescriptor securityTokenDescriptor = CreateSecurityTokenDescriptor(userId);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
                //Adding Token to the database
                _whiteListedTokensRepo.AddNewToken(tokenHandler.WriteToken(plainToken));
                return tokenHandler.WriteToken(plainToken);
            }
            catch (Exception)
            {
                return "Token Could not be Made";
            }
            
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="token"></param>
       /// <param name="userId"></param>
       /// <returns></returns>
       /// Add Error Handling to DB
        public bool ValidateToken(string token, String userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = CreateTokenValidationParameters(userId);
            var isTokenValid = false;
            try
            {
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token,
                    tokenValidationParameters, out validatedToken);
                isTokenValid = _whiteListedTokensRepo.DoesTokenExist(token);
            }
            catch(SecurityTokenException)
            {  
                return isTokenValid; 
            }
            
            return isTokenValid;
        }


        public bool RemoveToken(string token)
        {
            var doesExist = _whiteListedTokensRepo.DoesTokenExist(token);
            if (doesExist)
            {
                _whiteListedTokensRepo.DeleteToken(token);   
            }
            return doesExist;
        }
        
       /// <summary>
       /// Creates a Token Descriptor which is needed to create a token
       /// </summary>
       /// <param name="userId"></param>
       /// <returns></returns>
        private SecurityTokenDescriptor CreateSecurityTokenDescriptor(String userId)
        {
            var info = _userJwtInfoRepo.GetUserJwtInfos(userId);
            
            if (info.Count == 0)
            {
                return null;
            }
            
            var plainTextSecurityKey = info[0].UserSecurityKey.ToString();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
            var signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature);
            
          
            return  new SecurityTokenDescriptor ()
            {
                Audience = info[0].Audience,
                Issuer = info[0].Issuer,
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private TokenValidationParameters CreateTokenValidationParameters(String userId)
        {
            var info = _userJwtInfoRepo.GetUserJwtInfos(userId);
            
            if (info.Count == 0)
            {
                return null;
            }
            
            var plainTextSecurityKey = info[0].UserSecurityKey.ToString();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
            
            return new TokenValidationParameters()
            {
                ValidAudience = info[0].Audience,
                ValidIssuer = info[0].Issuer,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsIdentity"></param>
        public void setClaims(ClaimsIdentity claimsIdentity)
        {
            this.claimsIdentity = claimsIdentity;
        }  
    }
}