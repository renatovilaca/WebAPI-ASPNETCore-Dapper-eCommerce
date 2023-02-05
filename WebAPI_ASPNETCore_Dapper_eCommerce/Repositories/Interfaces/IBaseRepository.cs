using WebAPI_ASPNETCore_Dapper_eCommerce.Models;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        public List<T> Get();
        public T Get(int id);
        public void Insert(T model);
        public void Update(T model);
        public void Delete(int id);
    }
}
