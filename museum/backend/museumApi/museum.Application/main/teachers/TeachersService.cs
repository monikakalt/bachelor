using AutoMapper;
using museum.Application.main.teachers.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main.teachers
{
    public class TeachersService: ITeachersService
    {
        protected readonly ITeachersRepository _repository;
        protected readonly IMapper _mapper;
        public TeachersService(ITeachersRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public IEnumerable<TeacherDTO> GetAllTeachers()
        {
            var list = _repository.GetTeachers();
            var listDto = _mapper.Map<List<TeacherDTO>>(list);
            return listDto;
        }

        public Teacher PostTeacher(TeacherDTO item)
        {
            var sub = _mapper.Map<Teacher>(item);
            _repository.Post(sub);
            return sub;
        }

        public void DeleteTeacher(int id)
        {
            _repository.Delete(id);
        }

        public TeacherDTO GetTeacherById(int id)
        {
            var s = _repository.GetById(id);
            var dto = _mapper.Map<TeacherDTO>(s);
            return dto;
        }

        public void UpdateTeacher(int id, TeacherDTO s)
        {
            var sub = _mapper.Map<Teacher>(s);
            _repository.Update(id, sub);
        }
    }
}
