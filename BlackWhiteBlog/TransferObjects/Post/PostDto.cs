using System;

namespace BlackWhiteBlog.TransferObjects.Post
{
    public class FullPostDto
    {
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string PicLink { get; set; }
        public DateTime PostDate { get; set; }
        public string HtmlContent { get; set; }
    }
}