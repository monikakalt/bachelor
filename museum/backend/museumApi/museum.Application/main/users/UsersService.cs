using AutoMapper;
using museum.Application.main.users.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main.users
{
    public class UsersService : IUsersService
    {
        protected readonly IUsersRepository _usersRepository;
        protected readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public UserDTO Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            var user = _usersRepository.Authenticate(email, password);

            // check if username exists
            if (user == null)
                return null;

            var userDto = _mapper.Map<UserDTO>(user);
            // authentication successful
            return userDto;
        }

        public void UpdateUser(UserDTO user, string password = null)
        {

        }
        public IEnumerable<UserDTO> GetAllUsers()
        {
            var list = _usersRepository.GetAllUsers();
            var listDto = _mapper.Map<List<UserDTO>>(list);
            return listDto;
        }

        public UserDTO GetUserById(int id)
        {
            var user = _usersRepository.GetUserById(id);
            var userDto = _mapper.Map<UserDTO>(user);
            return userDto;
        }

        public void Save(UserDTO user)
        {
            var userNormal = _mapper.Map<User>(user);
            _usersRepository.PostUser(userNormal);
        }
    }
}