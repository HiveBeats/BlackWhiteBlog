namespace BlackWhiteBlog.DomainModel.Models
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}