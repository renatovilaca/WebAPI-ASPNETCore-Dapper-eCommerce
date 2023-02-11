using Dapper.FluentMap.Mapping;
using System.Reflection.Metadata;
using WebAPI_ASPNETCore_Dapper_eCommerce.Models;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Mappers
{
    public class UserCustomMap : EntityMap<UserCustom>
    {
        public UserCustomMap()
        {
            Map(m => m.UserId).ToColumn("Id");
            Map(m => m.Mail).ToColumn("Email");
            Map(m => m.FullName).ToColumn("Name");
            Map(m => m.MothersName).ToColumn("Mother");
        }

    }
}
