using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class UserFilterDto : IDto
    {
        public string Firstname { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public bool? Status { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
    }
}
