namespace museum.EF.repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using museumApi.EF.entities;

    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(MuseumContext context)
            : base(context)
        {
        }

        public List<Student> GetStudents()
        {
            return this.MuseumContext.Students
                .Include(x => x.FkClassNavigation)
                .ToList();
        }

        public void Post(Student c)
        {
            this.MuseumContext.Add(c);
            this.MuseumContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var c = this.MuseumContext.Students.SingleOrDefault(p => p.Id == id);
            this.MuseumContext.Remove(c);
            this.MuseumContext.SaveChanges();
        }

        public void Update(int id, Student c)
        {
            var s = this.MuseumContext.Students.Where(p => id == p.Id).Single();
            s.Address = c.Address;
            s.Birthdate = c.Birthdate;
            s.Email = c.Email;
            s.FkClass = c.FkClass;
            s.FkGraduates = c.FkGraduates;
            s.FullName = c.FullName;
            s.Phone = c.Phone;
            s.SurnameAfterMarriage = c.SurnameAfterMarriage;
            s.Workplace = c.Workplace;
            s.Comment = c.Comment;
            MuseumContext.SaveChanges();
        }

        public Student GetById(int id)
        {
            var sub = this.MuseumContext.Students.Where(p => id == p.Id).Single();

            return sub;
        }

        public MuseumContext MuseumContext
        {
            get { return this.Context as MuseumContext; }
        }
    }
}
