using museum.Application.main.events.dto;
using museum.EF.entities;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main.events
{
    public interface IEventService
    {
        IEnumerable<ReservationDTO> GetAllEvents();
        ReservationDTO GetEventById(int id);
        void DeleteEvent(int id);
        Reservation PostEvent(ReservationDTO item);
        void UpdateEvent(int id, ReservationDTO s);
    }
}
