using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RpgGame.DbContexts;
using RpgGame.DTOs.Auth;
using RpgGame.Helpers;
using RpgGame.Models;
using RpgGame.ResourceParameters;

namespace RpgGame.Services
{
    public class AuthRepository : IAuthRepository, IDisposable
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName)
            };
            SymmetricSecurityKey key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:JWTSecret").Value));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = credentials
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public User Register(CreateUserDto newUser)
        {
            var email = newUser.Email.ToLower().Trim();
            var existingUser = GetUserByEmail(email);
            if (existingUser != null)
            {
                throw new Exception("User with email address already exists");
            }

            Authentication auth = new Authentication();
            auth.CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userEntity = new User
            {
                Email = email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Address = newUser.Address,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = newUser.Role,
                Id = Guid.NewGuid()
            };

            _context.Users.Add(userEntity);
            return userEntity;
        }

        public async Task<LoginDto> Login(LoginResource loginResource)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginResource.Email);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            Authentication auth = new Authentication();
            if (!auth.VerifyPasswordHash(loginResource.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Invalid login credentials provided");
            }

            return new LoginDto()
            {
                AccessToken = CreateToken(user),
                RefreshToken = "Refresh",
                UserId = user.Id
            };
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void UpdateUser(Guid userId, User userPayload)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public bool UserExists(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            return _context.Users.Any(u => u.Email == email.ToLower().Trim());
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}