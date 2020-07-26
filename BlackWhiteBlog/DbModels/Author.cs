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
        private ICollection<Post> Posts { get; set; }
    }
}