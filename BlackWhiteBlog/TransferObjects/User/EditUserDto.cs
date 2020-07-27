namespace BlackWhiteBlog.TransferObjects.User
{
    public class EditUserDto : UserRequestDto
    {
        public string EditedUserName { get; set; }
        public int EditedUserPrivs { get; set; }
    }
}