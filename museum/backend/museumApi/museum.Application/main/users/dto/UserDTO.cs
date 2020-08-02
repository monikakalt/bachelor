using museum.Application.main.chronicles.dto;
using museum.Application.main.events.dto;
using System.Collections.Generic;

namespace museum.Application.main.users.dto
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
