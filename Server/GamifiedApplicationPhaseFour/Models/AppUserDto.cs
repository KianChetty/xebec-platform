﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GamifiedApplicationPhaseFour.Models
{
    public class AppUserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }


        public AppUserDto(string email, string role, string passwordHash)
        {
            //this.Id = id;
            this.Email = email;
            this.Role = role;
            this.PasswordHash = passwordHash;
        }

    }
}
