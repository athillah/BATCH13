using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Data
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions) { }

        public DbSet<Film> Films { get; set; }
        public DbSet<Review> Reviews { get; set; }

                protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed default roles
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();
            var managerRoleId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                }
            );

            // Seed default admin user
            var adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<User>();

            var adminUser = new User
            {
                Id = adminUserId,
                UserName = "admin@jwtauth.com",
                NormalizedUserName = "ADMIN@JWTAUTH.COM",
                Email = "admin@jwtauth.com",
                NormalizedEmail = "ADMIN@JWTAUTH.COM",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                CreatedAt = DateTime.UtcNow
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
            builder.Entity<User>().HasData(adminUser);

            // Assign admin role to admin user
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );
        }
    }
}