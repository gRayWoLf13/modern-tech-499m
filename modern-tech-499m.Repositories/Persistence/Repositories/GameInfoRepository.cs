﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Persistence.Repositories
{
    // https://forums.asp.net/t/2006343.aspx?Repository+pattern+without+ORM+
    //https://www.danylkoweb.com/Blog/creating-a-repository-pattern-without-an-orm-A9
    public class GameInfoRepository : BaseRepository<GameInfo>, IGameInfoRepository
    {
        public GameInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {}

        protected override List<GameInfo> Maps(SQLiteDataReader reader)
        {
            List<GameInfo> infos = new List<GameInfo>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    GameInfo info = new GameInfo
                    {
                        Id = Convert.ToInt32(reader[nameof(GameInfo.Id)].ToString()),
                        GameDate = Convert.ToDateTime(reader[nameof(GameInfo.GameDate)].ToString()),
                        Score = Convert.ToInt32(reader[nameof(GameInfo.Score)].ToString()),
                        GameFinished = Convert.ToBoolean(reader[nameof(GameInfo.GameFinished)].ToString()),
                        InternalGameData = reader[nameof(GameInfo.InternalGameData)].ToString(),
                        InternalSolverData = reader[nameof(GameInfo.InternalSolverData)].ToString()
                    };
                    infos.Add(info);
                }
            }
            return infos;
        }

        protected override GameInfo Map(SQLiteDataReader reader)
        {
            GameInfo info = new GameInfo();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    info.Id = Convert.ToInt32(reader[nameof(GameInfo.Id)].ToString());
                    info.GameDate = Convert.ToDateTime(reader[nameof(GameInfo.GameDate)].ToString());
                    info.Score = Convert.ToInt32(reader[nameof(GameInfo.Score)].ToString());
                    info.GameFinished = Convert.ToBoolean(reader[nameof(GameInfo.GameFinished)].ToString());
                    info.InternalGameData = reader[nameof(GameInfo.InternalGameData)].ToString();
                    info.InternalSolverData = reader[nameof(GameInfo.InternalSolverData)].ToString();
                }
            }
            return info;
        }

        protected override void GetByIdCommandParameters(int id, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(@"Id", id);
        }

        protected override void DeleteCommandParameters(int id, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(@"Id", id);
        }

        protected override void UpdateCommandParameters(GameInfo entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(entity.Id), entity.Id);
            cmd.Parameters.AddWithValue(nameof(entity.GameDate), entity.GameDate);
            cmd.Parameters.AddWithValue(nameof(entity.Score), entity.Score);
            cmd.Parameters.AddWithValue(nameof(entity.GameFinished), entity.GameFinished);
            cmd.Parameters.AddWithValue(nameof(entity.InternalGameData), entity.InternalGameData);
            cmd.Parameters.AddWithValue(nameof(entity.InternalSolverData), entity.InternalSolverData);
        }

        protected override void InsertCommandParameters(GameInfo entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(entity.GameDate), entity.GameDate);
            cmd.Parameters.AddWithValue(nameof(entity.Score), entity.Score);
            cmd.Parameters.AddWithValue(nameof(entity.GameFinished), entity.GameFinished);
            cmd.Parameters.AddWithValue(nameof(entity.InternalGameData), entity.InternalGameData);
            cmd.Parameters.AddWithValue(nameof(entity.InternalSolverData), entity.InternalSolverData);
        }

        public override GameInfo Get(int id)
        {
            string sql = "select * from GameInfo where id = @id";
            return GetById(id, sql);
        }

        public override IEnumerable<GameInfo> GetAll()
        {
            string sql = "select * from GameInfo";
            return GetAll(sql);
        }

        public override IEnumerable<GameInfo> Find(Func<GameInfo, bool> predicate)
        {
            var allGameInfos = GetAll();
            return allGameInfos.Where(predicate);
        }

        public override GameInfo SingleOrDefault(Func<GameInfo, bool> predicate)
        {
            var allGameInfos = GetAll();
            return allGameInfos.SingleOrDefault(predicate);
        }

        public override void Add(GameInfo entity)
        {
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                string insertSql = "Insert into GameInfo (GameDate, Score, GameFinished, InternalGameData, InternalSolverData) VALUES (@GameDate, @Score, @GameFinished, @InternalGameData, @InternalSolverData)";
                Insert(entity, insertSql, transaction);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void AddRange(IEnumerable<GameInfo> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public override void Remove(GameInfo entity)
        {
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                string insertSql = "Delete from GameInfo where Id = @Id";
                Delete(entity, insertSql, transaction);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void RemoveRange(IEnumerable<GameInfo> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public IEnumerable<GameInfo> GetTopScoreGames(int count)
        {
            var allGameInfos = GetAll();
            return allGameInfos.OrderBy(info => info.Score).Take(count);
        }

        public IEnumerable<GameInfo> GetGamesByUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}