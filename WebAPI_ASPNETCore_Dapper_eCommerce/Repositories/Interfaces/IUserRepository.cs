using System.Data.SqlClient;
using WebAPI_ASPNETCore_Dapper_eCommerce.Models;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public List<User> Get();
        public User Get(int id);
        public void Insert(User user);
        public void Update(User user);
        public void Delete(int id);
    }
}
