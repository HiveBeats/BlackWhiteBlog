using System;
using System.Collections.Generic;

namespace BlackWhiteBlog.DbModels
{
    public class Post
    {
        public int PostId { get; set; }
        public DateTime PostDate { get; set; }
        public string PostTitle { get; set; }
        
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<PostContent> PostContents { get; set; }
    }
}