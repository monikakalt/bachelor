using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.EF.repositories
{
    public interface IClassesRepository
    {
        List<ClassInfo> GetClasses();
        void Post(ClassInfo c);
        void Delete(int id);
        void Update(int id, ClassInfo c);
        ClassInfo GetClassByName(string n);
        ClassInfo GetById(int id);
    }
}
