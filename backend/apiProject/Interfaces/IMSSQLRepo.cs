using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IMSSQLRepo<T> where T:class
    {
        T Get(long id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null,
            string[] includeProperties = null
            );
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string[] includeProperties = null
            );
        void Add(T entity);
        void Remove(long id);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
