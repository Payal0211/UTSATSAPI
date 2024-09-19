using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly TalentConnectAdminDBContext _dbContext;
        internal DbSet<T> dbSet;
        protected GenericRepository(TalentConnectAdminDBContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            this.dbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync().ConfigureAwait(false);
        }

        public async Task Add(T entity)
        {
            if (entity != null)
                await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            if (entities != null)
                await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            if (entity != null)
                _dbContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            if (entities != null)
                _dbContext.Set<T>().RemoveRange(entities);
        }
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<IEnumerable<T>> GetAllByCondition(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> Query = dbSet;
            var model = Query.Where(expression);

            return await model.ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> Query = dbSet;
            var model = Query.Where(expression);
            return model.FirstOrDefault();
            // return await model.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetWithRawSql(string query, params object[] strParams)
        {
            return _dbContext.Set<T>().FromSqlRaw(string.Format("{0} {1}", query, strParams)).ToList();
        }

    }
}
