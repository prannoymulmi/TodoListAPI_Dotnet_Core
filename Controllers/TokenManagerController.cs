using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ListsWebAPi.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ListsWebAPi.Controllers
{
    public class TokenManagerController: ITokenManagerController
    {
     
        protected ClaimsIdentity claimsIdentity { get; set; }

        public TokenManagerController()
        {
                
        }

        
       /// <summary>
       /// This is a Method which creates a JWT Token which will be then used to validate to get the resources from the API
       /// </summary>
       /// <returns></returns>
        public string CreateToken()
        {
            SecurityTokenDescriptor securityTokenDescriptor = CreateSecurityTokenDescriptor();
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(plainToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateToken(string token)
        {
            SecurityToken validatedToken;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = CreateTokenValidationParameters();
            
            try
            {
                tokenHandler.ValidateToken(token,
                    tokenValidationParameters, out validatedToken);
            }
            catch(SecurityTokenException)
            {  
                return false; 
            }
            
            return true;
        }
        
        

       /// <summary>
       /// Creates a Token Descriptor which is needed to create a token
       /// </summary>
       /// <returns></returns>
        private SecurityTokenDescriptor CreateSecurityTokenDescriptor()
        {
            var plainTextSecurityKey = "This is my shared, not so secret, secret!";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
            var signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature);
            
           ///TODO: Create personalized Audience and Security Key usinga DB
            return  new SecurityTokenDescriptor ()
            {
                Audience = "http://www.thisIsMySite.com",
                Issuer = "http://www.thisIsMySite.com",
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials
            };
        }

        private TokenValidationParameters CreateTokenValidationParameters()
        {
            var plainTextSecurityKey = "This is my shared, not so secret, secret!";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));
            
            return new TokenValidationParameters()
            {
                ValidAudience = "http://my.website.com",
                ValidIssuer = "http://my.tokenissuer.com",
                IssuerSigningKey = signingKey,
                ValidateLifetime = true
            };
        }

        public void setClaims(ClaimsIdentity claimsIdentity)
        {
            this.claimsIdentity = claimsIdentity;
        }  
    }
}