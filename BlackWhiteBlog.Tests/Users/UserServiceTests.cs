using BlackWhiteBlog.Tests.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Threading.Tasks;
using BlackWhiteBlog.DbModels;
using BlackWhiteBlog.Services;
using BlackWhiteBlog.TransferObjects;
using BlackWhiteBlog.TransferObjects.User;
using Microsoft.EntityFrameworkCore;

public class UserServiceTests : UsersTest
{
    public UserServiceTests() : base(
            new DbContextOptionsBuilder<BlogDbContext>()
                .UseSqlite("Filename=TestUserService.db")
                .Options)
    {
        SetMainUserHashedPassword();
    }

    public void SetMainUserHashedPassword()
    {
        using(var ctx = new BlogDbContext(ContextOptions))
        {
            var user = ctx.Users.FirstOrDefault(u => u.UserId == 1);

            var hashedPassword = LoginService.HashPassword("ndt5bm#gY");
            user.HashedPassword = hashedPassword;
            
            ctx.SaveChanges();
        }
    }

    [Fact]
    public async Task Can_Login()
    {
        using(var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var user = new LoginDto {UserName = "John Grave", UserPassword = "ndt5bm#gY"};

            var loggedUser = await service.Login(user);
            var dbUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == 1);
            
            Assert.NotNull(loggedUser);
            Assert.NotNull(loggedUser.LoginDto.Token);
            Assert.Equal(dbUser.Token, loggedUser.LoginDto.Token);
            Assert.Null(loggedUser.LoginDto.UserPassword);
            Assert.Equal(user.UserName, loggedUser.LoginDto.UserName);
        }
    }

    [Fact]
    public async Task Not_Login_IfNotValid()
    {
         using(var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var user = new LoginDto {UserName = null, UserPassword = "ndt5bm#fY"};

            var loggedUser = await service.Login(user);
            Assert.Null(loggedUser);

            user = new LoginDto {UserName = "John Grave", UserPassword = null};
            loggedUser = await service.Login(user);
            Assert.Null(loggedUser);

            loggedUser = await service.Login(null);
            Assert.Null(loggedUser);
        }
    }

    [Fact]
    public async Task Not_Login_IfPasswordNotCorrect()
    {
        using(var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var user = new LoginDto {UserName = "John Grave", UserPassword = "ndt5bm#fY"};

            var loggedUser = await service.Login(user);
           
            Assert.Null(loggedUser);
        }
    }

    [Fact]
    public async Task Not_Login_IfNotExists()
    {
        using(var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var user = new LoginDto {UserName = "John Gra", UserPassword = "ndt5bm#gY"};

            var loggedUser = await service.Login(user);
           
            Assert.Null(loggedUser);
        }
    }

}