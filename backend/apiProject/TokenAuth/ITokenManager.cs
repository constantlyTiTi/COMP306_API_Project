using apiProject.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.TokenAuth
{
    public interface ITokenManager
    {
        
/*        bool Authenticate(string userName, string jwtToken);*/
        string GenerateJwtToken(IdentityUser user, ProjectPSConfig psConfig);
/*        bool VerifyToken(string token);*/
    }
}
