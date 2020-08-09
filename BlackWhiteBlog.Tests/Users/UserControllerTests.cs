using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BlackWhiteBlog.Controllers;
using BlackWhiteBlog.Controllers.Users;
using System.Threading.Tasks;
using BlackWhiteBlog.DbModels;
using BlackWhiteBlog.Helpers.Permissions;
using BlackWhiteBlog.Services;
using BlackWhiteBlog.TransferObjects;
using BlackWhiteBlog.TransferObjects.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace BlackWhiteBlog.Tests.Users
{
    public class UsersTest
    {
        protected UsersTest(DbContextOptions<BlogDbContext> contextOptions)
        {
            ContextOptions = contextOptions;
            
            Seed();
        }

        protected DbContextOptions<BlogDbContext> ContextOptions { get; }

        private void Seed()
        {
            using (var context = new BlogDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                
                var user = new User() {UserId = 1, UserName = "John Grave", Privs = 4, HashedPassword = "ndt5bm#gY"};
                var author = new Author() {AuthorId = 1, AuthorName = "Ваня Лалетин", AuthorPicLink = "./pic", AuthorDesc = "Some Guy", UserId = 1 };
                user.Author = author;
                context.Users.Add(user);

                context.SaveChanges();
            }
        }
    }

    public class UserControllerTests : UsersTest
    {
        public UserControllerTests() : base(
            new DbContextOptionsBuilder<BlogDbContext>()
                .UseSqlite("Filename=TestUsers.db")
                .Options)
        {
            
        }

        [Fact]
        public async Task Can_Get_Items()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));

                var actionResult = await controller.Get(new UserRequestDto(){UserId = 1, AuthorId = 1});
                Assert.True(actionResult is OkObjectResult);

                var okResult = actionResult as OkObjectResult;
                var items = okResult.Value as IEnumerable<UserCardDto>;
                
                Assert.Equal(1, items.Count());
                Assert.Equal("John Grave", items.First().UserName);
            }
        }
    
    }
}