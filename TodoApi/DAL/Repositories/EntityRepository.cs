using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.DataRepository;

namespace TodoApi.DAL
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, new()
    {
        private TodoContext _dbContext;
        private DbSet<T> _dbSet;

        public EntityRepository(TodoContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
            _dbContext.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllEnumerable()
        {
            return _dbSet;
        }

        public void Insert(T item)
        {
            _dbSet.Add(item);
            _dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Update(T item)
        {
            _dbSet.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
