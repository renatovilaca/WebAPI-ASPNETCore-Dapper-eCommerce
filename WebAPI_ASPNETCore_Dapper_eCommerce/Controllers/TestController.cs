using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using WebAPI_ASPNETCore_Dapper_eCommerce.Models;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Controllers
{
    [Route("api/Dapper/Tests")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IDbConnection _connection;
        private static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce;Integrated Security=True;";

        public TestController()
        {
            _connection = new SqlConnection(_connectionString);
        }

        [HttpGet("MultipleResultSets/{id}")]
        public IActionResult MultipleResultSets(int id)
        {
            string sql = "SELECT * FROM Users WHERE Id = @Id;" +
            "SELECT * FROM Contacts WHERE UserId = @Id;" +
            "SELECT * FROM DeliveryAddress WHERE UserId = @Id;" +
            "SELECT D.* FROM UsersDepartments UD INNER JOIN Departments D ON UD.DepartmentId = D.Id WHERE UD.UserId = @Id;";

            using (var multipleResultSets = _connection.QueryMultiple(sql, new { Id = id }))
            {
                var user = multipleResultSets.Read<User>().SingleOrDefault();      
                var contact = multipleResultSets.Read<Contact>().SingleOrDefault();
                var deliveryAddress = multipleResultSets.Read<DeliveryAddress>().ToList();
                var departments = multipleResultSets.Read<Department>().ToList();

                if (user != null)
                {
                    user.Contact = contact;
                    user.DeliveryAddresses= deliveryAddress;
                    user.Departments = departments;

                    return Ok(user);
                }
            }

            return NotFound();

        }

        [HttpGet("StoredProcedure/GetUsers")]
        public IActionResult StoredProcedureGetUsers()
        {
            var users = _connection.Query<User>("GetUsers", commandType: CommandType.StoredProcedure);

            return Ok(users);
        }

        [HttpGet("StoredProcedure/GetUser/{id}")]
        public IActionResult StoredProcedureGetUserById(int id)
        {
            var user = _connection.Query<User>("GetUserById", new { Id = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();

            return Ok(user);
        }

    }
}
