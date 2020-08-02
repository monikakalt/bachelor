using museum.Application.main.teachers.dto;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main.teachers
{
    public interface ITeachersService
    {
        IEnumerable<TeacherDTO> GetAllTeachers();
        TeacherDTO GetTeacherById(int id);
        void DeleteTeacher(int id);
        Teacher PostTeacher(TeacherDTO item);
        void UpdateTeacher(int id, TeacherDTO s);
    }
}
