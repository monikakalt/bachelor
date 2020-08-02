using Microsoft.AspNetCore.Http;
using museum.Application.main.students.dto;
using museumApi.EF.entities;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace museum.Application.main
{
    public interface IStudentsService
    {
        string ReadExcelPackageToString(ExcelPackage package, ExcelWorksheets worksheets);
        IEnumerable<StudentDTO> GetAllStudents();
        StudentDTO GetStudentById(int id);
        void DeleteStudent(int id);
        Student PostStudent(StudentDTO item);
        void UpdateStudent(int id, StudentDTO s);
    }
}
