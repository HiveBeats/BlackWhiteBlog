namespace BlackWhiteBlog.TransferObjects.User
{
    public class UserDetailDto : UserCardDto
    {
        public int Privs { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPicLink { get; set; }
    }
}