using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShoppingStore.Data.Migrations
{
    public partial class SeedData : Migration
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
                        NormalizedName = "ADMIN"
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

                    var photo = new Photo
                    {
                        FileName = "product_image",
                        FileAddress = "/photos/product_image.png"
                    };

                    context.Photos.Add(photo);
                    context.SaveChanges();

                    for (int i = 0; i < 10; i++)
                    {
                        var category_gb = new Category
                        {
                            CategoryId = "category" + i + "_en-GB",
                            CategoryName = "Category" + i
                        };

                        var category_us = new Category
                        {
                            CategoryId = "category" + i + "_en-US",
                            CategoryName = "Category" + i
                        };

                        var category_zh = new Category
                        {
                            CategoryId = "category" + i + "_zh-TW",
                            CategoryName = "類型" + i
                        };

                        context.Categories.Add(category_gb);
                        context.Categories.Add(category_zh);
                        context.Categories.Add(category_us);

                        context.SaveChanges();

                        var product_gb = new Product
                        {
                            ProductId = "productDemo" + i + "_en-GB",
                            Name = "Product" + i,
                            Description = "This is product demo description",
                            Price = 2M * i,
                            CategoryId = category_gb.CategoryId,
                            PhotoId = photo.PhotoId,
                        };

                        var product_us = new Product
                        {
                            ProductId = "productDemo" + i + "_en-US",
                            Name = "Product" + i,
                            Description = "This is product demo description",
                            Price = 3M * i,
                            CategoryId = category_us.CategoryId,
                            PhotoId = photo.PhotoId,
                        };

                        var product_zh = new Product
                        {
                            ProductId = "productDemo" + i + "_zh-TW",
                            Name = "產品" + i,
                            Description = "產品描述Demo" + i,
                            Price = 100M * i,
                            CategoryId = category_zh.CategoryId,
                            PhotoId = photo.PhotoId,
                        };

                        context.Products.Add(product_gb);
                        context.Products.Add(product_us);
                        context.Products.Add(product_zh);
                        context.SaveChanges();
                    }

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
                        context.SaveChanges();
                    }


                    if (role != null)
                    {
                        context.Roles.Remove(role);
                        context.SaveChanges();
                    }



                    if (user != null)
                    {
                        context.Users.Remove(user);
                        context.SaveChanges();
                    }

                    var photo = context.Photos.FirstOrDefault(p => p.FileName == "product_image");

                    if (photo != null)
                    {
                        context.Photos.Remove(photo);
                        context.SaveChanges();



                        for (int i = 0; i < 10; i++)
                        {
                            var category_gb = context.Categories.FirstOrDefault(c => c.CategoryId == "category" + i + "_en-GB");
                            var category_us = context.Categories.FirstOrDefault(c => c.CategoryId == "category" + i + "_en-US");
                            var category_zh = context.Categories.FirstOrDefault(c => c.CategoryId == "category" + i + "_zh-TW");


                            if (category_gb != null && category_us != null && category_zh != null)
                            {
                                context.Categories.Remove(category_gb);
                                context.Categories.Remove(category_zh);
                                context.Categories.Remove(category_us);
                                context.SaveChanges();
                            }

                            var product_gb = context.Products.FirstOrDefault(p => p.ProductId == "productDemo" + i + "_en-GB");
                            var product_us = context.Products.FirstOrDefault(p => p.ProductId == "productDemo" + i + "_en-US");
                            var product_zh = context.Products.FirstOrDefault(p => p.ProductId == "productDemo" + i + "_zh-TW");

                            if (product_gb != null && product_us != null && product_zh != null)
                            {
                                context.Products.Remove(product_gb);
                                context.Products.Remove(product_us);
                                context.Products.Remove(product_zh);
                                context.SaveChanges();
                            }
                        }
                    }


                    transaction.Commit();
                }
            }
        }
    }
}
