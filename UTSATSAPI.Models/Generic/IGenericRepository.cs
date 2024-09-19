using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        Task<IEnumerable<T>> GetAllByCondition(Expression<Func<T, bool>> expression);
        Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetWithRawSql(string query, params object[] strParams);




    }
}
