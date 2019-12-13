using System;
using System.Data.SQLite;

namespace modern_tech_499m.Repositories.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IDatabaseContext DataContext { get; }
        SQLiteTransaction BeginTransaction();
        void Commit();
    }
}
