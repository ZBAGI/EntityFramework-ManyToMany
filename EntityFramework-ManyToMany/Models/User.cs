using EntityFramework_ManyToMany.Relationship;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework_ManyToMany.Models
{
    public class User
    {
        public User(string name, string surname, string email) : this()
        {
            Name = name;
            Surname = surname;
            Email = surname;
        }

        protected User()
        {
            Roles = new JointCollectionFacade<User, Role, UserRoleJoint>(this);
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public ICollection<Role> Roles { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRoleJoint> UserRoleJoints { get; } = new List<UserRoleJoint>();
    }
}
