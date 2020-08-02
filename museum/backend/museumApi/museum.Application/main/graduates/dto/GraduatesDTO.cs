using museum.Application.main.students.dto;
using System.Collections.Generic;

namespace museum.Application.main.graduates.dto
{
    public class GraduatesDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public IEnumerable<StudentDTO> Students { get; set; }
    }
}
