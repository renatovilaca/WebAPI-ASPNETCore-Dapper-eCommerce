using WebAPI_ASPNETCore_Dapper_eCommerce.Models;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserById(int id); 
        void InsertUser(User user);
        void UpdateUser(User user); 
        void DeleteUser(int id);
    }
}
