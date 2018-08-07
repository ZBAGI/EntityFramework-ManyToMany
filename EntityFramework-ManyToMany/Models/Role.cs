using EntityFramework_ManyToMany.Relationship;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework_ManyToMany.Models
{
    public class Role
    {
        public Role(string name) : this()
        {
            Name = name;
        }

        protected Role()
        {
            Users = new JointCollectionFacade<Role, User, UserRoleJoint>(this);
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [NotMapped, JsonIgnore] //To avoid circular reference we need to JsonIgnore
        public ICollection<User> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRoleJoint> UserRoleJoints { get; } = new List<UserRoleJoint>();
    }
}
