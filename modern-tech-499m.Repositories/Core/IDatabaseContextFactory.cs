using System;

namespace modern_tech_499m.Repositories.Core
{
    public interface IDatabaseContextFactory : IDisposable
    {
        IDatabaseContext Context { get; }
    }
}
