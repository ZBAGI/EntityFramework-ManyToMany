using EntityFramework_ManyToMany.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_ManyToMany
{
    public class Database : DbContext
    {
        private static readonly string Server = "localhost";
        private static readonly int Port = 3130;
        private static readonly string UserName = "root";
        private static readonly string Password = "password";
        private static readonly string DatabaseName = "test";

        public Database() : base() { }

        public void Init()
        {
            if (Database.EnsureCreated())
            {
                var randomUser1 = new User("Lorem", "Ipsum", "Lorem@Ipsum.com");
                var randomUser2 = new User("Dolor", "Sit", "dolor@sit.com");
                var randomUser3 = new User("Consectetur", "Adipiscing", "consectetur@adipiscing.com");
                var randomUser4 = new User("Odio", "Vulputate", "odio@vulputate.com");
                var randomUser5 = new User("Sagittis", "Morbi", "sagittis@morbi.com");

                var rootRole = new Role("Root");
                randomUser1.Roles.Add(rootRole);
                randomUser2.Roles.Add(rootRole);

                var adminRole = new Role("Admin");
                randomUser1.Roles.Add(adminRole);
                randomUser2.Roles.Add(adminRole);
                randomUser3.Roles.Add(adminRole);

                var vipRole = new Role("VIP");
                randomUser1.Roles.Add(vipRole);
                randomUser2.Roles.Add(vipRole);
                randomUser3.Roles.Add(vipRole);
                randomUser4.Roles.Add(vipRole);


                Users.AddRange(randomUser1, randomUser2, randomUser3, randomUser4, randomUser5);
                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"Server={Server};Port={Port.ToString()};Database={DatabaseName};Uid={UserName};Pwd={Password};SslMode=Preferred;charset=utf8");
            //EF Many To Many supports lazy loading proxy
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoleJoint>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRoleJoint>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoleJoints);

            modelBuilder.Entity<UserRoleJoint>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoleJoints);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
