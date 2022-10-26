using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KioskManagement.Model.Models;
using System;

namespace KioskManagement.Data.Migrations
{
    public  class SeedData
    {
        private  AppUser _appUser= new AppUser();
        private  PasswordHasher<AppUser> _passwordHasher= new PasswordHasher<AppUser>();
        public  void SeedUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
           .HasData(_appUser = new AppUser()
           {
               Id = Guid.NewGuid().ToString(),
               FullName = "Admin",
               Email = "admin@astec.vn",
               CreatedDate = DateTime.Now,
               UserName = "admin",
               PasswordHash = _passwordHasher.HashPassword(_appUser, "123456")
           });
             

            modelBuilder.Entity<AppRole>()
                .HasData(new AppRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name ="CreateUser",
                    NormalizedName = "CreateUser",
                    CreatedDate = DateTime.Now,
                },
                new AppRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "ViewUser",
                    NormalizedName = "ViewUser",
                    CreatedDate = DateTime.Now,
                },
                 new AppRole()
                 {
                     Id = Guid.NewGuid().ToString(),
                     Name = "UpdateUser",
                     NormalizedName = "UpdateUser",
                     CreatedDate = DateTime.Now,
                 },
                  new AppRole()
                  {
                      Id = Guid.NewGuid().ToString(),
                      Name = "DeleteUser",
                      NormalizedName = "DeleteUser",
                      CreatedDate = DateTime.Now,
                  },
                   new AppRole()
                   {
                       Id = Guid.NewGuid().ToString(),
                       Name = "CreateRole",
                       NormalizedName = "CreateRole",
                       CreatedDate = DateTime.Now,
                   },
                    new AppRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "ViewRole",
                        NormalizedName = "ViewRole",
                        CreatedDate = DateTime.Now,
                    },
                    new AppRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "UpdateRole",
                        NormalizedName = "UpdateRole",
                        CreatedDate = DateTime.Now,
                    },
                    new AppRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "DeleteRole",
                        NormalizedName = "DeleteRole",
                        CreatedDate = DateTime.Now,
                    },
                     new AppRole()
                     {
                         Id = Guid.NewGuid().ToString(),
                         Name = "CreateGroup",
                         NormalizedName = "CreateGroup",
                         CreatedDate = DateTime.Now,
                     },
                      new AppRole()
                      {
                          Id = Guid.NewGuid().ToString(),
                          Name = "ViewGroup",
                          NormalizedName = "ViewGroup",
                          CreatedDate = DateTime.Now,
                      },
                       new AppRole()
                       {
                           Id = Guid.NewGuid().ToString(),
                           Name = "UpdateGroup",
                           NormalizedName = "UpdateGroup",
                           CreatedDate = DateTime.Now,
                       }
                );
        }

      
               
    }
}