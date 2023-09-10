using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CartApi.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IMapper Mapper { get; }
        IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sourceCollection);
        Task<IList<T>> GetAll(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Expression<Func<T, bool>> expression = null,
            List<string> includes = null
        );

        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            List<string> includes = null
            );

        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);

        Task<T> Insert(T entity);

        Task<int> InsertRange(IEnumerable<T> entities);

        Task<T> Update(T entity);

        Task<bool> DeleteRange(IEnumerable<T> entities);

        Task<bool> DeleteById(int id);

        Task<bool> Delete(T entity);
    }
}
