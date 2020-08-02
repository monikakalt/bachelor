using museumApi.EF.entities;
using System.Collections.Generic;
using System.Linq;

namespace museum.EF.repositories
{
    public class EventRepository: Repository<Reservation>, IEventRepository
    {
        public EventRepository(MuseumContext context) : base(context)
        {
        }

        public List<Reservation> GetEvents()
        {
            return museumContext.Reservations
                .Where(x => x.IsDeleted != 1)
                .ToList();
        }

        public void Post(Reservation c)
        {
            museumContext.Add(c);
            museumContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var c = museumContext.Reservations.SingleOrDefault(p => p.Id == id);
            c.IsDeleted = 1;
            museumContext.SaveChanges();
        }

        public void Update(int id, Reservation c)
        {
            var sub = museumContext.Reservations.Where(p => id == p.Id).Single();
            sub.Email = c.Email;
            sub.End = c.End;
            sub.Start = c.Start;
            sub.Title = c.Title;
            museumContext.SaveChanges();
        }

        public Reservation GetById(int id)
        {
            var sub = museumContext.Reservations.Where(p => id == p.Id).Single();

            return sub;
        }

        public MuseumContext museumContext
        {
            get { return Context as MuseumContext; }
        }
    }
}
