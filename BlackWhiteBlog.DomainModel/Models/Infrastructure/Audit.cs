namespace BlackWhiteBlog.DomainModel.Models.Infrastructure
{
    public class Audit : IEntity<int>
    {
        #region ctor
        private Audit()
        {
            
        }

        public Audit(string obj, int changerId)
        {
            Object = obj;
            ChangerId = changerId;
        }
        #endregion
        
        public int Id { get; private set; }
        public string Object { get; private set; }
        public int ChangerId { get; private set; }
    }
}