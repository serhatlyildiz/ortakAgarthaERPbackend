using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class PasswordResetRequest : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ResetToken { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; }

    }
}
