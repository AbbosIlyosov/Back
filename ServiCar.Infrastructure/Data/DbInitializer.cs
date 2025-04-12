using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.Entities;
using ServiCar.Infrastructure.Persistence;

namespace ServiCar.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void SeedData(ServiCarApiContext context)
        {
            // Ensure the database exists and is up to date
            context.Database.Migrate();

            SeedUsers(context);
            SeedRoles(context);
            SeedUserRoles(context);
            SeedWorkingTimes(context);
            SeedLocations(context);
            SeedCategories(context);
        }

        private static void SeedUsers(ServiCarApiContext context)
        {
            string password = "Test123!";
            List<User> users = new List<User>();
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            if (!context.Users.Any(u => u.NormalizedEmail == "USER@TEST.COM"))
            {
                var user = new User()
                {
                    UserName = "ldecap",
                    Email = "user@test.com",
                    NormalizedEmail = "USER@TEST.COM",
                    NormalizedUserName = "LDECAP",
                    LockoutEnabled = false,
                    PhoneNumber = "1234567890",
                    FirstName = "Leonardo",
                    LastName = "Decaprio"
                };

                user.PasswordHash = passwordHasher.HashPassword(user, password);
                users.Add(user);
            }

            if(!context.Users.Any(u => u.NormalizedEmail == "WORKER@TEST.COM"))
            {
                var user = new User()
                {
                    UserName = "tstark",
                    NormalizedUserName = "TSTARK",
                    Email = "worker@test.com",
                    NormalizedEmail = "WORKER@TEST.COM",
                    LockoutEnabled = false,
                    PhoneNumber = "1234567890",
                    FirstName = "Tony",
                    LastName = "Stark",
                    IsCompanyWorker = true,
                };

                user.PasswordHash = passwordHasher.HashPassword(user, password);
                users.Add(user);
            }

            if(!context.Users.Any(u => u.NormalizedEmail == "ADMIN@TEST.COM"))
            {
                var user = new User()
                {
                    UserName = "cnolan",
                    NormalizedUserName = "CNOLAN",
                    Email = "admin@test.com",
                    NormalizedEmail = "ADMIN@TEST.COM",
                    LockoutEnabled = false,
                    PhoneNumber = "1234567890",
                    FirstName = "Christopher",
                    LastName = "Nolan"
                };

                user.PasswordHash = passwordHasher.HashPassword(user, password);
                users.Add(user);
            }

            if(users.Count > 0)
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        private static void SeedRoles(ServiCarApiContext context)
        {
            List<Role> roles = new List<Role>();

            if(!context.Roles.Any(r => r.NormalizedName == "USER"))
            {
                roles.Add(new Role { Name = "User", ConcurrencyStamp = "1", NormalizedName = "USER" });
            }

            if(!context.Roles.Any(r => r.NormalizedName == "WORKER"))
            {
                roles.Add(new Role { Name = "Worker", ConcurrencyStamp = "2", NormalizedName = "WORKER" });
            }

            if (!context.Roles.Any(r => r.NormalizedName == "ADMIN"))
            {
                roles.Add(new Role { Name = "Admin", ConcurrencyStamp = "3", NormalizedName = "ADMIN" });
            }

            if (roles.Count > 0)
            {
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }

        private static void SeedUserRoles(ServiCarApiContext context)
        {
            var users = context.Users.ToList();
            var roles = context.Roles.ToList();

            var user = users.FirstOrDefault(u => u.NormalizedEmail == "USER@TEST.COM");
            var worker = users.FirstOrDefault(u => u.NormalizedEmail == "WORKER@TEST.COM");
            var admin = users.FirstOrDefault(u => u.NormalizedEmail == "ADMIN@TEST.COM");

            var userRole = roles.FirstOrDefault(r => r.NormalizedName == "USER");
            var workerRole = roles.FirstOrDefault(r => r.NormalizedName == "WORKER");
            var adminRole = roles.FirstOrDefault(r => r.NormalizedName == "ADMIN");

            var userRoles = new List<UserRole>();

            if (user != null && userRole != null && !context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == userRole.Id))
            {
                userRoles.Add(new UserRole() { UserId = user.Id, RoleId = userRole.Id });
            }

            if (worker != null && workerRole != null && !context.UserRoles.Any(ur => ur.UserId == worker.Id && ur.RoleId == workerRole.Id))
            {
                userRoles.Add(new UserRole() { UserId = worker.Id, RoleId = workerRole.Id });
            }

            if (admin != null && adminRole != null && !context.UserRoles.Any(ur => ur.UserId == admin.Id && ur.RoleId == adminRole.Id))
            {
                userRoles.Add(new UserRole() { UserId = admin.Id, RoleId = adminRole.Id });
            }

            if (userRoles.Count > 0)
            {
                context.UserRoles.AddRange(userRoles);
                context.SaveChanges();
            }
        }

        private static void SeedWorkingTimes(ServiCarApiContext context)
        {
            if (!context.WorkingTimes.Any())
            {
                var workingTimes = new List<WorkingTime>
                {
                    new WorkingTime { Name = "Morning Shift", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(12, 0, 0) },
                    new WorkingTime { Name = "Afternoon Shift", StartTime = new TimeOnly(13, 0, 0), EndTime = new TimeOnly(17, 0, 0) },
                    new WorkingTime { Name = "Evening Shift", StartTime = new TimeOnly(18, 0, 0), EndTime = new TimeOnly(22, 0, 0) },
                    new WorkingTime { Name = "Full Day Service", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(20, 0, 0) },
                    new WorkingTime { Name = "Weekend Service", StartTime = new TimeOnly(10, 0, 0), EndTime = new TimeOnly(16, 0, 0) },
                    new WorkingTime { Name = "Night Shift", StartTime = new TimeOnly(22, 0, 0), EndTime = new TimeOnly(6, 0, 0) }
                };

                context.WorkingTimes.AddRange(workingTimes);
                context.SaveChanges();
            }
        }

        private static void SeedLocations(ServiCarApiContext context)
        {
            if (!context.Locations.Any())
            {
                var locationsNP = new List<Location>
                {
                    new Location
                    {
                        City = "Kathmandu",
                        District = "Kathmandu",
                        Latitude = 27.700769m,
                        Longitude = 85.300140m
                    },
                    new Location
                    {
                        City = "Pokhara",
                        District = "Kaski",
                        Latitude = 28.209583m,
                        Longitude = 83.985593m
                    },
                    new Location
                    {
                        City = "Lalitpur",
                        District = "Lalitpur",
                        Latitude = 27.666075m,
                        Longitude = 85.316216m
                    },
                    new Location
                    {
                        City = "Bharatpur",
                        District = "Chitwan",
                        Latitude = 27.674733m,
                        Longitude = 84.432706m
                    },
                    new Location
                    {
                        City = "Butwal",
                        District = "Rupandehi",
                        Latitude = 27.700001m,
                        Longitude = 83.450001m
                    },
                    new Location
                    {
                        City = "Biratnagar",
                        District = "Morang",
                        Latitude = 26.455048m,
                        Longitude = 87.270065m
                    },
                    new Location
                    {
                        City = "Dharan",
                        District = "Sunsari",
                        Latitude = 26.812288m,
                        Longitude = 87.283863m
                    },
                    new Location
                    {
                        City = "Hetauda",
                        District = "Makwanpur",
                        Latitude = 27.428732m,
                        Longitude = 85.032242m
                    }
                };

                var locationsUZ = new List<Location>
                {
                    new Location
                    {
                        City = "Tashkent",
                        District = "Tashkent",
                        Latitude = 41.299495m,
                        Longitude = 69.240073m
                    },
                    new Location
                    {
                        City = "Samarkand",
                        District = "Samarkand",
                        Latitude = 39.654134m,
                        Longitude = 66.974973m
                    },
                    new Location
                    {
                        City = "Bukhara",
                        District = "Bukhara",
                        Latitude = 39.768299m,
                        Longitude = 64.455444m
                    },
                    new Location
                    {
                        City = "Khiva",
                        District = "Khorezm",
                        Latitude = 41.378568m,
                        Longitude = 60.363562m
                    },
                    new Location
                    {
                        City = "Andijan",
                        District = "Andijan",
                        Latitude = 40.782060m,
                        Longitude = 72.344245m
                    },
                    new Location
                    {
                        City = "Fergana",
                        District = "Fergana",
                        Latitude = 40.386400m,
                        Longitude = 71.784320m
                    },
                    new Location
                    {
                        City = "Namangan",
                        District = "Namangan",
                        Latitude = 40.998300m,
                        Longitude = 71.672570m
                    },
                    new Location
                    {
                        City = "Nukus",
                        District = "Karakalpakstan",
                        Latitude = 42.461110m,
                        Longitude = 59.619447m
                    }
                };

                context.Locations.AddRange(locationsNP);
                context.SaveChanges();
            }
        }

        private static void SeedCategories(ServiCarApiContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Repair"
                    },
                    new Category
                    {
                        Name = "Car Wash"
                    },
                    new Category
                    {
                        Name = "Maintenance"
                    },
                    new Category
                    {
                        Name = "Tire"
                    },
                    new Category
                    {
                        Name = "Fuel"
                    }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }

}
