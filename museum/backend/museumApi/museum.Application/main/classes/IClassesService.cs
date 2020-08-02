using museum.Application.main.classes.dto;
using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.Application.main.classes
{
    public interface IClassesService
    {
        IEnumerable<ClassDTO> GetAllClasses();
        ClassDTO GetClassById(int id);
        void DeleteClass(int id);
        ClassInfo PostClass(ClassDTO item);
        void UpdateClass(int id, ClassDTO c);
    }
}
