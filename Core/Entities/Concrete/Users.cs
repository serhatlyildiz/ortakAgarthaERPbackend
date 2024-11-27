using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Users : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Adress { get; set; }
        public bool Status { get; set; }
        public string Cinsiyet { get; set; }
        public List<int>? Roles { get; set; }
    }
}
