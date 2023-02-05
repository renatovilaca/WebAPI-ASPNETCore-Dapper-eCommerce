using WebAPI_ASPNETCore_Dapper_eCommerce.Models;
using WebAPI_ASPNETCore_Dapper_eCommerce.Repositories.Interfaces;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<User> _dbUsers = new()
        {
            new User() { Id = 1, Name = "Pamela Lima", Email = "pam@mail.com"},
            new User() { Id = 2, Name = "Joao Costa", Email = "jc@mail.com" },
            new User() { Id = 3, Name = "Jessica Silva", Email = "jessy@mail.com"}
        };

        public List<User> Get()
        {
            return _dbUsers;
        }

        public User Get(int id)
        {
            return _dbUsers.FirstOrDefault(u => u.Id.Equals(id));
        }

        public void Insert(User user)
        {
            user.Id = 1;
            var lastUser = _dbUsers.LastOrDefault();

            if (lastUser != null)
                user.Id += lastUser.Id;

            _dbUsers.Add(user);
        }

        public void Update(User user)
        {
            _dbUsers.Remove(_dbUsers.FirstOrDefault(u => u.Id.Equals(user.Id)));
            _dbUsers.Add(user);
        }
        public void Delete(int id)
        {
            _dbUsers.Remove(_dbUsers.FirstOrDefault(u => u.Id.Equals(id)));
        }
    }
}
