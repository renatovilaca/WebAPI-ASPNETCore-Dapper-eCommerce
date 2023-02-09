using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SqlClient;
using WebAPI_ASPNETCore_Dapper_eCommerce.Models;
using WebAPI_ASPNETCore_Dapper_eCommerce.Repositories.Interfaces;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Repositories
{
    public class UserContribRepository : IUserRepository
    {
        private IDbConnection _connection;
        private static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce;Integrated Security=True;";

        public UserContribRepository()
        {
            _connection = new SqlConnection(_connectionString);
        }

        public List<User> Get()
        {
            return _connection.GetAll<User>().ToList();
        }

        public User Get(int id)
        {
            return _connection.Get<User>(id);
        }

        public void Insert(User user)
        {

            user.Id = Convert.ToInt32(_connection.Insert(user));
        }

        public void Update(User user)
        {

            _connection.Update(user);

        }
        public void Delete(int id)
        {
            _connection.Delete(Get(id));
        }
    }
}
