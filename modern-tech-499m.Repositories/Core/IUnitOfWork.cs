using System;
using System.Data.SQLite;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IDatabaseContext DataContext { get; }
        SQLiteTransaction BeginTransaction();
        void Commit();
    }
}
