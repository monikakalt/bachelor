using museum.Application.main.users.dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main
{
    public interface IUsersService
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUserById(int id);
        void Save(UserDTO user);
        void UpdateUser(UserDTO user, string password = null);
        UserDTO Authenticate(string email, string password);
    }
}
