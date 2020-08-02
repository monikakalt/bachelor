using museumApi.EF.entities;
using System.Collections.Generic;
using System.Linq;

namespace museum.EF.repositories
{
    public class TeachersRepository: Repository<Teacher>, ITeachersRepository
    {
        public TeachersRepository(MuseumContext context) : base(context)
        {
        }

        public List<Teacher> GetTeachers()
        {
            return MuseumContext.Teachers.ToList();
        }

        public void Post(Teacher c)
        {
            MuseumContext.Add(c);
            MuseumContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var c = MuseumContext.Teachers.SingleOrDefault(p => p.Id == id);
            MuseumContext.Remove(c);
            MuseumContext.SaveChanges();
        }

        public void Update(int id, Teacher c)
        {
            var sub = MuseumContext.Teachers.Where(p => id == p.Id).Single();
            sub.FullName = c.FullName;
            sub.Subject = c.Subject;
            MuseumContext.SaveChanges();
        }

        public Teacher GetById(int id)
        {
            var sub = MuseumContext.Teachers.Where(p => id == p.Id).Single();

            return sub;
        }

        public MuseumContext MuseumContext
        {
            get { return Context as MuseumContext; }
        }
    }
}
