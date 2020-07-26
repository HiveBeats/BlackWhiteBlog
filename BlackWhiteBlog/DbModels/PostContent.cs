namespace BlackWhiteBlog.DbModels
{
    public class PostContent
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        
        public string Content { get; set; }
        public int PostColor { get; set; }
    }
}