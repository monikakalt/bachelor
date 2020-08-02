using System;
using System.Collections.Generic;

namespace museumApi.EF.entities
{
    public partial class User
    {
        public User()
        {
            Chronicles = new HashSet<Chronicle>();
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Chronicle> Chronicles { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
