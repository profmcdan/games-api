using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RpgGame.DTOs.Auth;
using RpgGame.Models;
using RpgGame.ResourceParameters;

namespace RpgGame.Services
{
    public interface IAuthRepository
    {
        User Register(CreateUserDto newUser);
        Task<LoginDto> Login(LoginResource loginResource);
        IEnumerable<User> GetUsers();
        User GetUserById(Guid userId);
        User GetUserByEmail(string email);
        void UpdateUser(Guid userId, User userPayload);
        void DeleteUser(User user);
        bool UserExists(string email);
        bool Save();
    }
}