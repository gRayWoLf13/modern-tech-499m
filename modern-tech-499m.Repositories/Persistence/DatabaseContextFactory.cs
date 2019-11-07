using modern_tech_499m.Repositories.Core;

namespace modern_tech_499m.Repositories.Persistence
{
    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        private IDatabaseContext _dataContext;
        public void Dispose()
        {
            _dataContext?.Dispose();
        }

        public IDatabaseContext Context => _dataContext ?? (_dataContext = new DatabaseContext());
    }
}
