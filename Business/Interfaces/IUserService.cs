
using Business.DTOs;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterUserAsync(UserRegisterDto registerDto);
        Task<string> LoginUserAsync(UserLoginDto loginDto);
        Task<UserResponseDto> GetUserByIdAsync(int id);
    }
}