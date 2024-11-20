using Core.Entities;

namespace Entities.DTOs
{
    public class PasswordResetDto : IDto
    {
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}
