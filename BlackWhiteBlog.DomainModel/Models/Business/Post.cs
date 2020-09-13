using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackWhiteBlog.DomainModel.Models.Business
{
    public class Post : IEntity<int>
    {
        #region ctor
        
        private Post()
        {

        }

        public Post(Author author, Content whiteContent, Content blackContent)
        {
            DateCreated = DateTime.UtcNow;
            Author = author;
            WhiteContent = whiteContent;
            BlackContent = blackContent;
        }
        
        #endregion

        public int Id { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime? DateClosed { get; private set; }
        public Author Author { get; private set; }
        public Content WhiteContent { get; private set; }
        public Content BlackContent { get; private set; }
        
        //EF Properties
        public int AuthorId { get; set; }
        //public ICollection<PostContent> PostContents { get; set; }

        public void ClosePost(DateTime? closingDate = null)
        {
            if (closingDate == null)
                DateClosed = DateTime.UtcNow;
            else DateClosed = closingDate;
        }
        [NotMapped]
        public bool IsClosed => DateClosed != null && DateClosed <= DateTime.UtcNow;
    }
}