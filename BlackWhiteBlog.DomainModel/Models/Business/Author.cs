using System.Collections.Generic;

namespace BlackWhiteBlog.DomainModel.Models.Business
{
    public class Author : IEntity<int>
    {
        #region ctor
        
        private Author()
        {
            
        }

        public Author(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }
        
        #endregion
        
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public ICollection<Post> Posts { get; private set; }

        public void CreatePost(Post post)
        {
            Posts.Add(post);
        }

        public void UpdatePublicData(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }
    }
}