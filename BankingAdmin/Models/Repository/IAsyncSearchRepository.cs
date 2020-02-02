using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingAdmin.Models.Repository
{
    public interface IAsyncSearchRepository<TEntity, TQuery>
    {
        Task<IEnumerable<TEntity>> GetManyByQueryAsync(TQuery query);
        Task<IEnumerable<TEntity>> GetManyAsync(Func<TEntity, bool>[] predicateList);

    }
}
