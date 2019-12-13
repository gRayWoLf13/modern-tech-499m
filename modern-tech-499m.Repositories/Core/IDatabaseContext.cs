using System;
using System.Data.SQLite;

namespace modern_tech_499m.Repositories.Core
{
    public interface IDatabaseContext : IDisposable
    {
        SQLiteConnection Connection { get; }
    }
}
