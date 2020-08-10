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
using BlackWhiteBlog.Helpers.Permissions;

public class UserServiceTests : UsersTest
{
    public UserServiceTests() : base(
            new DbContextOptionsBuilder<BlogDbContext>()
                .UseSqlite("Filename=TestUserService.db")
                .Options)
    {
        SetMainUserHashedPassword();
    }

    private void SetMainUserHashedPassword()
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

    [Fact]
    public async Task Can_Register()
    {
        using (var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var registerUserDto = new RegisterUserDto()
            {
                UserName = "John Gandon",
                Password = "ndt5bm#gY",
                UserPermissions = 4,
                AuthorName = "Ванька Встанька"
            };

            var loggedInRegisteredUser = await service.Register(registerUserDto);
            
            Assert.NotNull(loggedInRegisteredUser);
            Assert.NotNull(loggedInRegisteredUser.LoginDto.Token);
            Assert.Null(loggedInRegisteredUser.LoginDto.UserPassword);
            Assert.Equal(registerUserDto.UserName, loggedInRegisteredUser.LoginDto.UserName);
            
            var dbUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == loggedInRegisteredUser.UserId);
            Assert.NotNull(dbUser);
            Assert.NotNull(dbUser.AuthorId);
            Assert.Equal(registerUserDto.UserPermissions, dbUser.Privs);

            ctx.Users.Remove(dbUser);
            await ctx.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task Can_Register_With_DefaultPermissions()
    {
         using (var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var registerUserDto = new RegisterUserDto()
            {
                UserName = "John Gandon",
                Password = "ndt5bm#gY",
                AuthorName = "Ванька Встанька"
            };

            var loggedInRegisteredUser = await service.Register(registerUserDto);          
            Assert.NotNull(loggedInRegisteredUser);
            
            var dbUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == loggedInRegisteredUser.UserId);
            Assert.NotNull(dbUser);

            Assert.Equal((int)UserPermission.AddPost, dbUser.Privs);

            ctx.Users.Remove(dbUser);
            await ctx.SaveChangesAsync();
        }
    }

    
    [Fact]
    public async Task Can_Registered_Login()
    {
        using (var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var registerUserDto = new RegisterUserDto()
            {
                UserName = "John Gandon",
                Password = "ndt5bm#gY",
                UserPermissions = 4,
                AuthorName = "Ванька Встанька"
            };
            
            var loggedInRegisteredUser = await service.Register(registerUserDto);
            var loginDto = new LoginDto(){UserName = registerUserDto.UserName, UserPassword = registerUserDto.Password};
            var loggedUser = await service.Login(loginDto);
            
            Assert.NotNull(loggedUser);
            Assert.NotNull(loggedUser.LoginDto.Token);
            
            var dbUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == loggedUser.UserId);
            ctx.Users.Remove(dbUser);
            await ctx.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task Not_Register_IfNotValid()
    {
        using (var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var registerUserDto = new RegisterUserDto()
            {
                UserName = null,
                Password = "ndt5bm#gY",
                UserPermissions = 4,
                AuthorName = "Ванька Встанька"
            };

            var notRegisteredUser = await service.Register(registerUserDto);
            var otherNotRegisteredUser = await service.Register(null);
            
            Assert.Null(notRegisteredUser);
            Assert.Null(otherNotRegisteredUser);
        }
    }
    
    [Fact]
    public async Task Not_Register_IfExists()
    {
        using (var ctx = new BlogDbContext(ContextOptions))
        {
            var service = new UserService(ctx);
            var existingUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == 1);
            var registerUserDto = new RegisterUserDto()
            {
                UserName = existingUser.UserName,
                Password = "ndt5bm#gY",
                UserPermissions = 4,
                AuthorName = "Ваня Лалетин"
            };

            var notRegisteredUser = await service.Register(registerUserDto);

            Assert.Null(notRegisteredUser);
        }
    }
}