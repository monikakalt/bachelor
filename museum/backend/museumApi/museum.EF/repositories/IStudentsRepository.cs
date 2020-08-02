using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.EF.repositories
{
    public interface IStudentsRepository
    {
        List<Student> GetStudents();
        void Post(Student c);
        void Delete(int id);
        void Update(int id, Student c);
        Student GetById(int id);
    }
}
