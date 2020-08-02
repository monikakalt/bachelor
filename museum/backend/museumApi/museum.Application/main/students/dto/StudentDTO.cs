using museumApi.EF.entities;
using System;

namespace museum.Application.main.students.dto
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string SurnameAfterMarriage { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Workplace { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public int? FkGraduates { get; set; }
        public int FkClass { get; set; }
        public int? FkClassNavigationFkTeacher { get; set; }
        public Graduates FkGraduatesNavigation { get; set; }
    }
}
