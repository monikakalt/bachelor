using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.EF.repositories
{
    public interface IChroniclesRepository
    {
        List<Chronicle> GetAll();
        Chronicle GetRecent();
        void Post(Chronicle c);
        void Delete(int id);
        void Update(int id, Chronicle c);
        Chronicle GetById(int id);
    }
}