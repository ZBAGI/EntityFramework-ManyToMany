using EntityFramework_ManyToMany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFramework_ManyToMany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private Database Database { get; set; }

        public RoleController(Database database)
        {
            Database = database;
        }

        [HttpGet]
        public async Task<ActionResult<List<Role>>> Get()
        {
            var users = await Database.Roles.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Get(int id)
        {
            var role = await Database.Roles.SingleOrDefaultAsync(u => u.Id == id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async void Post(string name)
        {
            await Database.Roles.AddAsync(new Role(name));
        }

        [HttpPost("{roleId:int}/user/{userId:int}")]
        public async Task<ActionResult> Post([FromRoute] int userId, [FromRoute] int roleId)
        {
            var role = await Database.Roles.SingleOrDefaultAsync(u => u.Id == roleId);
            var user = await Database.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (role == null || user == null)
                return NotFound();

            //Many to many magic
            role.Users.Add(user);
            await Database.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var role = await Database.Roles.SingleOrDefaultAsync(u => u.Id == id);
            if (role == null)
                return NotFound();

            Database.Roles.Remove(role);
            await Database.SaveChangesAsync();

            return Ok();
        }
    }
}
