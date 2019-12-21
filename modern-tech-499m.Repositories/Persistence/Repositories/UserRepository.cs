using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {}

        protected override List<User> Maps(SQLiteDataReader reader)
        {
            List<User> users = new List<User>();
            if (!reader.HasRows)
                return users;
            while (reader.Read())
            {
                User user = new User
                {
                    Id = Convert.ToInt32(reader[nameof(User.Id)].ToString()),
                    Username = reader[nameof(User.Username)].ToString(),
                    BirthDate = DateTime.FromOADate(Convert.ToDouble(reader[nameof(User.BirthDate)].ToString())),
                    FirstName = reader[nameof(User.FirstName)].ToString(),
                    LastName = reader[nameof(User.LastName)].ToString(),
                    Patronymic = reader[nameof(User.Patronymic)].ToString(),
                    PasswordHash = (byte[])reader[nameof(User.PasswordHash)]
                };
                users.Add(user);
            }

            return users;
        }

        protected override User Map(SQLiteDataReader reader)
        { 
            User user = new User();
            if (!reader.HasRows)
                return user;
            if (!reader.Read())
                return user;
            user.Id = Convert.ToInt32(reader[nameof(User.Id)].ToString());
            user.Username = reader[nameof(User.Username)].ToString();
            user.BirthDate = DateTime.FromOADate(Convert.ToDouble(reader[nameof(User.BirthDate)].ToString()));
            user.FirstName = reader[nameof(User.FirstName)].ToString();
            user.LastName = reader[nameof(User.LastName)].ToString();
            user.Patronymic = reader[nameof(User.Patronymic)].ToString();
            user.PasswordHash = (byte[]) reader[nameof(User.PasswordHash)];

            return user;
        }

        protected override void UpdateCommandParameters(User entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(User.Id), entity.Id);
            InsertCommandParameters(entity, cmd);
        }

        protected override void InsertCommandParameters(User entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(User.Username), entity.Username);
            cmd.Parameters.AddWithValue(nameof(User.BirthDate), entity.BirthDate.ToOADate());
            cmd.Parameters.AddWithValue(nameof(User.FirstName), entity.FirstName);
            cmd.Parameters.AddWithValue(nameof(User.LastName), entity.LastName);
            cmd.Parameters.AddWithValue(nameof(User.Patronymic), entity.Patronymic);
            cmd.Parameters.AddWithValue(nameof(User.PasswordHash), entity.PasswordHash);
        }

        protected override void InsertManyCommandParameters(IEnumerable<User> entities, SQLiteCommand cmd)
        {
            int counter = 0;
            foreach (var entity in entities)
            {
                cmd.Parameters.AddWithValue($"{nameof(User.Username)}{counter}", entity.Username);
                cmd.Parameters.AddWithValue($"{nameof(User.BirthDate)}{counter}", entity.BirthDate.ToOADate());
                cmd.Parameters.AddWithValue($"{nameof(User.FirstName)}{counter}", entity.FirstName);
                cmd.Parameters.AddWithValue($"{nameof(User.LastName)}{counter}", entity.LastName);
                cmd.Parameters.AddWithValue($"{nameof(User.Patronymic)}{counter}", entity.Patronymic);
                cmd.Parameters.AddWithValue($"{nameof(User.PasswordHash)}{counter}", entity.PasswordHash);
                counter++;
            }
        }

        public override User Get(int id)
        {
            string sql = "select Id, Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash from Users where Id = @Id";
            return GetById(id, sql);
        }

        public override IEnumerable<User> GetAll()
        {
            string sql = "select Id, Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash from Users";
            return GetAll(sql);
        }

        public override IEnumerable<User> Find(Func<User, bool> predicate)
        {
            var allUsers = GetAll();
            return allUsers.Where(predicate);
        }

        public override int Add(User entity)
        {
            return TryExecuteFunction(() =>
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                string insertSql =
                    "insert into Users (Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash) VALUES (@Username, @BirthDate, @FirstName, @LastName, @Patronymic, @PasswordHash)";
                Insert(entity, insertSql, transaction);
                var id = (int) _connection.LastInsertRowId;
                _unitOfWork.Commit();
                return id;
            });
        }

        public override void AddRange(IEnumerable<User> entities)
        {
            TryExecuteAnyAction(() =>
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                StringBuilder insertSql = new StringBuilder(
                    "Insert into Users (Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash) VALUES ");
                int counter = 0;
                foreach (var _ in entities)
                {
                    insertSql.Append(
                        $"(@Username{counter}, @BirthDate{counter}, @FirstName{counter}, @LastName{counter}, @Patronymic{counter}, @PasswordHash{counter}), ");
                    counter++;
                }
                insertSql.Remove(insertSql.Length - 2, 2);
                InsertMany(entities, insertSql.ToString(), transaction);
                _unitOfWork.Commit();
            });
        }

        public override void Remove(User entity)
        {
            TryExecuteAnyAction(() =>
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                string deleteSql = "Delete from Users where Id = @Id";
                Delete(entity, deleteSql, transaction);
                _unitOfWork.Commit();
            });
        }

        public override void RemoveRange(IEnumerable<User> entities)
        {
            TryExecuteAnyAction(() =>
            {
                int counter = 0;
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                StringBuilder deleteSql = new StringBuilder("Delete from Users where Id IN (");
                foreach (var _ in entities)
                {
                    deleteSql.Append($@"Id{counter}, ");
                    counter++;
                }

                deleteSql.Remove(deleteSql.Length - 2, 2);
                deleteSql.Append(")");
                DeleteMany(entities, deleteSql.ToString(), transaction);
                _unitOfWork.Commit();
            });
        }

        public IEnumerable<User> GetUserFromGame(int id)
        {
            var getUsers1Sql =
                $"select Id, Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash from Users where Id IN (select Player1Id from GameInfo where Id = {id})";
            var users1 = GetAll(getUsers1Sql);
            var getUsers2Sql = $"select Id, Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash from Users where Id IN (select Player2Id from GameInfo where Id = {id})";
            var users2 = GetAll(getUsers2Sql);
            foreach (var user in users1)
            {
                yield return user;
            }

            foreach (var user in users2)
            {
                yield return user;
            }
        }

        public async Task<(LoginResult loginResult, User loggedUser)> LoginUser(string username, byte[] passwordHash)
        {
            var taskResult = await Task.Run(() =>
            {
                var searchForUserSql =
                    "select Id, Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash from Users where Username = @Username";

                var users = TryExecuteFunction(() =>
                {
                    using (var cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = searchForUserSql;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue(nameof(User.Username), username);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            return Maps(reader);
                        }
                    }
                });
                if (users.Count == 0)
                    return (LoginResult.UsernameNotExists, null);

                if (users.Count > 1)
                    throw new Exception("Multiple users with same username");

                if (!users[0].PasswordHash.SequenceEqual(passwordHash))
                    return (LoginResult.WrongPassword, null);

                return (LoginResult.Success, users[0]);
            });
            return taskResult;
        }

        public async Task<(RegisterResult registerResult, User registeredUser)> RegisterUser(User user)
        {
            var taskResult = await Task.Run(() =>
            {
                var searchForUserSql =
                    "select Id, Username, BirthDate, FirstName, LastName, Patronymic, PasswordHash from Users where Username = @Username";

                var users = TryExecuteFunction(() =>
                {
                    using (var cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = searchForUserSql;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue(nameof(User.Username), user.Username);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            return Maps(reader);
                        }
                    }
                });
                if (users.Count > 0)
                    return (RegisterResult.UsernameAlreadyExists, null);

                var newUserId = Add(user);

                user.Id = newUserId;

                return (RegisterResult.Success, user);
            });
            return taskResult;
        }
    }
}
