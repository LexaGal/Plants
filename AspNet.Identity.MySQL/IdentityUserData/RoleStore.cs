﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Identity.MySQL.Database;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.MySQL.IdentityUserData
{
    /// <summary>
    ///     Class that implements the key ASP.NET Identity role store iterfaces
    /// </summary>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole>
        where TRole : IdentityRole
    {
        private readonly RoleTable roleTable;


        /// <summary>
        ///     Default constructor that initializes a new MySQLDatabase
        ///     instance using the Default Connection string
        /// </summary>
        public RoleStore()
        {
            new RoleStore<TRole>(new MySQLDatabase());
        }

        /// <summary>
        ///     Constructor that takes a MySQLDatabase as argument
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(MySQLDatabase database)
        {
            Database = database;
            roleTable = new RoleTable(database);
        }

        public MySQLDatabase Database { get; private set; }

        public IQueryable<TRole> Roles
        {
            get { throw new NotImplementedException(); }
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            roleTable.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("user");

            roleTable.Delete(role.Id);

            return Task.FromResult<object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            var result = roleTable.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            var result = roleTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("user");

            roleTable.Update(role);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            if (Database != null)
            {
                Database.Dispose();
                Database = null;
            }
        }
    }
}