using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.EF.repositories
{
    public interface IGraduatesRepository
    {
        List<Graduates> GetGraduates();
        void Post(Graduates c);
        void Delete(int id);
        void Update(int id, Graduates c);
        Graduates GetById(int id);
    }
}
