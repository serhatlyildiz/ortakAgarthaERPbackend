﻿using Core.Entities;

namespace Entities.DTOs
{
    public class UserForUpdateDto : IDto
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Adress { get; set; }
        public string? Cinsiyet { get; set; }
        public bool Status { get; set; }
        public List<int>? Roles { get; set; }
    }
}
