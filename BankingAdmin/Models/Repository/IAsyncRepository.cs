using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BankingAdmin.Models.Repository
{
    public interface IAsyncRepository<TEntity, TKey>
    {
        public Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> GetAsync(TKey id);
        public Task<TKey> AddAsync(TEntity item);
        public Task<TKey> UpdateAsync(TKey id, TEntity item);
        public Task<TKey> DeleteAsync(TKey id);
    }
}
