using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
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
        { }

        protected int? StringToNullableInt(string value)
        {
            if (string.IsNullOrEmpty(value) || string.Equals(value.ToLower(), "null"))
                return null;
            return Convert.ToInt32(value);
        }

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
                        GameDate = DateTime.FromOADate(Convert.ToDouble(reader[nameof(GameInfo.GameDate)].ToString())),
                        Player1Id = StringToNullableInt(reader[nameof(GameInfo.Player1Id)].ToString()),
                        Player2Id = StringToNullableInt(reader[nameof(GameInfo.Player2Id)].ToString()),
                        Score = Convert.ToInt32(reader[nameof(GameInfo.Score)].ToString()),
                        GameFinished = Convert.ToInt32(reader[nameof(GameInfo.GameFinished)].ToString()) == 1,
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
                    info.GameDate = DateTime.FromOADate(Convert.ToDouble(reader[nameof(GameInfo.GameDate)].ToString()));
                    info.Player1Id = StringToNullableInt(reader[nameof(GameInfo.Player1Id)].ToString());
                    info.Player2Id = StringToNullableInt(reader[nameof(GameInfo.Player2Id)].ToString());
                    info.Score = Convert.ToInt32(reader[nameof(GameInfo.Score)].ToString());
                    info.GameFinished = Convert.ToInt32(reader[nameof(GameInfo.GameFinished)].ToString()) == 1;
                    info.InternalGameData = reader[nameof(GameInfo.InternalGameData)].ToString();
                    info.InternalSolverData = reader[nameof(GameInfo.InternalSolverData)].ToString();
                }
            }
            return info;
        }

        protected override void UpdateCommandParameters(GameInfo entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(entity.Id), entity.Id);
            InsertCommandParameters(entity, cmd);
        }

        protected override void InsertCommandParameters(GameInfo entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(entity.GameDate), entity.GameDate.ToOADate());
            cmd.Parameters.AddWithValue(nameof(entity.Player1Id), entity.Player1Id);
            cmd.Parameters.AddWithValue(nameof(entity.Player2Id), entity.Player2Id);
            cmd.Parameters.AddWithValue(nameof(entity.Score), entity.Score);
            cmd.Parameters.AddWithValue(nameof(entity.GameFinished), entity.GameFinished ? 1 : 0);
            cmd.Parameters.AddWithValue(nameof(entity.InternalGameData), entity.InternalGameData);
            cmd.Parameters.AddWithValue(nameof(entity.InternalSolverData), entity.InternalSolverData);
        }

        protected override void InsertManyCommandParameters(IEnumerable<GameInfo> entities, SQLiteCommand cmd)
        {
            int counter = 0;
            foreach (var entity in entities)
            {
                cmd.Parameters.AddWithValue($"{nameof(entity.GameDate)}{counter}", entity.GameDate.ToOADate());
                cmd.Parameters.AddWithValue($"{nameof(entity.Player1Id)}{counter}", entity.Player1Id);
                cmd.Parameters.AddWithValue($"{nameof(entity.Player2Id)}{counter}", entity.Player2Id);
                cmd.Parameters.AddWithValue($"{nameof(entity.Score)}{counter}", entity.Score);
                cmd.Parameters.AddWithValue($"{nameof(entity.GameFinished)}{counter}", entity.GameFinished ? 1 : 0);
                cmd.Parameters.AddWithValue($"{nameof(entity.InternalGameData)}{counter}", entity.InternalGameData);
                cmd.Parameters.AddWithValue($"{nameof(entity.InternalSolverData)}{counter}", entity.InternalSolverData);
                counter++;
            }
        }

        public override GameInfo Get(int id)
        {
            string sql = "select Id, GameDate, Player1Id, Player2Id, Score, GameFinished, InternalGameData, InternalSolverData from GameInfo where Id = @Id";
            return GetById(id, sql);
        }

        public override IEnumerable<GameInfo> GetAll()
        {
            string sql = "select Id, GameDate, Player1Id, Player2Id, Score, GameFinished, InternalGameData, InternalSolverData from GameInfo";
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
                string insertSql = "Insert into GameInfo (GameDate, Player1Id, Player2Id, Score, GameFinished, InternalGameData, InternalSolverData) VALUES (@GameDate, @Player1Id, @Player2Id, @Score, @GameFinished, @InternalGameData, @InternalSolverData)";
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
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                StringBuilder insertSql = new StringBuilder(
                    "Insert into GameInfo (GameDate, Player1Id, Player2Id, Score, GameFinished, InternalGameData, InternalSolverData) VALUES ");
                int counter = 0;
                foreach (var entity in entities)
                {
                    insertSql.Append(
                        $"(@GameDate{counter}, @Player1Id{counter}, @Player2Id{counter}, @Score{counter}, @GameFinished{counter}, @InternalGameData{counter}, @InternalSolverData{counter}), ");
                    counter++;
                }
                insertSql.Remove(insertSql.Length - 1, 1);
                InsertMany(entities, insertSql.ToString(), transaction);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void Remove(GameInfo entity)
        {
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                string deleteSql = "Delete from GameInfo where Id = @Id";
                Delete(entity, deleteSql, transaction);
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
            int counter = 0;
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                StringBuilder deleteSql = new StringBuilder("Delete from GameInfo where Id IN (");
                foreach (var entity in entities)
                {
                    deleteSql.Append($@"Id{counter}, ");
                }

                deleteSql.Remove(deleteSql.Length - 2, 2);
                deleteSql.Append(")");
                DeleteMany(entities, deleteSql.ToString(), transaction);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<GameInfo> GetTopScoreGames(int count)
        {
            var allGameInfos = GetAll();
            return allGameInfos.OrderBy(info => info.Score).Take(count);
        }

        public IEnumerable<GameInfo> GetGamesByUser(int id)
        {
            var getManySql = "select Id from GameInfo where Player1Id = @Id OR Player2Id = @Id";
            var gameInfoIds = GetAll(getManySql);
            return gameInfoIds;
        }
    }
}
