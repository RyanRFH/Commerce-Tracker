using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend.Models;

namespace commerce_tracker_v2.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}