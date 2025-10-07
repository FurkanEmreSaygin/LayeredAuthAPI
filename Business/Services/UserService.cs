
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Business.DTOs;
using Business.Interfaces;
using Business.Settings;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public UserService(IAuthRepository authRepository, IMapper mapper, IOptions<JwtSettings> jwtSettings)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

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
        public async Task DeleteUserAsync(int id)
        {
            User? user = await _authRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            await _authRepository.DeleteUserAsync(user);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int userId, UserUpdateDto UserUpdateDto)
        {
            User? userToUpdate = await _authRepository.GetUserByIdAsync(userId);

            if (userToUpdate == null)
            {
                throw new Exception("User not found.");
            }

            if (!string.IsNullOrEmpty(UserUpdateDto.Email) && UserUpdateDto.Email != userToUpdate.Email)
            {
                if (await _authRepository.GetUserByEmailAsync(UserUpdateDto.Email) != null)
                {
                    throw new Exception("New email address is already in use by another account.");
                }
                userToUpdate.Email = UserUpdateDto.Email;
            }

            if (!string.IsNullOrEmpty(UserUpdateDto.Username))
            {
                userToUpdate.Username = UserUpdateDto.Username;
            }

            if (!string.IsNullOrEmpty(UserUpdateDto.NewPassword))
            {
                userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(UserUpdateDto.NewPassword);
            }

            await _authRepository.UpdateUserAsync(userToUpdate);

            return _mapper.Map<UserResponseDto>(userToUpdate);
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtSettings.ExpiresInDays),
                signingCredentials: credentials);
 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}