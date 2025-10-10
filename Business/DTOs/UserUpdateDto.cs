
namespace Business.DTOs
{
    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}