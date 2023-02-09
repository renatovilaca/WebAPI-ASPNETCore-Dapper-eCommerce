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
            //Keeping commented codes for example

            //Single table
            //return _connection.Query<User>("SELECT * FROM Users").ToList();

            /*
            //User with contact
            string sql = "SELECT * FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id";

            return _connection.Query<User, Contact, User>(sql,
               (user, contact) =>
               {
                   user.Contact = contact;
                   return user;
               }).ToList();
            */

            //User with Contact and Delivery Adressess
            /*
            string sql = "SELECT * FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id LEFT JOIN DeliveryAddress DA ON DA.UserId = U.Id";

            List<User> users = new();

            _connection.Query<User, Contact, DeliveryAddress, User>(sql,
                (user, contact, deliveryAddress) =>
                {
                    if (users.SingleOrDefault(u => u.Id == user.Id) == null)
                    {
                        user.DeliveryAddresses = new List<DeliveryAddress>();
                        user.Contact = contact;
                        users.Add(user);
                    }
                    else
                    {
                        user = users.SingleOrDefault(u => u.Id == user.Id);

                    }

                    if (user != null && deliveryAddress != null)
                         user.DeliveryAddresses.Add(deliveryAddress);

                    return user;
                });

            return users;
            */

            //User with Contact and Delivery Adressess and Departments
            string sql = "SELECT U.*,C.*,DA.*,D.* FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id LEFT JOIN DeliveryAddress DA ON DA.UserId = U.Id LEFT JOIN UsersDepartments UD ON UD.UserId = U.Id LEFT JOIN Departments D ON UD.DepartmentId = D.Id";

            List<User> users = new();

            _connection.Query<User, Contact, DeliveryAddress, Department, User>(sql,
                (user, contact, deliveryAddress, department) =>
                {
                    if (users.SingleOrDefault(u => u.Id == user.Id) == null)
                    {
                        user.Departments = new List<Department>();
                        user.DeliveryAddresses = new List<DeliveryAddress>();

                        user.Contact = contact;
                        users.Add(user);
                    }
                    else
                    {
                        user = users.SingleOrDefault(u => u.Id == user.Id);
                    }

                    if (user.DeliveryAddresses.SingleOrDefault(d => d.Id == deliveryAddress.Id) == null)
                        user.DeliveryAddresses.Add(deliveryAddress);

                    if (user.Departments.SingleOrDefault(d => d.Id == department.Id) == null)
                        user.Departments.Add(department);

                    return user;
                });

            return users;

        }

        public User Get(int id)
        {
            //Keeping commented codes for example

            //Single table
            //return _connection.QuerySingleOrDefault<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id});

            /*
            string sql = "SELECT * FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id WHERE U.Id = @Id";

            return _connection.Query<User, Contact, User>(sql, 
                (user, contact) => 
                { 
                    user.Contact = contact;
                    return user;
                },
                new { Id = id }).SingleOrDefault();
            */

            //User with Contact and Delivery Addresses
            /*
            string sql = "SELECT * FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id LEFT JOIN DeliveryAddress DA ON DA.UserId = U.Id WHERE U.Id = @Id";

            List<User> users = new();

            _connection.Query<User, Contact, DeliveryAddress, User>(sql,
                (user, contact, deliveryAddress) =>
                {
                    if (users.SingleOrDefault(u => u.Id == user.Id) == null)
                    {
                        user.DeliveryAddresses = new List<DeliveryAddress>();
                        user.Contact = contact;
                        users.Add(user);
                    }
                    else
                    {
                        user = users.SingleOrDefault(u => u.Id == user.Id);

                    }

                    if (user != null && deliveryAddress != null)
                        user.DeliveryAddresses.Add(deliveryAddress);

                    return user;
                },
                new { Id = id });

            return users.SingleOrDefault();
            */

            string sql = "SELECT U.*,C.*,DA.*,D.* FROM Users U LEFT JOIN Contacts C ON C.UserId = U.Id LEFT JOIN DeliveryAddress DA ON DA.UserId = U.Id LEFT JOIN UsersDepartments UD ON UD.UserId = U.Id LEFT JOIN Departments D ON UD.DepartmentId = D.Id WHERE U.Id = @Id";

            List<User> users = new();

            _connection.Query<User, Contact, DeliveryAddress, Department, User>(sql,
                (user, contact, deliveryAddress, department) =>
                {
                    if (users.SingleOrDefault(u => u.Id == user.Id) == null)
                    {
                        user.Departments = new List<Department>();
                        user.DeliveryAddresses = new List<DeliveryAddress>();

                        user.Contact = contact;
                        users.Add(user);
                    }
                    else
                    {
                        user = users.SingleOrDefault(u => u.Id == user.Id);
                    }

                    if (user.DeliveryAddresses.SingleOrDefault(d => d.Id == deliveryAddress.Id) == null)
                        user.DeliveryAddresses.Add(deliveryAddress);

                    if (user.Departments.SingleOrDefault(d => d.Id == department.Id) == null)
                        user.Departments.Add(department);

                    return user;
                },
                new { Id = id });

            return users.SingleOrDefault();


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

                if (user.DeliveryAddresses != null && user.DeliveryAddresses.Count() > 0)
                {
                    string sqlAddress = "INSERT INTO DeliveryAddress (UserId, AddressTitle, Zipcode, State, City, District, Address, Number, Address2) VALUES (@UserId, @AddressTitle, @Zipcode, @State, @City, @District, @Address, @Number, @Address2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    foreach (var address in user.DeliveryAddresses)
                    {
                        address.UserId = user.Id;
                        address.Id = _connection.Query<int>(sqlAddress, address, transaction).Single();

                    }
                }

                if (user.Departments != null & user.Departments.Count()> 0)
                {
                    string sqlUserDepartment = "INSERT INTO UsersDepartments (UserId, DepartmentId) VALUES (@UserId, @DepartmentId);";

                    foreach (var userDepartment in user.Departments)
                    {
                        _connection.Execute(sqlUserDepartment, new { UserId = user.Id, DepartmentId = userDepartment.Id }, transaction);

                    }
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

                if (user.DeliveryAddresses != null)
                {
                    string sqlDeleteAddress = "DELETE FROM DeliveryAddress WHERE UserId = @Id;";

                    _connection.Execute(sqlDeleteAddress, user, transaction);

                }

                if (user.DeliveryAddresses != null && user.DeliveryAddresses.Count() > 0)
                {
                    string sqlAddress = "INSERT INTO DeliveryAddress (UserId, AddressTitle, Zipcode, State, City, District, Address, Number, Address2) VALUES (@UserId, @AddressTitle, @Zipcode, @State, @City, @District, @Address, @Number, @Address2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    foreach (var address in user.DeliveryAddresses)
                    {
                        address.UserId = user.Id;
                        address.Id = _connection.Query<int>(sqlAddress, address, transaction).Single();

                    }
                }

                if (user.Departments != null)
                {
                    string sqlDeleteDepartments = "DELETE FROM UsersDepartments WHERE UserId = @Id;";

                    _connection.Execute(sqlDeleteDepartments, user, transaction);

                }

                if (user.Departments != null & user.Departments.Count() > 0)
                {
                    string sqlUserDepartment = "INSERT INTO UsersDepartments (UserId, DepartmentId) VALUES (@UserId, @DepartmentId);";

                    foreach (var userDepartment in user.Departments)
                    {
                        _connection.Execute(sqlUserDepartment, new { UserId = user.Id, DepartmentId = userDepartment.Id }, transaction);

                    }
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
