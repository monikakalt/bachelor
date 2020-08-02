using Microsoft.EntityFrameworkCore;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace museum.EF.repositories
{
    public class ChroniclesRepository : Repository<Chronicle>, IChroniclesRepository
    {
        public ChroniclesRepository(MuseumContext context) : base(context)
        {
        }

        public List<Chronicle> GetAll()
        {
            return MuseumContext.Chronicles
              .Include(p => p.Photos)
              .ToList();
        }

        public Chronicle GetRecent()
        {
            var list = MuseumContext.Chronicles.ToList();
            return list.OrderBy(x => x.Date).FirstOrDefault();
        }

        public Chronicle GetById(int id)
        {
            return MuseumContext.Chronicles
                .Where(p => id == p.Id)
                .Include(p => p.Photos)
                .FirstOrDefault();
        }

        public void Post(Chronicle c)
        {
            MuseumContext.Add(c);
            MuseumContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var c = MuseumContext.Chronicles.SingleOrDefault(p => p.Id == id);
            MuseumContext.Remove(c);
            MuseumContext.SaveChanges();
        }

        public void Update(int id, Chronicle c)
        {
            var sub = MuseumContext.Chronicles.Where(p => id == p.Id).Single();
            sub.Title = c.Title;
            MuseumContext.SaveChanges();
        }

        public MuseumContext MuseumContext
        {
            get { return Context as MuseumContext; }
        }
    }
}
