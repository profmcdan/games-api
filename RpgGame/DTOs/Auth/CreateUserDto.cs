using System;
using RpgGame.Models;

namespace RpgGame.DTOs.Auth
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; } = Roles.Basic;

    }
}