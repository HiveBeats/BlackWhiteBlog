using System.Threading.Tasks;
using BlackWhiteBlog.TransferObjects.User;

namespace BlackWhiteBlog.Services
{
    public interface IUserService
    {
        Task<UserLoginDto> Login(LoginDto userDto);
        Task<UserLoginDto> Register(RegisterUserDto userDto);
    }
}