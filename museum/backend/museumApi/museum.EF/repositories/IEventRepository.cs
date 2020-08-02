using museum.EF.entities;
using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.EF.repositories
{
    public interface IEventRepository
    {
        List<Reservation> GetEvents();
        void Post(Reservation c);
        void Delete(int id);
        void Update(int id, Reservation c);
        Reservation GetById(int id);
    }
}
