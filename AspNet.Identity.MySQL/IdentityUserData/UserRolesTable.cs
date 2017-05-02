using System.Collections.Generic;
using AspNet.Identity.MySQL.Database;

namespace AspNet.Identity.MySQL.IdentityUserData
{
    /// <summary>
    ///     Class that represents the UserRoles table in the MySQL Database
    /// </summary>
    public class UserRolesTable
    {
        private readonly MySQLDatabase _database;

        /// <summary>
        ///     Constructor that takes a MySQLDatabase instance
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        ///     Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            var roles = new List<string>();
            var commandText =
                "Select Roles.Name from UserRoles, Roles where UserRoles.UserId = @userId and UserRoles.RoleId = Roles.Id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
                roles.Add(row["Name"]);

            return roles;
        }

        /// <summary>
        ///     Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            var commandText = "Delete from UserRoles where UserId = @userId";
            var parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        ///     Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            var commandText = "Insert into UserRoles (UserId, RoleId) values (@userId, @roleId)";
            var parameters = new Dictionary<string, object>();
            parameters.Add("userId", user.Id);
            parameters.Add("roleId", roleId);

            return _database.Execute(commandText, parameters);
        }
    }
}