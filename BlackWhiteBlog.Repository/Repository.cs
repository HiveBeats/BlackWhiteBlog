using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlackWhiteBlog.DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace BlackWhiteBlog.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        protected readonly DbContext _ctx;

        public Repository(DbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _ctx.Set<TEntity>().ToListAsync();
        }

        public async Task Add(TEntity entity)
        {
            _ctx.Set<TEntity>().Add(entity);
            await SaveChanges(entity);
        }

        public async Task Delete(TEntity entity)
        {
            _ctx.Set<TEntity>().Remove(entity);
            await SaveChanges(entity);
        }

        public async Task SaveChanges(TEntity entity)
        {
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> Contains(ISpecification<TEntity> specification = null)
        {
            return await Count(specification) > 0 ? true : false;
        }

        public async Task<bool> Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return await Count(predicate) > 0 ? true : false;
        }

        public async Task<int> Count(ISpecification<TEntity> specification = null)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
        {
            return await _ctx.Set<TEntity>().Where(predicate).CountAsync();
        }

        public async Task<TEntity> GetById(TKey id)
        {
            return await _ctx.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> Find(ISpecification<TEntity> specification = null)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
            _ctx.Set<TEntity>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
            await SaveChanges(entity);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_ctx.Set<TEntity>().AsQueryable(), spec);
        }
    }
}