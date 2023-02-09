using WebAPI_ASPNETCore_Dapper_eCommerce.Models;
using WebAPI_ASPNETCore_Dapper_eCommerce.Repositories.Interfaces;
using WebAPI_ASPNETCore_Dapper_eCommerce.Repositories;
using WebAPI_ASPNETCore_Dapper_eCommerce.Services.Interfaces;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Services
{
    public class UserContribService : IUserContribService
    {
        private IUserRepository _userRepository;

        public UserContribService()
        {
            _userRepository = new UserContribRepository();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.Get();
        }

        public User GetUserById(int id)
        {
            return _userRepository.Get(id);
        }

        public void InsertUser(User user)
        {
            _userRepository.Insert(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }
        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }
    }
}
