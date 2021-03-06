﻿using System;
using System.Collections.Generic;
using AspNet.Identity.MySQL.Database;

namespace AspNet.Identity.MySQL.IdentityUserData
{
    /// <summary>
    ///     Class that represents the Role table in the MySQL Database
    /// </summary>
    public class RoleTable
    {
        private readonly MySQLDatabase _database;

        /// <summary>
        ///     Constructor that takes a MySQLDatabase instance
        /// </summary>
        /// <param name="database"></param>
        public RoleTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        ///     Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(string roleId)
        {
            var commandText = "Delete from Roles where Id = @id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        ///     Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public int Insert(IdentityRole role)
        {
            var commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            var parameters = new Dictionary<string, object>();
            parameters.Add("@name", role.Name);
            parameters.Add("@id", role.Id);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        ///     Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(string roleId)
        {
            var commandText = "Select Name from Roles where Id = @id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        ///     Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public string GetRoleId(string roleName)
        {
            string roleId = null;
            var commandText = "Select Id from Roles where Name = @name";
            var parameters = new Dictionary<string, object> {{"@name", roleName}};

            var result = _database.QueryValue(commandText, parameters);
            if (result != null)
                return Convert.ToString(result);

            return roleId;
        }

        /// <summary>
        ///     Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            IdentityRole role = null;

            if (roleName != null)
                role = new IdentityRole(roleName, roleId);

            return role;
        }

        /// <summary>
        ///     Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole role = null;

            if (roleId != null)
                role = new IdentityRole(roleName, roleId);

            return role;
        }

        public int Update(IdentityRole role)
        {
            var commandText = "Update Roles set Name = @name where Id = @id";
            var parameters = new Dictionary<string, object>();
            parameters.Add("@id", role.Id);

            return _database.Execute(commandText, parameters);
        }
    }
}