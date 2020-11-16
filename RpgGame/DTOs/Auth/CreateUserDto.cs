using System;
using System.ComponentModel.DataAnnotations;
using RpgGame.Models;

namespace RpgGame.DTOs.Auth
{
    public class CreateUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        public Roles Role { get; set; } = Roles.Basic;

    }
}