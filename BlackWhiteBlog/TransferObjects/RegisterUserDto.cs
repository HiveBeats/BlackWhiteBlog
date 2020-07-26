namespace BlackWhiteBlog.TransferObjects
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserPermissions { get; set; }
        public string AuthorName { get; set; }
    }
}