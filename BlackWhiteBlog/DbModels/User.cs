namespace BlackWhiteBlog.DbModels
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Token { get; set; }
        public int Privs { get; set; }
        
        public int? AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }
}