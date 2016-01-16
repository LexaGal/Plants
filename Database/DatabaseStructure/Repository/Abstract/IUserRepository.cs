using Database.MappingTypes;

namespace Database.DatabaseStructure.Repository.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(string fn, string ln, string pass);
    }
}