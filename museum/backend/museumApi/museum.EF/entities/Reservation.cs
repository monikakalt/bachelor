using System;
using System.Collections.Generic;

namespace museumApi.EF.entities
{
    public partial class Reservation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Email { get; set; }
        public byte? IsDeleted { get; set; }
        public int FkUser { get; set; }
        public byte? ReminderSent { get; set; }

        public User FkUserNavigation { get; set; }
    }
}
