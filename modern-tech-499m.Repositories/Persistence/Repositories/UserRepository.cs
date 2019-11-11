using System;
using System.Collections.Generic;
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
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    User user = new User()
                    {
                        Id = Convert.ToInt32(reader[nameof(User.Id)].ToString()),
                        BirthDate = DateTime.FromOADate(Convert.ToDouble(reader[nameof(User.BirthDate)].ToString())),
                        FirstName = reader[nameof(User.FirstName)].ToString(),
                        LastName = reader[nameof(User.LastName)].ToString(),
                        Patronymic = reader[nameof(User.Patronymic)].ToString()
                    };
                    users.Add(user);
                }
            }

            return users;
        }

        protected override User Map(SQLiteDataReader reader)
        { 
            User user = new User();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    user.Id = Convert.ToInt32(reader[nameof(User.Id)].ToString());
                    user.BirthDate = DateTime.FromOADate(Convert.ToDouble(reader[nameof(User.BirthDate)].ToString()));
                    user.FirstName = reader[nameof(User.FirstName)].ToString();
                    user.LastName = reader[nameof(User.LastName)].ToString();
                    user.Patronymic = reader[nameof(User.Patronymic)].ToString();
                }
            }

            return user;
        }

        protected override void UpdateCommandParameters(User entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(User.Id), entity.Id);
            InsertCommandParameters(entity, cmd);
        }

        protected override void InsertCommandParameters(User entity, SQLiteCommand cmd)
        {
            cmd.Parameters.AddWithValue(nameof(User.BirthDate), entity.BirthDate.ToOADate());
            cmd.Parameters.AddWithValue(nameof(User.FirstName), entity.FirstName);
            cmd.Parameters.AddWithValue(nameof(User.LastName), entity.LastName);
            cmd.Parameters.AddWithValue(nameof(User.Patronymic), entity.Patronymic);
        }

        protected override void InsertManyCommandParameters(IEnumerable<User> entities, SQLiteCommand cmd)
        {
            int counter = 0;
            foreach (var entity in entities)
            {
                cmd.Parameters.AddWithValue(nameof(User.BirthDate), entity.BirthDate.ToOADate());
                cmd.Parameters.AddWithValue(nameof(User.FirstName), entity.FirstName);
                cmd.Parameters.AddWithValue(nameof(User.LastName), entity.LastName);
                cmd.Parameters.AddWithValue(nameof(User.Patronymic), entity.Patronymic);
                counter++;
            }
        }

        public override User Get(int id)
        {
            string sql = "select * from Users where Id = @Id";
            return GetById(id, sql);
        }

        public override IEnumerable<User> GetAll()
        {
            string sql = "select * from Users";
            return GetAll(sql);
        }

        public override IEnumerable<User> Find(Func<User, bool> predicate)
        {
            var allUsers = GetAll();
            return allUsers.Where(predicate);
        }

        public override User SingleOrDefault(Func<User, bool> predicate)
        {
            var allUsers = GetAll();
            return allUsers.SingleOrDefault(predicate);
        }

        public override void Add(User entity)
        {
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                string insertSql =
                    "insert into Users (BirthDate, FirstName, LastName, Patronymic) VALUES (@BirthDate, @FirstName, @LastName, @Patronymic)";
                Insert(entity, insertSql, transaction);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void AddRange(IEnumerable<User> entities)
        {
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                StringBuilder insertSql = new StringBuilder(
                    "Insert into Users (BirthDate, FirstName, LastName, Patronymic) VALUES ");
                int counter = 0;
                foreach (var entity in entities)
                {
                    insertSql.Append(
                        $"(@BirthDate{counter}, @FirstName{counter}, @LastName{counter}, @Patronymic{counter}), ");
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

        public override void Remove(User entity)
        {
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                string deleteSql = "Delete from Users where Id = @Id";
                Delete(entity, deleteSql, transaction);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void RemoveRange(IEnumerable<User> entities)
        {
            int counter = 0;
            try
            {
                SQLiteTransaction transaction = _unitOfWork.BeginTransaction();
                StringBuilder deleteSql = new StringBuilder("Delete from Users where Id IN (");
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

        public IEnumerable<User> GetUserFromGame(int id)
        {
            var getUsersSql =
                $"select * from Users where Id IN (select Player1Id, Player2Id from GameInfo where Id = {id})";
            var users = GetAll(getUsersSql);
            return users;
        }
    }
}
