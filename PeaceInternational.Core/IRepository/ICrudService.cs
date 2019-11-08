using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PeaceInternational.Core.IRepository
{
    public interface ICrudService<TEntity> where TEntity : BaseEntity
    {
        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);
        int Update(TEntity entity);
        TEntity Get(int? id);
        TEntity Get(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetAsync(int? id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
        int Delete(TEntity entity);       
    }
}
