using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.EF.repositories
{
    public interface ITeachersRepository
    {
        List<Teacher> GetTeachers();
        void Post(Teacher t);
        void Delete(int id);
        void Update(int id, Teacher t);
        Teacher GetById(int id);
    }
}
