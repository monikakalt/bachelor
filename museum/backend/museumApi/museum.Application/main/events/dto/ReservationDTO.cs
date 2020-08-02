using museumApi.EF.entities;
using System;

namespace museum.Application.main.events.dto
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Email { get; set; }
        public bool? IsDeleted { get; set; }
        public int FkUser { get; set; }
        public bool? ReminderSent { get; set; }
        public User FkUserNavigation { get; set; }
    }
}
