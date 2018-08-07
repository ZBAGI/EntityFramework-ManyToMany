using EntityFramework_ManyToMany.Relationship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework_ManyToMany.Models
{
    public class UserRoleJoint : IJointEntity<User>, IJointEntity<Role>
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        User IJointEntity<User>.Navigation
        {
            get => User;
            set => User = value;
        }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        Role IJointEntity<Role>.Navigation
        {
            get => Role;
            set => Role = value;
        }
    }
}
