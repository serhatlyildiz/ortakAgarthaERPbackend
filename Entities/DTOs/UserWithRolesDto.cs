using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class UserWithRolesDto: IDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Adress { get; set; }
        public bool Status { get; set; }
        public string Cinsiyet { get; set; }
        public List<string> Roles { get; set; }    
    }
}
