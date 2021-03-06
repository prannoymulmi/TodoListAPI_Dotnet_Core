﻿using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ListsWebAPi.Interfaces
{
    public interface ITokenManagerController
    {
        string CreateToken(String userId);
        bool ValidateToken(string token);
        void setClaims(ClaimsIdentity claimsIdentity);
        bool RemoveToken(string token);
    }
}