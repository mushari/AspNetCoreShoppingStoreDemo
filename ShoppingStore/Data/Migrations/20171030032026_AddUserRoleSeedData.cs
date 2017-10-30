using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ShoppingStore.Data.Migrations
{
    public partial class AddUserRoleSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {

                    var role = new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName="ADMIN" 
                    };

                    context.Roles.Add(role);
                    context.SaveChanges();


                    var user = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "ADMIN@EXAMPLE.COM",
                        NormalizedUserName = "ADMIN",
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "secret");
                    user.PasswordHash = hashed;

                    var userStore = new UserStore<ApplicationUser>(context);
                    userStore.CreateAsync(user);

                    context.SaveChanges();

                    IdentityUserRole<string> userRole = new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    };
                    context.UserRoles.Add(userRole);

                    context.SaveChanges();

                    transaction.Commit();
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var role = context.Roles.Where(r => r.Name == "Admin").FirstOrDefault();

                    var user = context.Users.Where(u => u.UserName == "Admin").FirstOrDefault();

                    if (role != null && user != null)
                    {
                        IdentityUserRole<string> userRole = new IdentityUserRole<string>
                        {
                            RoleId = role.Id,
                            UserId = user.Id
                        };

                        context.UserRoles.Remove(userRole);
                    }

                    context.SaveChanges();

                    if (role != null)
                    {
                        context.Roles.Remove(role);
                    }
                    context.SaveChanges();



                    if (user != null)
                    {
                        context.Users.Remove(user);
                    }

                    context.SaveChanges();


                    transaction.Commit();
                }
            }
        }
    }
}
