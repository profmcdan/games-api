using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using RpgGame.DTOs.Auth;
using RpgGame.Helpers;
using RpgGame.ResourceParameters;
using RpgGame.Services;

namespace RpgGame.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        
        public AuthController(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("users")]
        public ActionResult<ServiceResponse<IEnumerable<UserDto>>> GetUsers()
        {
            ServiceResponse<IEnumerable<UserDto>> response = new ServiceResponse<IEnumerable<UserDto>>();
            var users = _authRepository.GetUsers();
            response.Data = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(response);
        }

        [HttpGet("users/{userId}", Name = "GetUser")]
        public ActionResult<ServiceResponse<UserDto>> GetUser(Guid userId)
        {
            ServiceResponse<UserDto> response = new ServiceResponse<UserDto>();
            var user = _authRepository.GetUserById(userId);
            if (user == null)
            {
                response.StatusCode = 404;
                response.Message = "The user with this Id does not exists";
                response.Success = false;
                return NotFound(response);
            }

            response.Data = _mapper.Map<UserDto>(user);
            return Ok(response);
        }
        
        [HttpPost("signup")]
        public ActionResult<ServiceResponse<UserDto>> RegisterUser(CreateUserDto user)
        {
            ServiceResponse<UserDto> response = new ServiceResponse<UserDto>();
            var exists = _authRepository.UserExists(user.Email);
            if (exists)
            {
                response.StatusCode = 400;
                response.Message = "User with this email already exists!";
                response.Success = false;
                return BadRequest(response);
            }

            var userEntity = _authRepository.Register(user);
            _authRepository.Save();
            response.Data = _mapper.Map<UserDto>(userEntity);
            response.StatusCode = 201;
            return CreatedAtRoute("GetUser", new {userId=userEntity.Id}, response);
        }
        

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<LoginDto>>> LoginUser(LoginResource loginResource)
        {
            ServiceResponse<LoginDto> response = new ServiceResponse<LoginDto>();
            var user = _authRepository.GetUserByEmail(loginResource.Email);
            if (user == null)
            {
                response.Message = "User with this email not found";
                response.StatusCode = 404;
                response.Success = false;
                return NotFound(response);
            }
            
            Authentication auth = new Authentication();
            if (!auth.VerifyPasswordHash(loginResource.Password, user.PasswordHash, user.PasswordSalt))
            {
                response.Message = "Invalid login credentials provided";
                response.StatusCode = 400;
                response.Success = false;
                return BadRequest(response);
            }

            response.Data = new LoginDto()
            {
                AccessToken = _authRepository.CreateToken(user),
                RefreshToken = "Refresh",
                UserId = user.Id
            };
            
            return Ok(response);
        }
    }
}