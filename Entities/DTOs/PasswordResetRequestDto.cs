using Core.Entities;

namespace Entities.DTOs
{
    public class PasswordResetRequestDto : IDto
    {
        public string Email { get; set; }
    }
}
