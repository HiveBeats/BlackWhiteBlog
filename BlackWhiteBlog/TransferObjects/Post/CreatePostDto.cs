using System.Collections;
using System.Collections.Generic;

namespace BlackWhiteBlog.TransferObjects.Post
{
    public class PostContentDto: UserRequestDto
    {
        public string Title { get; set; }
        public string PicLink { get; set; }
        public string HtmlContent { get; set; }
        public int Color { get; set; }
    }
    public class CreatePostDto : UserRequestDto
    {
        public IList<PostContentDto> Contents { get; set; }
    }
}