using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework_ManyToMany.Relationship
{
    public interface IJointEntity<TEntity>
    {
        TEntity Navigation { get; set; }
    }
}
