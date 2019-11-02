using PeaceInternational.Core.IRepository;
using PeaceInternational.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Infrastructure.Repository
{
    public class UnitOfWorK :IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWorK(ApplicationDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _context.Database.RollbackTransaction();
        }
    }
}
