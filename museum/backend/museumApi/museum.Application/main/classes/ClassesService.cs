using AutoMapper;
using museum.Application.main.classes.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.Application.main.classes
{
    public class ClassesService: IClassesService
    {
        protected readonly IClassesRepository _repository;
        protected readonly IMapper _mapper;

        public ClassesService(IClassesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ClassDTO GetClassById(int id)
        {
            var s = _repository.GetById(id);
            var dto = _mapper.Map<ClassDTO>(s);
            return dto;
        }

        public IEnumerable<ClassDTO> GetAllClasses()
        {
            var list = _repository.GetClasses();
            var listDto = _mapper.Map<List<ClassDTO>>(list);
            return listDto;
        }

        public ClassInfo PostClass(ClassDTO item)
        {
            var sub = _mapper.Map<ClassInfo>(item);
            _repository.Post(sub);
            return sub;
        }

        public void DeleteClass(int id)
        {
            _repository.Delete(id);
        }

        public void UpdateClass(int id, ClassDTO c)
        {
            var sub = _mapper.Map<ClassInfo>(c);
            _repository.Update(id, sub);
        }
    }
}
