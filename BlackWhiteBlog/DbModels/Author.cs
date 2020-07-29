using System.Collections.Generic;
using Org.BouncyCastle.Tsp;

namespace BlackWhiteBlog.DbModels
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPicLink { get; set; }
        public string AuthorDesc { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}