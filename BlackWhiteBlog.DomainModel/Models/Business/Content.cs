namespace BlackWhiteBlog.DomainModel.Models.Business
{
    public class Content :IEntity<int>
    {
        #region ctor
        private Content()
        {
            
        }

        public Content(string title, string text, string image)
        {
            Title = title;
            Text = text;
            Image = text;
        }
        #endregion
        
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public string Image { get; private set; }

        public void UpdateContent(string title, string text, string image)
        {
            Title = title;
            Text = text;
            Image = image;
        }
    }
}