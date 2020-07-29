namespace BlackWhiteBlog.DbModels
{
    public class PostContent
    {
        public int PostContentId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        
        public string Title { get; set; }
        public string ImageLink { get; set; }
        public string Content { get; set; }
        public int PostColor { get; set; }
    }
}