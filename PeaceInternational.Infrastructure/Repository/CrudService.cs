using Microsoft.EntityFrameworkCore;
using PeaceInternational.Core;
using PeaceInternational.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PeaceInternational.Infrastructure.Repository
{
    public class CrudService<TEntity> : ICrudService<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public CrudService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Delete(TEntity entity)
        {
            var result = _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public TEntity Get(int? id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression).SingleOrDefault();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<TEntity> GetAsync(int? id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>().Where(expression).SingleOrDefaultAsync();
        }

        public int? Insert(TEntity entity)
        {
            var result = _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public async Task<int?> InsertAsync(TEntity entity)
        {
            var sql = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return sql.Entity.Id;
        }

        public int Update(TEntity entity)
        {
            var result = _context.Set<TEntity>().Update(entity).Property(p => p.Id).IsModified = false;
            _context.SaveChanges();
            return entity.Id;
        }
    }
}
