using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DomainServices
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? page = null,
            int? pageSize = null);

        T Create();
        T GetByKey(params object[] key);
        T Insert(T entity);
        void DeleteByKey(params object[] key);
        void Update(T entity);
        int Count(Expression<Func<T, bool>> filter = null);
    }
}
