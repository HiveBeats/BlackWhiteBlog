using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BlackWhiteBlog.Controllers.Users;
using System.Threading.Tasks;
using BlackWhiteBlog.DbModels;
using BlackWhiteBlog.Services;
using BlackWhiteBlog.TransferObjects;
using BlackWhiteBlog.TransferObjects.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        protected virtual void Seed()
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


        private User CreateTestUser(BlogDbContext ctx, int privs, int id)
        {
            var user = new User() {UserId = id, UserName = "John Gaven", Privs = privs, HashedPassword = "ndt5bm#gY"};
            var author = new Author() {AuthorId = id, AuthorName = "Ваня Лалет", AuthorPicLink = "./pic", AuthorDesc = "Some Guy", UserId = id };
            user.Author = author;
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user;
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

        [Fact]
        public async Task Get_Items_BadRequest_IfNotValid()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));

                var actionResult = await controller.Get(null);
                Assert.True(actionResult is BadRequestObjectResult);

                actionResult = await controller.Get(new UserRequestDto(){UserId = 0, AuthorId = 1});
                Assert.True(actionResult is BadRequestObjectResult);

                actionResult = await controller.Get(new UserRequestDto(){UserId = null, AuthorId = 1});
                Assert.True(actionResult is BadRequestObjectResult);
            }
        }

        [Fact]
        public async Task Get_Items_BadRequest_WithoutPrivs()
        {
            using (var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));

                var user = CreateTestUser(ctx, 1, 2);
                var actionResult = await controller.Get(new UserRequestDto(){UserId = user.UserId, AuthorId = user.AuthorId});
                Assert.True(actionResult is BadRequestObjectResult);

                ctx.Users.Remove(user);
                ctx.SaveChanges();
            }
        }

        [Fact]
        public async Task Can_Get_Item()
        {
            using (var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));

                var actionResult = await controller.Get(1, new UserRequestDto(){UserId = 1, AuthorId = 1});
                Assert.True(actionResult is OkObjectResult);

                var okResult = actionResult as OkObjectResult;
                var item = okResult.Value as UserDetailDto;

                Assert.Equal(1, item.UserId);
                Assert.Equal("Ваня Лалетин", item.AuthorName);
                Assert.Equal(4, item.Privs);
                Assert.Equal("John Grave", item.UserName);
            }
        }

        [Fact]
        public async Task Get_Item_BadRequest_IfNotValid()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));

                var actionResult = await controller.Get(1, null);
                Assert.True(actionResult is BadRequestObjectResult);

                actionResult = await controller.Get(1, new UserRequestDto(){UserId = 0, AuthorId = 1});
                Assert.True(actionResult is BadRequestObjectResult);

                actionResult = await controller.Get(1, new UserRequestDto(){UserId = null, AuthorId = 1});
                Assert.True(actionResult is BadRequestObjectResult);
            }
        }

        [Fact]
        public async Task Get_Item_BadRequest_WithoutPrivs()
        {   
            using (var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));

                var user = CreateTestUser(ctx, 1, 2);
                var actionResult = await controller.Get(1, new UserRequestDto(){UserId = user.UserId, AuthorId = user.AuthorId});
                Assert.True(actionResult is BadRequestObjectResult);

                ctx.Users.Remove(user);
                ctx.SaveChanges();
            }
        }

        [Fact]
        public async Task Get_Item_NotFound_IfNotExists()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));

                var actionResult = await controller.Get(4, new UserRequestDto(){UserId = 1, AuthorId = 1});
                Assert.True(actionResult is NotFoundResult);
            }
        }

        public async Task Can_Register_User()
        {

        }
    
    }
}