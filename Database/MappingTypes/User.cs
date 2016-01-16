using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace Database.MappingTypes
{
    public class User
    {
        public User() { }
        public User(string fn, string ln, string em, string passHash)
        {
            Id = Guid.NewGuid();
            FirstName = fn;
            LastName = ln;
            Email = em;
            PasswordHash = passHash;
        }

        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}