using AutoMapper;
using museum.Application.main.events.dto;
using museum.Application.main.mailing;
using museum.EF.repositories;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;

namespace museum.Application.main.events
{
    public class EventService: IEventService
    {
        protected readonly IEventRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly IMailingService _mailingService;
        public EventService(IEventRepository repository, IMapper mapper, IMailingService mailingService)
        {
            _repository = repository;
            _mapper = mapper;
            _mailingService = mailingService;
        }

        public IEnumerable<ReservationDTO> GetAllEvents()
        {
            var list = _repository.GetEvents();
            var listDto = _mapper.Map<List<ReservationDTO>>(list);
            return listDto;
        }

        public Reservation PostEvent(ReservationDTO item)
        {
            item = ConvertTimeToLocal(item);
            var sub = _mapper.Map<Reservation>(item);
            _repository.Post(sub);
            return sub;
        }

        public void DeleteEvent(int id)
        {
            _repository.Delete(id);
            var reservation = GetEventById(id);
            _mailingService.SendDeleteEmail(reservation);
        }

        public ReservationDTO GetEventById(int id)
        {
            var s = _repository.GetById(id);
            var dto = _mapper.Map<ReservationDTO>(s);
            return dto;
        }

        public void UpdateEvent(int id, ReservationDTO s)
        {
            if (s.Email != null)
            {
                s = ConvertTimeToLocal(s);
            }
            if (s.Email == null)
            {
                s.Email = "kaltenytmonika3@gmail.com";
            }
            var oldEvent = GetEventById(id);
            var newEvent = _mapper.Map<Reservation>(s);

            var allEvents = _repository.GetEvents();

            foreach (var e in allEvents)
            {
                if ((newEvent.Start > e.Start) && (newEvent.End < e.End))
                {
                    throw new System.ArgumentException("Already exists", "original");
                }
                else
                {
                    _repository.Update(id, newEvent);
                }                
            }
            
            if(oldEvent.Start != newEvent.Start || oldEvent.End != newEvent.End)
            {
                _mailingService.SendUpdateEmail(s);
            }  
        }

        private ReservationDTO ConvertTimeToLocal(ReservationDTO r)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
            r.Start = TimeZoneInfo.ConvertTimeFromUtc(r.Start, easternZone);
            r.End = TimeZoneInfo.ConvertTimeFromUtc(r.End, easternZone);
            return r;
        }
    }
}
