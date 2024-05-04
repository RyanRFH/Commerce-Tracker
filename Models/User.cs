using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace dotnet_backend.Models
{
    public class User : IdentityUser
    {
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}