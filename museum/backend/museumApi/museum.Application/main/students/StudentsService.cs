using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using museum.Application.main.classes.dto;
using museum.Application.main.graduates.dto;
using museum.Application.main.students.dto;
using museum.Application.main.teachers.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using OfficeOpenXml;

namespace museum.Application.main.students
{
    public class StudentsService : IStudentsService
    {
        protected readonly IStudentsRepository _repository;
        protected readonly IClassesRepository _classesRepository;
        protected readonly IMapper _mapper;
        public StudentsService(IStudentsRepository repository, IMapper mapper, IClassesRepository classesRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _classesRepository = classesRepository;
        }
        public string ReadExcelPackageToString(ExcelPackage package, ExcelWorksheets worksheets)
        {
            var student = new StudentDTO();
            var studentClass = new ClassDTO();
            var teacher = new TeacherDTO();
            var graduate = new GraduatesDTO();

            for (int i = 0; i < worksheets.Count; i++)
            {
                var worksheet = worksheets[i];
                // laidos pavadinimas pvz. 164 laida ir metai
                var graduates = worksheet.Cells[1, 2].Value.ToString();
                if (!String.IsNullOrEmpty(graduates))
                {
                    graduate.Title = graduates.Split('(', ')')[1];
                    graduate.Year = GetYear(graduates);
                }

                // klasė
                var classProp = worksheet.Cells[2, 2].Value.ToString();
                if (!String.IsNullOrEmpty(classProp))
                {
                    var a = CheckForClass(classProp);
                }


                var rowCount = worksheet.Dimension?.Rows;
                var colCount = worksheet.Dimension?.Columns;

                if (!rowCount.HasValue || !colCount.HasValue)
                {
                    return string.Empty;
                }

                for (int row = 1; row <= rowCount.Value; row++)
                {
                    for (int col = 1; col <= colCount.Value; col++)
                    {
                       // sb.AppendFormat("{0}\t", worksheet.Cells[row, col].Value);
                    }
                }
                
            }

            return "a".ToString();
        }

        public int GetYear(string grad)
        {
            var yearString = grad.Substring(0, 4);
            try
            {
                return Convert.ToInt32(yearString);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public ClassInfo CheckForClass(string c)
        {
            var cl = _classesRepository.GetClassByName(c);
            if (cl == null)
            {
                cl.Title = c;
                _classesRepository.Post(cl);
            }
            return _classesRepository.GetClassByName(c);
        }
       
        public IEnumerable<StudentDTO> GetAllStudents()
        {
            var list = _repository.GetStudents();
            var listDto = _mapper.Map<List<StudentDTO>>(list);
            return listDto;
        }

        public Student PostStudent(StudentDTO item)
        {
            var sub = _mapper.Map<Student>(item);
            _repository.Post(sub);
            return sub;
        }

        public void DeleteStudent(int id)
        {
            _repository.Delete(id);
        }

        public StudentDTO GetStudentById(int id)
        {
            var s = _repository.GetById(id);
            var dto = _mapper.Map<StudentDTO>(s);
            return dto;
        }

        public void UpdateStudent(int id, StudentDTO s)
        {
            var sub = _mapper.Map<Student>(s);
            _repository.Update(id, sub);
        }
    }
}
