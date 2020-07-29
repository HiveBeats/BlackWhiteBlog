using System.Linq;
using System.Threading.Tasks;
using BlackWhiteBlog.DbModels;
using BlackWhiteBlog.Helpers.Permissions;
using BlackWhiteBlog.TransferObjects;
using BlackWhiteBlog.TransferObjects.User;
using Microsoft.EntityFrameworkCore;

namespace BlackWhiteBlog.Services
{
    public class UserService : IUserService
    {
        private readonly BlogDbContext _ctx;
        public UserService(BlogDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task<UserLoginDto> Login(LoginDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.UserName))
                return null;
            if (string.IsNullOrWhiteSpace(userDto.UserPassword))
                return null;
			
            //get_user
            var user = await _ctx.Users.Where(x => x.UserName == userDto.UserName).FirstOrDefaultAsync();
            if (user == null)
                return null;

            //compare_hash
            if (LoginService.CheckPassword(user.HashedPassword, userDto.UserPassword))
            {
                var tokenizedUserDto = await DoLogin(new UserLoginDto(userDto),  user);
                return tokenizedUserDto.WithoutPassword();
            }
            return null;
        }
        
        private async Task<UserLoginDto> DoLogin(UserLoginDto userDto, User user)
        {
            userDto.UserId = user.UserId;
            userDto.LoginDto.Token = LoginService.GetToken(user.UserName);
            user.Token = userDto.LoginDto.Token;
            await _ctx.SaveChangesAsync();
            return userDto;
        }
        
        public async Task<UserLoginDto> Register(RegisterUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.UserName))
                return null;
            if (string.IsNullOrWhiteSpace(userDto.Password))
                return null;
            var userExists = await _ctx.Users.AnyAsync(x => x.UserName == userDto.UserName);
            if (userExists)
                return null;

            //set_hashing_password
            var hashedPass = LoginService.HashPassword(userDto.Password);

            var author = new Author()
            {
                AuthorName = userDto.AuthorName
            };
            await _ctx.Authors.AddAsync(author);
            //create user
            var user = new User()
            {
                UserName = userDto.UserName,
                HashedPassword = hashedPass,
                //если права не были указаны изначально,указываем минимальные 
                Privs = userDto.UserPermissions ?? (int)UserPermission.AddPost,
                Author = author
            };
            
            await _ctx.Users.AddAsync(user);
            //create author
           
            //save changes in db
            await _ctx.SaveChangesAsync();

            //login
            var tokenizedUserDto = await DoLogin(new UserLoginDto(userDto), user);
            return tokenizedUserDto.WithoutPassword();
        }
    }
}