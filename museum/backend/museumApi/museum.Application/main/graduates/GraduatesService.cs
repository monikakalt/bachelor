using AutoMapper;
using museum.Application.main.graduates.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main.graduates
{
    public class GraduatesService : IGraduatesService
    {
        protected readonly IGraduatesRepository _repository;
        protected readonly IMapper _mapper;
        public GraduatesService(IGraduatesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<GraduatesDTO> GetAllGraduates()
        {
            var list = _repository.GetGraduates();
            var listDto = _mapper.Map<List<GraduatesDTO>>(list);
            return listDto;
        }

        public Graduates PostGraduate(GraduatesDTO item)
        {
            var sub = _mapper.Map<Graduates>(item);
            _repository.Post(sub);
            return sub;
        }

        public void DeleteGraduate(int id)
        {
            _repository.Delete(id);
        }

        public GraduatesDTO GetGraduateById(int id)
        {
            var s = _repository.GetById(id);
            var dto = _mapper.Map<GraduatesDTO>(s);
            return dto;
        }

        public void UpdateGraduate(int id, GraduatesDTO s)
        {
            var sub = _mapper.Map<Graduates>(s);
            _repository.Update(id, sub);
        }
    }
}