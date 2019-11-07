using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        private SQLiteConnection _connection;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connection = unitOfWork.DataContext.Connection;
        }

        protected int Insert(T entity, string insertSql, SQLiteTransaction transaction)
        {
            var i = 0;
            try
            {
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = insertSql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = transaction;
                    InsertCommandParameters(entity, cmd);
                    i = cmd.ExecuteNonQuery();
                }

                return i;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected int Update(T entity, string updateSql, SQLiteTransaction transaction)
        {
            int i = 0;
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = updateSql;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;
                UpdateCommandParameters(entity, cmd);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        protected int Delete(T entity, string deleteSql, SQLiteTransaction transaction)
        {
            int i = 0;
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = deleteSql;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;
                DeleteCommandParameters(i, cmd);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        protected T GetById(int id, string getByIdSql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = getByIdSql;
                cmd.CommandType = CommandType.Text;
                GetByIdCommandParameters(id, cmd);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    return Map(reader);
                }
            }
        }

        protected IEnumerable<T> GetAll(string getAllSql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = getAllSql;
                cmd.CommandType = CommandType.Text;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    return Maps(reader);
                }
            }
        }

        protected abstract List<T> Maps(SQLiteDataReader reader);

        protected abstract T Map(SQLiteDataReader reader);

        protected abstract void GetByIdCommandParameters(int id, SQLiteCommand cmd);

        protected abstract void DeleteCommandParameters(int id, SQLiteCommand cmd);

        protected abstract void UpdateCommandParameters(T entity, SQLiteCommand cmd);

        protected abstract void InsertCommandParameters(T entity, SQLiteCommand cmd);
        public abstract T Get(int id);
        public abstract IEnumerable<T> GetAll();
        public abstract IEnumerable<T> Find(Func<T, bool> predicate);
        public abstract T SingleOrDefault(Func<T, bool> predicate);
        public abstract void Add(T entity);
        public abstract void AddRange(IEnumerable<T> entities);
        public abstract void Remove(T entity);
        public abstract void RemoveRange(IEnumerable<T> entities);
    }
}
