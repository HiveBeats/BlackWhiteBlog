namespace BlackWhiteBlog.TransferObjects.User
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Token { get; set; }
    }

    public class UserLoginDto
    {
        public UserLoginDto(LoginDto dto)
        {
            LoginDto = dto;
        }

        public UserLoginDto(RegisterUserDto dto)
        {
            var loginDto = new LoginDto()
            {
                UserName = dto.UserName,
                UserPassword = dto.Password
            };
        }

        public LoginDto LoginDto { get; private set; }
        
        public int UserId { get; set; }

        public  UserLoginDto WithoutPassword()
        {
            this.LoginDto.UserPassword = null;
            return this;
        }
    }
}