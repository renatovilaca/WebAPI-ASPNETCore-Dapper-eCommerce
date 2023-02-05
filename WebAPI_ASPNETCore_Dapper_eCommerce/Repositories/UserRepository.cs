using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebAPI_ASPNETCore_Dapper_eCommerce.Models;
using WebAPI_ASPNETCore_Dapper_eCommerce.Repositories.Interfaces;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IDbConnection _connection;
        private static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce;Integrated Security=True;";

        public UserRepository()
        {
            _connection = new SqlConnection(_connectionString);
        }

        public List<User> Get()
        {
            return _connection.Query<User>("SELECT * FROM Users").ToList();

        }

        public User Get(int id)
        {
            return _connection.QuerySingleOrDefault<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id});

        }

        public void Insert(User user)
        {

            string sql = "INSERT INTO Users (Name, Email, Gender, RG, CPF, Mother, Status, CreatedAt) VALUES (@Name, @Email, @Gender, @RG, @CPF, @Mother, @Status, @CreatedAt); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            user.Id = _connection.Query<int>(sql, user).Single();

        }

        public void Update(User user)
        {
            string sql = "UPDATE Users SET Name = @Name, Email = @Email, Gender = @Gender, RG = @RG, CPF = @CPF, Mother = @Mother, Status = @Status, @CreatedAt = CreatedAt WHERE Id = @Id";

            _connection.Execute(sql, user);

        }
        public void Delete(int id)
        {
            string sql = "DELETE FROM Users WHERE Id = @Id";

            _connection.Execute(sql, new { Id = id });

        }

    }
}
