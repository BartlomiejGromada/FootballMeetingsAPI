﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Seeds;

public static class UserSeed
{
    public static void SeedData(FootballMeetingsDbContext dbContext)
    {
        if(!dbContext.Users.Any())
        {
            var userAdmin = new User()
            {
                Id = 1,
                Email = "bartlomiejgromada97@gmail.com",
                Password = "AHSWxNXA88y9+fH8ZglzrAAP14D5SQxkxk+S+kiBu/HsObqfbklFGqNWzx9lA1UMQQ==",
                FirstName = "Bartłomiej",
                LastName = "Gromada",
                DateOfBirth = new DateTime(2022, 1, 18),
                NickName = "Bartula",
                CreatedAt = DateTime.Now,
                IsActive = true,
                RoleId = 1,
            };

            var userCreator = new User()
            {
                Id = 2,
                Email = "jankowalski@gmail.com",
                Password = "AHSWxNXA88y9+fH8ZglzrAAP14D5SQxkxk+S+kiBu/HsObqfbklFGqNWzx9lA1UMQQ==",
                FirstName = "Jan",
                LastName = "Kowalsk",
                DateOfBirth = null,
                NickName = "Kowal",
                CreatedAt = DateTime.Now,
                IsActive = true,
                RoleId = 2,
            };

            var userUser = new User()
            {
                Id = 3,
                Email = "adamnowak@gmail.com",
                Password = "AHSWxNXA88y9+fH8ZglzrAAP14D5SQxkxk+S+kiBu/HsObqfbklFGqNWzx9lA1UMQQ==",
                FirstName = null,
                LastName = null,
                DateOfBirth = new DateTime(1998, 5, 10),
                NickName = "Nowak",
                CreatedAt = DateTime.Now,
                IsActive = true,
                RoleId = 3,
            };


            using var transaction = dbContext.Database.BeginTransaction();

            dbContext.Users.AddRange(userAdmin, userCreator, userUser);
            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Users ON;");
            dbContext.SaveChanges();

            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Users OFF;");
            transaction.Commit();
        }
    }
}
