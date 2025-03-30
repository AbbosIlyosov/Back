using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.Entities;
using ServiCar.Domain.Enums;

namespace ServiCar.Infrastructure.Persistence
{
    public class ServiCarApiContext
        : IdentityDbContext<User, Role, int>
    {
        public ServiCarApiContext(DbContextOptions<ServiCarApiContext> options, ILookupNormalizer normalizer)
            : base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }
        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointment>(Appointment =>
            {
                Appointment.Property(a => a.AppointmentTime).IsRequired();
                Appointment.Property(a => a.AppointmentStatusId).IsRequired();
                Appointment.Property(a => a.OrderNumber).IsRequired();

                Appointment.HasOne(a => a.User).WithMany(u => u.Appointments).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict); ;
                Appointment.HasOne(a => a.Point).WithMany(p => p.Appointments).HasForeignKey(a => a.PointId).OnDelete(DeleteBehavior.Restrict); ;
                Appointment.HasMany(a => a.Reviews).WithOne(r => r.Appointment).HasForeignKey(r => r.AppointmentId).OnDelete(DeleteBehavior.Restrict); ;
            });

            builder.Entity<Point>(Point =>
            {
                Point.Property(p => p.PointName).IsRequired();
                Point.Property(p => p.PointStatusId).IsRequired();

                Point.HasOne(p => p.Category).WithMany(c => c.Points).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
                Point.HasOne(p => p.Location).WithMany(l => l.Points).HasForeignKey(p => p.LocationId).OnDelete(DeleteBehavior.Restrict);
                Point.HasOne(p => p.Business).WithMany(b => b.Points).HasForeignKey(p => p.BusinessId).OnDelete(DeleteBehavior.Restrict);
                Point.HasOne(p => p.WorkingTime).WithMany(w => w.Points).HasForeignKey(p => p.WorkingTimeId).OnDelete(DeleteBehavior.Restrict);
                Point.HasOne(p => p.User).WithMany(u => u.Points).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);

                Point.HasMany(p => p.Appointments).WithOne(a => a.Point).HasForeignKey(a => a.PointId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Business>(Business =>
            {
                Business.Property(b => b.Name).IsRequired();

                Business.HasOne(b => b.Image).WithOne(i => i.Business).HasForeignKey<Business>(b => b.ImageId).OnDelete(DeleteBehavior.Restrict); ;
                Business.HasMany(b => b.Categories).WithOne(p => p.Business).HasForeignKey(p => p.BusinessId).OnDelete(DeleteBehavior.Restrict); ;
                Business.HasMany(b => b.Workers).WithOne(u => u.Business).HasForeignKey(b => b.BusinessId).OnDelete(DeleteBehavior.Restrict); ;
            });

            builder.Entity<Review>(Review =>
            {
                Review.Property(r => r.Rating).IsRequired();

                Review.HasMany(r => r.Images).WithOne(i => i.Review).HasForeignKey(i => i.ReviewId).OnDelete(DeleteBehavior.Restrict); ;

                Review.HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict); ;
                Review.HasOne(r => r.Point).WithMany(p => p.Reviews).HasForeignKey(r => r.PointId).OnDelete(DeleteBehavior.Restrict); ;
                Review.HasOne(r => r.Appointment).WithMany(a => a.Reviews).HasForeignKey(r => r.AppointmentId).OnDelete(DeleteBehavior.Restrict); ;
            });

            builder.Entity<Location>(Location =>
            {
                Location.Property(l => l.Longitude).HasPrecision(9, 6);
                Location.Property(l => l.Latitude).HasPrecision(9, 6);
            });

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
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if(entry.Entity is Point point){
                    if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
                    {
                        var businessId = point.BusinessId;
                        var business = Businesses.FirstOrDefault(b => b.Id == businessId);
                        if (business != null)
                        {
                            business.PointsCount = Points.Count(p => p.BusinessId == businessId);
                        }
                    }
                }

                if(entry.State == EntityState.Modified && entry.Entity is BaseModel baseModel)
                {
                    baseModel.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}
