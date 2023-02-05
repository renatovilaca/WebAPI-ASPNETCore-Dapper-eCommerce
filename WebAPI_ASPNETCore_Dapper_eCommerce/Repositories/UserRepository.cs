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

            string sql = "SELECT * FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id";

            return _connection.Query<User, Contact, User>(sql,
               (user, contact) =>
               {
                   user.Contact = contact;
                   return user;
               }).ToList();

            //return _connection.Query<User>("SELECT * FROM Users").ToList();

        }

        public User Get(int id)
        {
         
            string sql = "SELECT * FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id WHERE U.Id = @Id";

            return _connection.Query<User, Contact, User>(sql, 
                (user, contact) => 
                { 
                    user.Contact = contact;
                    return user;
                },
                new { Id = id }).SingleOrDefault();

            //return _connection.QuerySingleOrDefault<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id});

        }

        public void Insert(User user)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql = "INSERT INTO Users (Name, Email, Gender, RG, CPF, Mother, Status, CreatedAt) VALUES (@Name, @Email, @Gender, @RG, @CPF, @Mother, @Status, @CreatedAt); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                user.Id = _connection.Query<int>(sql, user, transaction).Single();

                if (user.Contact != null)
                {
                    string sqlContact = "INSERT INTO Contacts (UserId, Phone, CellPhone) VALUES (@UserId, @Phone, @CellPhone); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    user.Contact.UserId = user.Id;
                    user.Contact.Id = _connection.Query<int>(sqlContact, user.Contact, transaction).Single();
                }

                transaction.Commit();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 

                try
                {
                    transaction.Rollback();
                }
                catch { }

            }
            finally
            { 
                _connection.Close(); 
            }


        }

        public void Update(User user)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sql = "UPDATE Users SET Name = @Name, Email = @Email, Gender = @Gender, RG = @RG, CPF = @CPF, Mother = @Mother, Status = @Status, @CreatedAt = CreatedAt WHERE Id = @Id;";

                _connection.Execute(sql, user, transaction);

                if (user.Contact != null)
                {
                    string sqlContact = "UPDATE Contacts SET Phone = @Phone, CellPhone = @CellPhone WHERE Id = @Id;";

                    _connection.Execute(sqlContact, user.Contact, transaction);
                }

                transaction.Commit();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                try
                {
                    transaction.Rollback();
                }
                catch { }

            }
            finally
            {
                _connection.Close();
            }


        }
        public void Delete(int id)
        {
            string sql = "DELETE FROM Users WHERE Id = @Id";

            _connection.Execute(sql, new { Id = id });

        }

    }
}
