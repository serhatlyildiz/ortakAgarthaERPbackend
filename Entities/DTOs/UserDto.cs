using Core.Entities;

namespace Entities.DTOs
{
    public class UserDto : IDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
    }

}
