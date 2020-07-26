using System.Collections;
using System.Collections.Generic;

namespace BlackWhiteBlog.TransferObjects.Post
{
    public class PostContentDto
    {
        public string Title { get; set; }
        public string PicLink { get; set; }
        public string HtmlContent { get; set; }
        public int Color { get; set; }
    }
    public class CreatePostDto
    {
        public int AuthorId { get; set; }
        public IList<PostContentDto> Contents { get; set; }
    }
}