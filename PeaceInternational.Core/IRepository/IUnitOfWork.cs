using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.IRepository
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
