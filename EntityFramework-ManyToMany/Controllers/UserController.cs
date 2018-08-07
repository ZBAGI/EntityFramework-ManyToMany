using EntityFramework_ManyToMany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFramework_ManyToMany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private Database Database { get; set; }

        public UserController(Database database)
        {
            Database = database;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(await Database.Users.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await Database.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async void Post(string name, string surname, string email)
        {
            await Database.Users.AddAsync(new User(name, surname, email));
        }

        [HttpPost("{userId:int}/role/{roleId:int}")]
        public async Task<ActionResult> Post([FromRoute] int userId, [FromRoute] int roleId)
        {
            var user = await Database.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var role = await Database.Roles.SingleOrDefaultAsync(u => u.Id == roleId);

            if (user == null || role == null)
                return NotFound();

            //Many to many magic
            user.Roles.Add(role);
            await Database.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await Database.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();

            Database.Users.Remove(user);
            await Database.SaveChangesAsync();

            return Ok();
        }
    }
}
