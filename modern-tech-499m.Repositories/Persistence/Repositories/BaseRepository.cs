using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Persistence.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly SQLiteConnection _connection;
        protected readonly IUnitOfWork _unitOfWork;

        protected BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connection = unitOfWork.DataContext.Connection;
        }
        public abstract TEntity Get(int id);
        public abstract IEnumerable<TEntity> GetAll();
        public abstract IEnumerable<TEntity> Find(Func<TEntity, bool> predicate);
        public abstract void Add(TEntity entity);
        public abstract void AddRange(IEnumerable<TEntity> entities);
        public abstract void Remove(TEntity entity);
        public abstract void RemoveRange(IEnumerable<TEntity> entities);

        protected int Insert(TEntity entity, string insertSql, SQLiteTransaction transaction)
        {
            return TryExecuteFunction(() =>
            {
                int i;
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = insertSql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = transaction;
                    InsertCommandParameters(entity, cmd);
                    i = cmd.ExecuteNonQuery();
                }

                return i;
            });
        }

        protected int InsertMany(IEnumerable<TEntity> entities, string insertSql, SQLiteTransaction transaction)
        {
            return TryExecuteFunction(() =>
            {
                int i;
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = insertSql;
                    InsertManyCommandParameters(entities, cmd);
                    i = cmd.ExecuteNonQuery();
                }

                return i;
            });
        }

        protected int Update(TEntity entity, string updateSql, SQLiteTransaction transaction)
        {
            return TryExecuteFunction(() =>
            {
                int i;
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = updateSql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = transaction;
                    UpdateCommandParameters(entity, cmd);
                    i = cmd.ExecuteNonQuery();
                }

                return i;
            });
        }

        protected int Delete(TEntity entity, string deleteSql, SQLiteTransaction transaction)
        {
            return TryExecuteFunction(() =>
            {
                int i;
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = deleteSql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = transaction;
                    DeleteCommandParameters(entity, cmd);
                    i = cmd.ExecuteNonQuery();
                }

                return i;
            });
        }

        protected int DeleteMany(IEnumerable<TEntity> entities, string deleteSql, SQLiteTransaction transaction)
        {
            return TryExecuteFunction(() =>
            {
                int i;
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = deleteSql;
                    cmd.Transaction = transaction;
                    DeleteManyCommandParameters(entities, cmd);
                    i = cmd.ExecuteNonQuery();
                }

                return i;
            });
        }

        protected TEntity GetById(int id, string getByIdSql)
        {
            return TryExecuteFunction(() =>
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
            });
        }

        protected IEnumerable<TEntity> GetManyById(IEnumerable<int> ids, string getManyByIdSql)
        {
            return TryExecuteFunction(() =>
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
            });
        }

        protected IEnumerable<TEntity> GetAll(string getAllSql)
        {
            return TryExecuteFunction(() =>
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
            });
        }

        protected abstract List<TEntity> Maps(SQLiteDataReader reader);

        protected abstract TEntity Map(SQLiteDataReader reader);

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

        protected virtual void DeleteCommandParameters(TEntity entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
        }

        protected virtual void DeleteManyCommandParameters(IEnumerable<TEntity> entities, SQLiteCommand cmd)
        {
            int counter = 0;
            foreach (var entity in entities)
            {
                cmd.Parameters.AddWithValue($"@Id{counter}", entity.Id);
                counter++;
            }
        }

        protected abstract void UpdateCommandParameters(TEntity entity, SQLiteCommand cmd);

        protected abstract void InsertCommandParameters(TEntity entity, SQLiteCommand cmd);

        protected abstract void InsertManyCommandParameters(IEnumerable<TEntity> entities, SQLiteCommand cmd);

        protected TAny TryExecuteFunction<TAny>(Func<TAny> function)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        protected void TryExecuteAnyAction(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
