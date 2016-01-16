using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Database.DatabaseStructure.Repository.Abstract;
using Database.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public override bool Edit(User value)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string fn, string ln, string passhash)
        {
            return Context.UsersSet.SingleOrDefault(user => user.FirstName == fn &&
                                                                user.LastName == ln &&
                                                                user.PasswordHash == passhash);
        }
    }
}