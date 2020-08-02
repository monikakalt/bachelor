using museum.Application.main.graduates.dto;
using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.Application.main.graduates
{
    public interface IGraduatesService
    {
        IEnumerable<GraduatesDTO> GetAllGraduates();
        GraduatesDTO GetGraduateById(int id);
        void DeleteGraduate(int id);
        Graduates PostGraduate(GraduatesDTO item);
        void UpdateGraduate(int id, GraduatesDTO s);
    }
}
