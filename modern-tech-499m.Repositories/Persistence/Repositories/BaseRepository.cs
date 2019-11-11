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
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity, new()
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

        protected int InsertMany(IEnumerable<T> entities, string insertSql, SQLiteTransaction transaction)
        {
            var i = 0;
            try
            {
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = insertSql;
                    InsertManyCommandParameters(entities, cmd);
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
                DeleteCommandParameters(entity, cmd);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        protected int DeleteMany(IEnumerable<T> entities, string deleteSql, SQLiteTransaction transaction)
        {
            int i = 0;
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = deleteSql;
                cmd.Transaction = transaction;
                DeleteManyCommandParameters(entities, cmd);
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

        protected IEnumerable<T> GetManyById(IEnumerable<int> ids, string getManyByIdSql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = getManyByIdSql;
                cmd.CommandType = CommandType.Text;
                GetManyByIdCommandParameters(ids, cmd);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    return Maps(reader);
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

        protected virtual void GetByIdCommandParameters(int id, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Id", id);
        }

        protected virtual void GetManyByIdCommandParameters(IEnumerable<int> ids, SQLiteCommand cmd)
        {
            int counter = 0;
            foreach (int id in ids)
            {
                cmd.Parameters.AddWithValue($"@Id{counter}", id);
                counter++;
            }
        }

        protected virtual void DeleteCommandParameters(T entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
        }

        protected virtual void DeleteManyCommandParameters(IEnumerable<T> entities, SQLiteCommand cmd)
        {
            int counter = 0;
            foreach (var entity in entities)
            {
                cmd.Parameters.AddWithValue($"@Id{counter}", entity.Id);
                counter++;
            }
        }

        protected abstract void UpdateCommandParameters(T entity, SQLiteCommand cmd);

        protected abstract void InsertCommandParameters(T entity, SQLiteCommand cmd);

        protected abstract void InsertManyCommandParameters(IEnumerable<T> entities, SQLiteCommand cmd);
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
