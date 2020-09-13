using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlackWhiteBlog.DomainModel.Models;
namespace BlackWhiteBlog.Repository
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>
    {
        Task<TEntity> GetById(TKey id);
        Task<IEnumerable<TEntity>> Find(ISpecification<TEntity> specification = null);
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entity);
        Task Delete(TEntity entity);
        Task Update(TEntity entity);
        Task<bool> Contains(ISpecification<TEntity> specification = null);
        Task<bool> Contains(Expression<Func<TEntity, bool>> predicate);
        Task<int> Count(ISpecification<TEntity> specification = null);
        Task<int> Count(Expression<Func<TEntity, bool>> predicate);
    }
}