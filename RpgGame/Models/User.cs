using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RpgGame.Models
{
    
    
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Roles Role { get; set; } = Roles.Basic;
        public bool IsActive { get; set; } = false;
        public bool IsSuperUser { get; set; } = false;
        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<Character> Characters { get; set; }
    }
}