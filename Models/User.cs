using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Models;
using Microsoft.AspNetCore.Identity;

namespace dotnet_backend.Models
{
    public class User : IdentityUser
    {
        public List<Order> Orders { get; set; } = new List<Order>();
        public Basket Basket { get; set; } = new Basket();
    }
}