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

        [Fact]
        public async Task Can_Register_User()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                var registerUserDto = new RegisterUserDto()
                {
                    UserName = "John Gandon",
                    Password = "ndt5bm#gY",
                    UserPermissions = 4,
                    AuthorName = "Ванька Встанька"
                };

                var actionResult = await controller.Post(registerUserDto);
                Assert.True(actionResult is OkObjectResult);
                
                var okResult = actionResult as OkObjectResult;
                var registeredUserDto = okResult.Value as UserLoginDto;

                Assert.NotNull(registeredUserDto);
                Assert.NotNull(registeredUserDto.LoginDto.Token);
                Assert.Null(registeredUserDto.LoginDto.UserPassword);
                var dbUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == registeredUserDto.UserId);
                Assert.NotNull(dbUser);
                Assert.True(LoginService.CheckPassword(dbUser.HashedPassword, registerUserDto.Password));
                Assert.Equal(dbUser.Privs, registerUserDto.UserPermissions);
                ctx.Users.Remove(dbUser);
                await ctx.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task Register_BadRequest_IfCantRegisterUser()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                var existingUserDto = new RegisterUserDto()
                {
                    UserName = "John Grave",
                    Password = "ndt5bm#gY",
                    UserPermissions = 4,
                    AuthorName = "Ванька Встанька"
                };

                var userExistsResult = await controller.Post(existingUserDto);
                var invalidResult = await controller.Post(null);
                var invalidInputResult = await controller.Post(new RegisterUserDto() {UserName = null, Password = "null"});
                var invalidInputResult2 = await controller.Post(new RegisterUserDto() {UserName = "null", Password = null});
                                               
                Assert.True(userExistsResult is BadRequestObjectResult);
                Assert.True(invalidResult is BadRequestObjectResult);
                Assert.True(invalidInputResult is BadRequestObjectResult);
                Assert.True(invalidInputResult2 is BadRequestObjectResult);
            }
        }

        [Fact]
        public async Task Can_Login_User()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                var user = new LoginDto {UserName = "John Grave", UserPassword = "ndt5bm#gY"};
                
                var actionResult = await controller.Login(1, user);
                Assert.True(actionResult is OkObjectResult);
                
                var objectResult = actionResult as OkObjectResult;
                var loggedUser = objectResult.Value as UserLoginDto;
                Assert.NotNull(loggedUser);    
                
                var dbUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == loggedUser.UserId);
            
                Assert.NotNull(loggedUser);
                Assert.NotNull(loggedUser.LoginDto.Token);
                Assert.Equal(dbUser.Token, loggedUser.LoginDto.Token);
                Assert.Null(loggedUser.LoginDto.UserPassword);
                Assert.Equal(user.UserName, loggedUser.LoginDto.UserName);
            }
        }

        [Fact]
        public async Task Login_BadRequest_IfNotValid()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                
                var actionResult = await controller.Login(1, null);
                Assert.True(actionResult is BadRequestObjectResult);
                
                actionResult = await controller.Login(1, new LoginDto() {UserName = "John Grave", UserPassword = null});
                Assert.True(actionResult is BadRequestObjectResult);

                actionResult = await controller.Login(1, new LoginDto(){UserName = null, UserPassword = "ndt5bm#gY"});
                Assert.True(actionResult is BadRequestObjectResult);                
            }
        }

        [Fact]
        public async Task Login_BadRequest_IfCant()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                
                //bad login
                var actionResult = await controller.Login(1, new LoginDto() {UserName = "John Gravez", UserPassword = "ndt5bm#gY"});
                Assert.True(actionResult is BadRequestObjectResult);

                //bad password
                actionResult = await controller.Login(1, new LoginDto() {UserName = "John Grave", UserPassword = "ndt5bm#gF"});
                Assert.True(actionResult is BadRequestObjectResult);
            }
        }

        private async Task<IActionResult> EditUser(UserController controller, int editorId, int itemId, string newName, int newPriv)
        {
            var editedUserDto = new EditUserDto()
            {
                UserId  = editorId,
                EditedUserName= newName,
                EditedUserPrivs = newPriv
            };

            var result = await controller.Put(itemId, editedUserDto);
            return result;
        }

        [Fact]
        public async Task Can_Edit_User()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                var itemId = 1;
                var newName = "JohnGrave2";
                var newPriv = 2;
                var actionResult = await EditUser(controller, 1, itemId, newName, newPriv);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is OkResult);
                
                var editedUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserName == newName);
                Assert.NotNull(editedUser);
                Assert.Equal(itemId, editedUser.UserId);
                Assert.Equal(newPriv, editedUser.Privs);
                
                //вернем изменения
                editedUser.UserName = "John Grave";
                editedUser.Privs = 4;
                await ctx.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task Can_Login_AfterEdit()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                var itemId = 1;
                var newName = "JohnGrave2";
                var actionResult = await EditUser(controller, 1, itemId, newName, 2);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is OkResult);

                var loginResult = await controller.Login(1, new LoginDto() {UserName = newName, UserPassword = "ndt5bm#gY"});
                Assert.NotNull(loginResult);
                Assert.True(loginResult is OkObjectResult);
                var loggedUser = (loginResult as OkObjectResult).Value as UserLoginDto;
                Assert.NotNull(loggedUser);    
                
                //вернем изменения
                var editedUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserName == newName);
                editedUser.UserName = "John Grave";
                editedUser.Privs = 4;
                await ctx.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task UserEdit_BadRequest_IfNotFound()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                var itemId = 25;
                var newName = "JohnGrave2";
                var actionResult = await EditUser(controller, 1, itemId, newName, 2);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is BadRequestObjectResult);
            }
        }

        [Fact]
        public async Task UserEdit_BadRequest_DeniedByPrivs()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                //уменьшаем себе права
                var editorUser = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == 1);
                editorUser.Privs = 1;
                await ctx.SaveChangesAsync();

                var itemId = 1;
                var newName = "JohnGrave2";
                var actionResult = await EditUser(controller, 1, itemId, newName, 2);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is BadRequestObjectResult);

                //увеличиваем себе права до необходимых
                editorUser.Privs = 3;
                await ctx.SaveChangesAsync();

                actionResult = await EditUser(controller, 1, itemId, newName, 2);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is OkResult);
            }
        }

        [Fact]
        public async Task UserEdit_BadRequest_IfNotValid()
        {
            using(var ctx = new BlogDbContext(ContextOptions))
            {
                var controller = new UserController(ctx, new UserService(ctx));
                var newName = "JohnGrave2";
                var newPriv = 2;
                var actionResult = await EditUser(controller, 1, -1, newName, newPriv);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is BadRequestObjectResult);


                var invalidEditedUserDto = new EditUserDto()
                {
                    UserId  = 1,
                    EditedUserName = null
                };
                actionResult = await controller.Put(1, invalidEditedUserDto);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is BadRequestObjectResult);

                invalidEditedUserDto.UserId = null;
                invalidEditedUserDto.EditedUserName = "John Grave";
                invalidEditedUserDto.EditedUserPrivs = 4;
                actionResult = await controller.Put(1, invalidEditedUserDto);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is BadRequestObjectResult);

                actionResult = await controller.Put(1, null);
                Assert.NotNull(actionResult);
                Assert.True(actionResult is BadRequestObjectResult);
            }
        }    
    }
}