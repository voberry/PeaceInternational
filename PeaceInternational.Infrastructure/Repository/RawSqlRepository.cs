using PeaceInternational.Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PeaceInternational.Infrastructure;

namespace goalza.booking.infrastructure.Repository
{
    public class RawSqlRepository : IRawSqlRepository
    {
        private readonly ApplicationDbContext _context;

        public RawSqlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> FromSql<T>(string sql, params object[] parameters) where T : class
        {
            return _context.Query<T>().FromSql(sql, parameters);
        }
    }
}
