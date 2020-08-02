namespace museum.EF.repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using museumApi.EF.entities;

    public class ClassesRepository: Repository<ClassInfo>, IClassesRepository
    {
        public ClassesRepository(MuseumContext context)
            : base(context)
        {
        }

        public List<ClassInfo> GetClasses()
        {
            return this.MuseumContext.Classes
              //.Include(x => x.FkTeacher)
              .ToList();
        }

        public ClassInfo GetById(int id)
        {
            return this.MuseumContext.Classes.Where(p => id == p.Id).FirstOrDefault();
        }

        public void Post(ClassInfo c)
        {
            this.MuseumContext.Add(c);
            this.MuseumContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var c = this.MuseumContext.Classes.SingleOrDefault(p => p.Id == id);
            this.MuseumContext.Remove(c);
            this.MuseumContext.SaveChanges();
        }

        public void Update(int id, ClassInfo c)
        {
            var cl = this.MuseumContext.Classes.Where(p => id == p.Id).Single();
            cl.Title = c.Title;
            cl.FkTeacher = c.FkTeacher;
            this.MuseumContext.SaveChanges();
        }

        public ClassInfo GetClassByName(string n)
        {
            return this.MuseumContext.Classes.Where(x => x.Title == n).FirstOrDefault();
        }

        public MuseumContext MuseumContext
        {
            get { return this.Context as MuseumContext; }
        }
    }
}
