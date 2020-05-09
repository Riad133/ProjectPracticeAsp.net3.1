using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DLL.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DLL.UnitOfWork
{
    public interface IRepositoryBase<T>where T: class
    {
        Task createAsync(T entry);
        void UpdateAsync(T entry);
        void DeleteAsync(T entry);
        Task<T> GetAAsync(Expression<Func<T, bool>> expression = null);
        Task<List<T>>  GetListAsync(Expression<Func<T,bool>> expression=null);
        IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null);

    }

    public class RepositoryBase<T> : IRepositoryBase<T>where T: class
    {
        private readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task createAsync(T entry)
        {
            await _context.Set<T>().AddAsync(entry);
        }

        public void UpdateAsync(T entry)
        {
            _context.Set<T>().Update(entry);
        }

        public void DeleteAsync(T entry)
        {
            _context.Set<T>().Remove(entry);
        }

        public async Task<T> GetAAsync(Expression<Func<T, bool>> expression = null)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
            
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _context.Set<T>().ToListAsync();
            }
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public  IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return  _context.Set<T>().AsQueryable().AsNoTracking();
            }
            return  _context.Set<T>().Where(expression).AsQueryable().AsNoTracking();
        }
    }
}