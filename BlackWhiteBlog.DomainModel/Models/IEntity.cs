namespace BlackWhiteBlog.DomainModel.Models
{
    public interface IEntity<TIdentity>
    {
        TIdentity Id { get; }
    }
}