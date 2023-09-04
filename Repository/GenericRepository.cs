using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CartApi.Data;
using CartApi.Interfaces;

namespace Cart.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly CartDbContext _context;
        private readonly DbSet<T> _db;
        private readonly IMapper _mapper;

        public GenericRepository(CartDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _db = _context.Set<T>();
        }

        public IMapper Mapper => _mapper;

        

        public IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sourceCollection)
        {
            var destinationCollection = new List<TDestination>();

            foreach (var sourceItem in sourceCollection)
            {
                var destinationItem = Mapper.Map<TSource, TDestination>(sourceItem);
                destinationCollection.Add(destinationItem);
            }

            return destinationCollection;
        }


        public async Task<bool> Delete(int id)
        {

            var entity = await _db.FindAsync(id);
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _db.Attach(entity).State = EntityState.Deleted;
            int numberOfChanges = await _context.SaveChangesAsync();
            return numberOfChanges > 0;
        }

        public async Task<bool> DeleteRange(IEnumerable<T> entities)
        {
            if(!entities.Any()) throw new ArgumentNullException(nameof(entities));
            _db.RemoveRange(entities);
            int numberOfChanges = await  _context.SaveChangesAsync();
            return numberOfChanges > 0;

        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if(includes != null)
            {
                foreach(var include in includes)
                {
                   query =  query.Include(include);
                }
            }
            var result = await query.AsNoTracking().FirstOrDefaultAsync(expression);
            return result;
        }

        public async Task<IList<T>> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if(includes != null)
            {
                foreach(var include in includes) query.Include(include);
            }
            if (orderBy != null) orderBy(query);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var include in includes) {
                    query = query.Include(include);
                };
            }
            return await query.AsNoTracking().ToListAsync();
        }

       

        public async Task<T> Insert(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _db.Add(entity);
            var numberOfChanges = await  _context.SaveChangesAsync();
            return numberOfChanges > 0 ? entity : null;
        }

        public async Task<int> InsertRange(IEnumerable<T> entities)
        {
            if (!entities.Any()) throw new ArgumentNullException(nameof(entities));

            _db.AddRange(entities);
            var numberOfChanges =  await _context.SaveChangesAsync();
            return numberOfChanges;
        }

        public async Task<T> Update(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            _db.Attach(entity).State = EntityState.Modified;
            int numberOfChanges = await _context.SaveChangesAsync(); 
            return numberOfChanges > 0 ? entity : null;
        }
    }
}
