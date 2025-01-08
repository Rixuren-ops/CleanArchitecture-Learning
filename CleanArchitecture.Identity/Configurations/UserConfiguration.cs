using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@localhost",
                    NormalizedEmail = "ADMIN@LOCALHOST",
                    Name = "Omar",
                    LastName = "Hernandez",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Vaxidrez2025$"),
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "user",
                    NormalizedUserName = "USER",
                    Email = "user@localhost",
                    NormalizedEmail = "USER@LOCALHOST",
                    Name = "John",
                    LastName = "Doe",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Vaxidrez2025$"),
                });
        }
    }
}
