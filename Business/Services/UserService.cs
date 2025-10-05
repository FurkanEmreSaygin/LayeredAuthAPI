
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Business.DTOs;
using Business.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public UserService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }
        private const string JwtSecret = "Bu-cok-gizli-anahtar-olmalı-ve-en-az-16-karakter-olmalı";
        private const string JwtIssuer = "FoundationAuthAPI";

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            User? user = await _authRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<string> LoginUserAsync(UserLoginDto loginDto)
        {
            User? user = await _authRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password.");
            }

            string token = GenerateJwtToken(user);

            return token;
        }

        public async Task<UserResponseDto> RegisterUserAsync(UserRegisterDto registerDto)
        {

            if (await _authRepository.GetUserByEmailAsync(registerDto.Email) != null)
            {
                throw new Exception("Email address is already registered.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            User newUser = _mapper.Map<User>(registerDto);
            newUser.PasswordHash = passwordHash;

            User createdUser = await _authRepository.AddUserAsync(newUser);

            return _mapper.Map<UserResponseDto>(createdUser);
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtIssuer,
                audience: "FoundationAuthUsers",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}