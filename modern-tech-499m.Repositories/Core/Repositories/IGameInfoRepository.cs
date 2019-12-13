using System.Collections.Generic;
using modern_tech_499m.Repositories.Core.Domain;

namespace modern_tech_499m.Repositories.Core.Repositories
{
    public interface IGameInfoRepository : IRepository<GameInfo>
    {
        IEnumerable<GameInfo> GetTopScoreGames(int count);
        IEnumerable<GameInfo> GetGamesByUser(int id);
    }
}
