using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commerce_tracker_v2.Interfaces;
using dotnet_backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace commerce_tracker_v2.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            //Replace [jwt] with ENV keys
        }

        public string CreateToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}