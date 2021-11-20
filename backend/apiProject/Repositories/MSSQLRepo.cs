using apiProject.DBContexts;
using apiProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace apiProject.Repositories
{
    public class MSSQLRepo<T> : IMSSQLRepo<T> where T : class
    {
        private readonly MSSQLDbContext _dbContext;
        internal DbSet<T> _dbSet;

        public MSSQLRepo(MSSQLDbContext db)
        {
            _dbContext = db;
            _dbSet = db.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get(long id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(includeProperties != null)
            {
                foreach(string prop in includeProperties)
                {
                    query = query.Include(prop);
                }
            }
            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string[] includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (string prop in includeProperties)
                {
                    query = query.Include(prop);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(long id)
        {
            T entity = _dbSet.Find(id);
            Remove(entity);

        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}
