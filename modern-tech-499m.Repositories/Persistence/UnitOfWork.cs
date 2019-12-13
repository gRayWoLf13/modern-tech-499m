using System;
using System.Data.SQLite;
using modern_tech_499m.Repositories.Core;

namespace modern_tech_499m.Repositories.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseContextFactory _factory;
        private IDatabaseContext _context;
        public SQLiteTransaction Transaction { get; private set; }

        public UnitOfWork(IDatabaseContextFactory factory)
        {
            _factory = factory;
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            _context?.Dispose();
        }

        public IDatabaseContext DataContext => _context ?? (_context = _factory.Context);
        public SQLiteTransaction BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new ArgumentException("Previous transaction was not finished", nameof(Transaction));
            }
            Transaction = _context.Connection.BeginTransaction();
            return Transaction;
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                    Transaction = null;
                }
            }
            else
            {
                throw new ArgumentException("Transaction not opened", nameof(Transaction));
            }
        }
    }
}
