using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.Entities;

namespace ServiCar.Infrastructure.Persistence
{
    public class ServiCarApiContext
        : IdentityDbContext<User, Role, int>
    {
        private readonly ILookupNormalizer _normalizer;
        public ServiCarApiContext(DbContextOptions<ServiCarApiContext> options, ILookupNormalizer normalizer)
            : base(options)
        {
            _normalizer = normalizer;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.ToTable("Users");
            });

            builder.Entity<Role>(r =>
            {
                r.ToTable("Roles");
            });

            builder.Entity<UserRole>(ur =>
            {
                ur.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserRole<int>>(ur => 
            {
                ur.ToTable("UserRoles");
            });

            builder.Entity<IdentityRoleClaim<int>>(rc =>
            {
                rc.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserClaim<int>>(uc =>
            {
                uc.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<int>>(ul =>
            {
                ul.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<int>>(ut =>
            {
                ut.ToTable("UserTokens");
            });

            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            SeedUsers(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            User user = new User()
            {
                Id = 1,
                UserName = "ldecap",
                Email = "user@test.com",
                NormalizedEmail = _normalizer.NormalizeEmail("user@test.com"),
                NormalizedUserName = _normalizer.NormalizeName("ldecap"),
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                FirstName = "Leonardo",
                LastName = "Decaprio"
            };

            User worker = new User()
            {
                Id = 2,
                UserName = "tstark",
                NormalizedUserName = _normalizer.NormalizeName("tstark"),
                Email = "worker@test.com",
                NormalizedEmail = _normalizer.NormalizeEmail("worker@test.com"),
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                FirstName = "Tony",
                LastName = "Stark",
                IsCompanyWorker = true,
            };

            User admin = new User()
            {
                Id = 3,
                UserName = "cnolan",
                NormalizedUserName = _normalizer.NormalizeName("cnolan"),
                Email = "admin@test.com",
                NormalizedEmail = _normalizer.NormalizeEmail("admin@test.com"),
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                FirstName = "Christopher",
                LastName = "Nolan"
            };

            string password = "Test123!";

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            user.PasswordHash = passwordHasher.HashPassword(user, password);
            worker.PasswordHash = passwordHasher.HashPassword(worker, password);
            admin.PasswordHash = passwordHasher.HashPassword(admin, password);

            builder.Entity<User>().HasData(user, worker, admin);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User", ConcurrencyStamp = "1", NormalizedName = "USER" },
                new Role { Id = 2, Name = "Worker", ConcurrencyStamp = "2", NormalizedName = "WORKER" },
                new Role { Id = 3, Name = "Admin", ConcurrencyStamp = "3", NormalizedName = "ADMIN" });
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>() { RoleId = 1, UserId = 1 },
                new IdentityUserRole<int>() { RoleId = 2, UserId = 2 },
                new IdentityUserRole<int>() { RoleId = 3, UserId = 3 }
            );
        }
    }
}
