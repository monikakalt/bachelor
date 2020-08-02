using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.EF.repositories
{
    public interface IUsersRepository
    {
        List<User> GetAllUsers();
        void PostUser(User c);
        void DeleteUser(int id);
        void UpdateUser(int id, User c);
        User GetUserById(int id);
        User Authenticate(string email, string password);
    }
}
