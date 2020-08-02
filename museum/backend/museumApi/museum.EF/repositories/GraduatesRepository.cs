using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace museum.EF.repositories
{
    public class GraduatesRepository: Repository<Graduates>, IGraduatesRepository
    {
        public GraduatesRepository(MuseumContext context) : base(context)
        {
        }

        public List<Graduates> GetGraduates()
        {
            var list = museumContext.Graduates.OrderBy(x => x.Title).ToList();
            return list;

        }

        public void Post(Graduates c)
        {
            museumContext.Add(c);
            museumContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var c = museumContext.Graduates.SingleOrDefault(p => p.Id == id);
            museumContext.Remove(c);
            museumContext.SaveChanges();
        }

        public void Update(int id, Graduates c)
        {
            var sub = museumContext.Graduates.Where(p => id == p.Id).Single();
            sub.Title = c.Title;
            museumContext.SaveChanges();
        }

        public Graduates GetById(int id)
        {
            var sub = museumContext.Graduates.Where(p => id == p.Id).Single();

            return sub;
        }

        public MuseumContext museumContext
        {
            get { return Context as MuseumContext; }
        }
    }
}
