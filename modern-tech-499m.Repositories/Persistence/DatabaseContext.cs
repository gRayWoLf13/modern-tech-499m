using System.Data.SQLite;
using modern_tech_499m.Repositories.Core;
using System.Configuration;
using System.Data;

namespace modern_tech_499m.Repositories.Persistence
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public DatabaseContext()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SQLiteConnection(_connectionString);
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                return _connection;
            }
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
