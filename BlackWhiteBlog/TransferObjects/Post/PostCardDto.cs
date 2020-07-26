using System;

namespace BlackWhiteBlog.TransferObjects.Post
{
    public class PostCardDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string PicLink { get; set; }
        public DateTime PostDate { get; set; }
        public string TextContent { get; set; }
    }
}